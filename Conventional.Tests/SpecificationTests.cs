using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Conventional.Tests
{
    public class PropertiesShouldHavePublicGettersAndSettersMock
    {
        public string Public { get; set; }
        public string PrivateGet { private get; set; }
        public string PrivateSet { get; private set; }
    }

    public class SpecificationTests
    {
        [Test]
        public void PropertiesShouldHavePublicGettersAndSettersConformanceSpecification_FailsOnPrivatePropertyAccessors()
        {
            new[] { typeof(PropertiesShouldHavePublicGettersAndSettersMock) }
                .MustAllConformTo(Specification.PropertiesShouldHavePublicGettersAndSetters)
                .WithFailureAssertion(Assert.Fail);
        }
    }
}
