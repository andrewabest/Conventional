Conventional [![Build status](https://ci.appveyor.com/api/projects/status/b34y026n60v9oe16?svg=true)](https://ci.appveyor.com/project/andrewabest/conventional)
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
- Must have a default non-public constructor
- Must have appropriate constructors
- Must have attribute
- Must not take a dependency on
- Requires a corresponding implementation of (T)
- Enumerable properties must be eager loaded
- Collection properties must be immutable
- All properties must be immutable

Conventional.Async
============

A suite of async related convention tests. Conventional.Async is packaged within the base Best.Conventional nuget package for projects targeting .Net 4.5+.
    
## Sample Usage

```c#
new[] { myAssembly }
    .WhereTypes(x => x) // Insert funky type narrowing predicate of choice here
    .MustConformTo(AsyncConvention.VoidMethodsMustNotBeAsync)
    .WithFailureAssertion(Assert.Fail);
```

## Supplied Conventions

- Void methods must not be async
- Async methods must have 'Async' suffix

Conventional.Cecil
============

A suite of Mono.Cecil-based convention tests, for when standard reflection just wont cut it.

## To install from NuGet

    Install-Package Best.Conventional.Cecil
    
## Sample Usage

```c#
new[] { myAssembly }
    .WhereTypes(x => x) // Insert funky type narrowing predicate of choice here
    .MustConformTo(CecilConvention.MustNotUseDateTimeNow)
    .WithFailureAssertion(Assert.Fail);
```

## Supplied Conventions

- Must not use DateTime.Now
- Must not use DateTimeOffset.Now
- Exceptions thrown must be derived from specified type
- Must instantiate properties of specified type in default constructor
