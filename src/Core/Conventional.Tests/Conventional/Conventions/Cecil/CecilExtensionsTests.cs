using System.Collections;
using System.Collections.Generic;
using Conventional.Conventions.Cecil;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Cecil
{
    public class CecilExtensionsTests
    {
        private class HasBaseInterface : List<string> {}
        
        private class DerivedFromDerieved : Derived {}
        
        private class Derived : Base{}
        
        private class Base {}
        
        [Test]
        public void IsAssignableTo_DelvesTypeHeirarchy_ToFindInterfaces()
        {
            typeof(HasBaseInterface).ToTypeDefinition().IsAssignableTo(typeof(IEnumerable)).Should().BeTrue();
        }
        
        [Test]
        public void IsAssignableTo_DelvesTypeHeirarchy_ToFindBases()
        {
            typeof(DerivedFromDerieved).ToTypeDefinition().IsAssignableTo(typeof(Base)).Should().BeTrue();
        }
    }
}