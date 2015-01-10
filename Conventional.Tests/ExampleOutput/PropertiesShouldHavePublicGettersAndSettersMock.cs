namespace Conventional.Tests.ExampleOutput
{
    public class PropertiesShouldHavePublicGettersAndSettersMock
    {
        public string Public { get; set; }
        public string PrivateGet { private get; set; }
        public string PrivateSet { get; private set; }
    }
}