Conventional
============

A suite of basic convention tests to run over types to make sure your duckies are all in a row.

To install from NuGet

    Install-Package Best.Conventional 
    
Sample Usage

`new[] { typeof(MyType), typeof(MyOtherType) }
    .MustConformTo(Convention.PropertiesShouldHavePublicGetters.And(Convention.PropertiesShouldHavePublicSetters))
    .WithFailureAssertion(Assert.Fail);`
