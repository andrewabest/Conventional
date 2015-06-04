Conventional [![Build status](https://ci.appveyor.com/api/projects/status/b34y026n60v9oe16?svg=true)](https://ci.appveyor.com/project/andrewabest/conventional)
============

Conventional provides a suite of ready-made tests for enforcing conventions within your types, solutions and databases to make sure your duckies are all in a row.

## To install from NuGet

    Install-Package Best.Conventional 
    
## Type Conventions

### Sample Usage

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

### Supplied Type Conventions

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

### Supplied Type Conventions (.Net 4.5 Only)

- Void methods must not be async
- Async methods must have 'Async' suffix
- Libraries should call Task.ConfigureAwait(false) to prevent deadlocks

## Solution Conventions

### Sample Usage

```c#
ThisSolution
    .MustConformTo(Convention.MustOnlyContainToDoAndNoteComments)
    .WithFailureAssertion(Assert.Fail);
```

### Supplied Solution Conventions

- Must only contain Todo and Note comments
- Must only contain informative comments

## Database Conventions

### Sample Usage

```c#
TheDatabase
    .WithConnectionString("YourConnectionString")
    .MustConformTo(Convention.AllIdentityColumnsMustBeNamedTableNameId)
    .WithFailureAssertion(Assert.Fail);
```

### Supplied Database Conventions

- All identity columns must be named tablenameId
- All tables must have a clustered index