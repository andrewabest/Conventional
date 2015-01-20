using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace Conventional.Cecil
{
    public static class CecilExtensions
    {
        public static Type AsReflectedType(this TypeDefinition typeDefinition)
        {
            return Type.GetType(typeDefinition.FullName + ", " + typeDefinition.Module.Assembly.FullName);
        }

        public static Type AsReflectedType(this TypeReference typeReference)
        {
            return Type.GetType(typeReference.FullName + ", " + typeReference.Module.Assembly.FullName);
        }

        public static TypeDefinition ToTypeDefinition(this Type type)
        {
            return (TypeDefinition)ModuleDefinition.ReadModule(type.Assembly.Location)
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
}