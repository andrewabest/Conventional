Conventional [![Build status](https://ci.appveyor.com/api/projects/status/b34y026n60v9oe16?svg=true)](https://ci.appveyor.com/project/andrewabest/conventional)
============

A suite of convention tests to run over sets of types _or whole solutions_ to make sure your duckies are all in a row.

## To install from NuGet

    Install-Package Best.Conventional 
    
## Sample Usage

Standard Syntax
```c#
new[] { typeof(MyType), typeof(MyOtherType) }
    .MustConformTo(
        Convention.PropertiesMustHavePublicGetters.And(
        Convention.PropertiesMustHavePublicSetters))
    .WithFailureAssertion(Assert.Fail);
```

Fluent Syntax
```c#
new[] { typeof(MyType), typeof(MyOtherType) }
    .MustConformTo(Convention.PropertiesMustHavePublicGetters)
    .AndMustConformTo(Convention.PropertiesMustHavePublicSetters)
    .WithFailureAssertion(Assert.Fail);
```

Or Mix It Up!
```c#
new[] { typeof(MyType), typeof(MyOtherType) }
    .MustConformTo(
        Convention.PropertiesMustHavePublicGetters.And(
        Convention.PropertiesMustHavePublicSetters))
    .AndMustConformTo(Convention.MustHaveADefaultConstructor)
    .WithFailureAssertion(Assert.Fail);
```

## Supplied Conventions

- Properties must have public getters
- Properties must have public setters
- Properties must have protected setters
- Properties must have private setters
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
- Must not use DateTime.Now
- Must not use DateTimeOffset.Now
- Exceptions thrown must be derived from specified type
- Must instantiate properties of specified type in default constructor
- All properties must be instantiated during construction

## Supplied Conventions (.Net 4.5 Only)

- Void methods must not be async
- Async methods must have 'Async' suffix
- Libraries should call Task.ConfigureAwait(false) to prevent deadlocks

## Solution Convention Sample Usage

Standard Syntax
```c#
ThisSolution
    .MustConformTo(Convention.MustOnlyContainToDoAndNoteComments)
    .WithFailureAssertion(Assert.Fail);
```

## Supplied Solution Conventions

- Must only contain Todo and Note comments
- Must only contain informative comments

### 2015-04-07 0.1.x Release Breaking Changes

* The Conventional.Async assembly has been merged into the core Conventional assembly, and async conventions are now accessed through the same Convention class as the core conventions.

* The Conventional.Cecil assembly has been merged into the core Conventional assembly, and Mono.Cecil based conventions are now accessed through the same Convention class as the core conventions.

The aim of these changes was to simplify usage of Conventional - there is now one central location for conventions, and only one package regardless of the conventions you wish to enforce. The only side effect of the changes is the core package is now dependent on Mono.Cecil.
