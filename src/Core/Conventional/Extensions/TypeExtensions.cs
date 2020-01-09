using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Conventional.Extensions
{
    public static class TypeExtensions
    {
        internal static bool IsGenericImplementation(this Type type)
        {
            return (type.BaseType != null && type.BaseType.IsGenericType) ||
                   type.GetInterfaces().Any(i => i.IsGenericType);
        }

        public static IEnumerable<Type> WhereTypes(this IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            var types = assemblies.SelectMany(x => x.GetExportedTypes()).Where(predicate).ToArray();

            return types;
        }
    }
}