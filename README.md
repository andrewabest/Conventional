Conventional
============

A suite of basic convention tests to run over types to make sure your duckies are all in a row.

## To install from NuGet

    Install-Package Best.Conventional 
    
## Sample Usage

```c#
new[] { typeof(MyType), typeof(MyOtherType) }
    .MustConformTo(
        Convention.PropertiesShouldHavePublicGetters.And(
        Convention.PropertiesShouldHavePublicSetters))
    .WithFailureAssertion(Assert.Fail);
```

## Supplied Conventions

- Properties should have public getters
- Properties should have public setters
- Name should start with
- Name should end with
- Should live in namespace
- Should have a default constructor
- Should have attribute
- Should not take a dependency on
