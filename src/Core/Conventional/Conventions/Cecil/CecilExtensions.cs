using System;
using System.Collections.Generic;
using Mono.Cecil;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Conventional.Conventions.Cecil
{
    public static class CecilExtensions
    {
        public static bool IsAssignableTo(this TypeDefinition type, Type derivedType)
        {
            return
                IsAssignableToInterface(type, derivedType) || IsAssignableToBase(type, derivedType);
        }

        public static bool IsAssignableToInterface(this TypeDefinition type, Type derivedType)
        {
            var cecilFormattedTypeName = derivedType.FullName.Replace("+", "/");

            return type.Interfaces.Any(
                i =>
                    i.InterfaceType.FullName.Equals(cecilFormattedTypeName) ||
                    i.InterfaceType.Resolve().Interfaces.Any(x => IsAssignableToInterface(x.InterfaceType.Resolve(), derivedType)));
        }
        
        public static bool IsAssignableToBase(this TypeDefinition type, Type derivedType)
        {
            var cecilFormattedTypeName = derivedType.FullName.Replace("+", "/");

            return (type.BaseType != null && type.BaseType.FullName.Equals(cecilFormattedTypeName)) ||
                   (type.BaseType != null && IsAssignableToBase(type.BaseType.Resolve(), derivedType));
        }

        public static IEnumerable<PropertyDefinition> GetPropertiesOfType(this TypeDefinition typeDefinition, Type type)
        {
            return
                typeDefinition.Properties.Where(
                    p => p.PropertyType.Name.StartsWith(type.Name));
        }

        public static TypeDefinition ToTypeDefinition(this Type type)
        {
            return (TypeDefinition)ModuleDefinition.ReadModule(type.Assembly.Location, new ReaderParameters { AssemblyResolver = new ConventionalAssemblyResolver() })
                .GetType(type.FullName, true);
        }

        public static IEnumerable<TypeDefinition> GetAllBaseTypeDefinitions(this TypeDefinition typeDefinition)
        {
            do
            {
                typeDefinition = typeDefinition.BaseType as TypeDefinition;
                if (typeDefinition != null)
                {
                    yield return typeDefinition;
                }
            } while (typeDefinition != null && typeDefinition.BaseType.Name != "Object");
        }

        /// <summary>
        /// An async method will have an AsyncStateMachine attribute pointing to the generated async state machine type 
        /// for example [AsyncStateMachine(typeof(AsyncMethods.<DownloadHtmlAsyncTask>d__0))]
        /// see: http://www.codeproject.com/Articles/535635/Async-Await-and-the-Generated-StateMachine
        /// </summary>
        public static TypeDefinition GetAsyncStateMachineType(this MethodDefinition provider)
        {
            var asyncStateMachineAttribute = provider.GetAttribute<AsyncStateMachineAttribute>();
            return (TypeDefinition)asyncStateMachineAttribute.ConstructorArguments[0].Value;
        }

        public static TypeDefinition GetIteratorStateMachineType(this MethodDefinition provider)
        {
            var iteratorStateMachineAttribute = provider.GetAttribute<IteratorStateMachineAttribute>();
            return (TypeDefinition) iteratorStateMachineAttribute.ConstructorArguments[0].Value;
        }

        public static bool HasAttribute<TAttribute>(this MethodDefinition subject) where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(subject) != null;
        }

        public static CustomAttribute GetAttribute<TAttribute>(this MethodDefinition subject) where TAttribute : Attribute
        {
            return subject.CustomAttributes.FirstOrDefault(attribute => attribute.AttributeType.Name == typeof(TAttribute).Name);
        }
    }

    public sealed class AssemblyResolutionException : Exception
    {
        public AssemblyNameReference AssemblyReference { get; }

        public AssemblyResolutionException (AssemblyNameReference reference)
            : base ($"Failed to resolve assembly: '${reference}'")
        {
            AssemblyReference = reference;
        }
    }
}