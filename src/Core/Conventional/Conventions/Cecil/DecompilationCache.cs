using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
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
        static readonly object CecilMutex; // Note: Cecil is not internally thread-safe in some places

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
            var typeInstructions = GetOrAdd(InstructionsByType, type, GetInstructionsFor);

            if (includeStateMachine)
            {
                var stateMachineTypeInstructions = GetOrAdd(StateMachineInstructionsByType, type, GetStateMachineInstructions);

                return typeInstructions
                    .Union(stateMachineTypeInstructions)
                    .Distinct()
                    .ToArray();
            }

            return typeInstructions;
        }

        static IReadOnlyCollection<Instruction> GetInstructionsFor(Type type)
        {
            var typeDefinition = GetOrAdd(TypeDefinitionsByType, type, GetTypeDefinitionFor);

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
            var typeDefinition = GetOrAdd(TypeDefinitionsByType, type, GetTypeDefinitionFor);

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

            // Note: This isn't an exact match. 
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

                var typeDefinition = parentTypeDefinition.NestedTypes.SingleOrDefault(td => td.Name == type.Name)
                                     ?? throw new Exception($"Could not find nested type {type.Name} in declaring type {parentTypeDefinition.FullName}");

                return typeDefinition;
            }
            
            var result = GetOrAdd(
                TypeDefinitionsByType,
                type,
                t =>
                {
                    var moduleDefinition = GetOrAdd(ModuleDefinitionsByAssemblyLocation, t.Assembly.Location, GetModuleDefinitionByLocation);
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
        
        static TValue GetOrAdd<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (dictionary.TryGetValue(key, out var value)) return value;

            value = valueFactory(key);
            // Note: valueFactory can re-entrantly call other conventional methods, and it could have added `key`
            return dictionary.GetOrAdd(key, value);
        }
    }
}