using System;
using System.Reflection;
using Conventional.Conventions.Cecil;
using Mono.Cecil;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Cecil
{
    public class ConventionalAssemblyResolverTests
    {
        [Test]
        public void CanResolvePathToAssembly()
        {
            var resolver = new ConventionalAssemblyResolver();
            var assembly = Assembly.GetExecutingAssembly();
            var ignored = Version.Parse("1.0.0");
            var reference = new AssemblyNameReference(assembly.FullName, ignored);
            Assert.DoesNotThrow(() => resolver.Resolve(reference));
        }
    }
}
