using System;
using System.Collections.Generic;
using Mono.Cecil;
using System.Linq;

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