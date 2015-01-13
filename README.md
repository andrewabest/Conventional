Conventional
============

A suite of basic convention tests to run over types to make sure your duckies are all in a row.

## To install from NuGet

    Install-Package Best.Conventional 
    
## Sample Usage

Standard Syntax
```c#
new[] { typeof(MyType), typeof(MyOtherType) }
    .MustConformTo(
        Convention.PropertiesShouldHavePublicGetters.And(
        Convention.PropertiesShouldHavePublicSetters))
    .WithFailureAssertion(Assert.Fail);
```

Fluent Syntax
```c#
new[] { typeof(MyType), typeof(MyOtherType) }
    .MustConformTo(Convention.PropertiesShouldHavePublicGetters)
    .AndMustConformTo(Convention.PropertiesShouldHavePublicSetters)
    .WithFailureAssertion(Assert.Fail);
```

Or Mix It Up!
```c#
new[] { typeof(MyType), typeof(MyOtherType) }
    .MustConformTo(
        Convention.PropertiesShouldHavePublicGetters.And(
        Convention.PropertiesShouldHavePublicSetters))
    .AndMustConformTo(Convention.MustHaveADefaultConstructor)
    .WithFailureAssertion(Assert.Fail);
```

## Supplied Conventions

- Properties must have public getters
- Properties must have public setters
- Name must start with
- Name must end with
- Must live in namespace
- Must have a default constructor
- Must have attribute
- Must not take a dependency on
