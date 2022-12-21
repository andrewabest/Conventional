using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Conventional.Conventions.Cecil
{
    public static class DecompilationCache
    {
        static readonly ConcurrentDictionary<string, ModuleDefinition> ModuleDefinitionsByAssemblyLocation;
        static readonly ConcurrentDictionary<Type, TypeDefinition> TypeDefinitionsByType;
        static readonly ConcurrentDictionary<Type, IReadOnlyCollection<Instruction>> InstructionsByType;
        static readonly ConcurrentDictionary<Type, IReadOnlyCollection<Instruction>> StateMachineInstructionsByType;
        static readonly object CecilMutex; // Cecil is not internally thread-safe in some places

        static DecompilationCache()
        {
            ModuleDefinitionsByAssemblyLocation = new ConcurrentDictionary<string, ModuleDefinition>();
            TypeDefinitionsByType = new ConcurrentDictionary<Type, TypeDefinition>();
            InstructionsByType = new ConcurrentDictionary<Type, IReadOnlyCollection<Instruction>>();
            StateMachineInstructionsByType = new ConcurrentDictionary<Type, IReadOnlyCollection<Instruction>>();
            CecilMutex = new object();
        }

        public static IReadOnlyCollection<Instruction> InstructionsFor(Type type, bool includeStateMachine = true)
        {
            var typeInstructions = InstructionsByType.GetOrAdd(type, GetInstructionsFor(type));

            if (includeStateMachine)
            {
                var stateMachineTypeInstructions = StateMachineInstructionsByType.GetOrAdd(type, GetStateMachineInstructions(type));

                return typeInstructions
                    .Union(stateMachineTypeInstructions)
                    .Distinct()
                    .ToArray();
            }

            return typeInstructions;
        }

        static IReadOnlyCollection<Instruction> GetInstructionsFor(Type type)
        {
            var typeDefinition = TypeDefinitionsByType.GetOrAdd(type, GetTypeDefinitionFor);

            lock (CecilMutex)
            {
                var typeInstructions =
                    typeDefinition
                        .Methods
                        .Where(method => method.HasBody)
                        .SelectMany(method => method.Body.Instructions)
                        .Distinct()
                        .ToArray();

                return typeInstructions;
            }
        }

        static IReadOnlyCollection<Instruction> GetStateMachineInstructions(Type type)
        {
            var typeDefinition = TypeDefinitionsByType.GetOrAdd(type, GetTypeDefinitionFor);

            lock (CecilMutex)
            {
                var stateMachineInstructions =
                    typeDefinition
                        .Methods
                        .Where(x => x.HasAttribute<AsyncStateMachineAttribute>())
                        .SelectMany(x => x.GetAsyncStateMachineType().Methods.Where(method => method.HasBody))
                        .SelectMany(method => method.Body.Instructions)
                        .Union(
                            typeDefinition
                                .Methods
                                .Where(x => x.HasAttribute<IteratorStateMachineAttribute>())
                                .SelectMany(x => x.GetIteratorStateMachineType().Methods.Where(method => method.HasBody))
                                .SelectMany(method => method.Body.Instructions))
                        .Distinct()
                        .ToArray();

                return stateMachineInstructions;
            }
        }

        public static MethodDefinition GetMethodDefinitionFor(MethodInfo method)
        {
            var typeDefinition = GetTypeDefinitionFor(method.DeclaringType!);

            //TODO This isn't an exact match. Please feel free to tighten the criteria if you need to.  -andrewh 22/9/2021
            var methodDefinition = typeDefinition.Methods
                .Where(m => m.Name == method.Name)
                .First(m => m.Parameters.Count == method.GetParameters().Length);
            return methodDefinition;
        }

        public static TypeDefinition GetTypeDefinitionFor(Type type)
        {
            if (type.IsNested && type.DeclaringType != null)
            {
                var parentTypeDefinition = GetTypeDefinitionFor(type.DeclaringType);

                // ReSharper disable once ReplaceWithSingleCallToSingleOrDefault
                var typeDefinition = parentTypeDefinition.NestedTypes.Where(td => td.Name == type.Name).SingleOrDefault()
                    ?? throw new Exception($"Could not find nested type {type.Name} in declaring type {parentTypeDefinition.FullName}");

                return typeDefinition;
            }

            var result = TypeDefinitionsByType.GetOrAdd(
                type,
                t =>
                {
                    var moduleDefinition = ModuleDefinitionsByAssemblyLocation.GetOrAdd(t.Assembly.Location, GetModuleDefinitionByLocation);
                    var typeDefinition = (TypeDefinition)moduleDefinition.GetType(type.FullName, true);
                    if (typeDefinition is null)
                    {
                        var additionalMessage = 
                            string.Join(
                                Environment.NewLine, 
                                moduleDefinition.Types.Select(td => $" - {td.FullName}"));
                        
                        throw new InvalidOperationException($"Could not find type {t.FullName} in {t.Assembly.Location}.{Environment.NewLine}Types we can see: {additionalMessage}");
                    }

                    return typeDefinition;
                });

            return result;
        }

        static ModuleDefinition GetModuleDefinitionByLocation(string location)
        {
            var moduleDefinition = ModuleDefinition.ReadModule(
                location,
                new ReaderParameters
                {
                    AssemblyResolver = new ConventionalAssemblyResolver()
                });

            return moduleDefinition;
        }
    }
}