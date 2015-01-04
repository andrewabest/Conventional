using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Conventional.Tests
{
    public class MockModel
    {
        public string Name { get; private set; }
    }

    public class SpecificationTests
    {
        [Test]
        public void Blah()
        {
            new[] {typeof (MockModel)}.MustAllConformTo(
                new PropertiesShouldHavePublicGettersAndSettersConformanceSpecification())
                .WithFailureAssertion(Assert.Fail);
        }

    }
}
