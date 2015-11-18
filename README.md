Conventional [![Build status](https://ci.appveyor.com/api/projects/status/b34y026n60v9oe16?svg=true)](https://ci.appveyor.com/project/andrewabest/conventional)
============

Conventional provides a suite of ready-made tests for enforcing conventions within your types, assemblies, solutions and databases to make sure your duckies are all in a row.

Conventional's backlog can be found here https://trello.com/b/kay7a8Ya/conventional

![](https://raw.github.com/andrewabest/Conventional/master/duck.png)

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
- Must not have a property of type
- Requires a corresponding implementation of (T)
- Enumerable properties must be eager loaded
- Collection properties must be immutable
- All properties must be immutable
- Must not resolve current time via DateTime
- Must not use DateTimeOffset.Now
- Exceptions thrown must be derived from specified type
- Must instantiate properties of specified type in default constructor
- All properties must be instantiated during construction
- Must have matching embedded resource

### Supplied Type Conventions (.Net 4.5 Only)

- Void methods must not be async
- Async methods must have 'Async' suffix
- Libraries should call Task.ConfigureAwait(false) to prevent deadlocks

### Known offenders and Doomsday tests

If you are in a scenario where you want to acknowledge previous types that break a convention you are introducing - but prevent more from being introduced, `WithKnownOffenders` can be supplied

```c#
new[] { typeof(OldType), typeof(NewType) }
    .WithKnownOffenders(1)
    .MustConformTo(Convention.NameMustStartWith("New"));
```

Taking this further, if you want to enforce that the existing offenders be compliant by a given date, you can supply `ByDoomsday`, along with an optional `WithWarningWithin` to warn as the date approaches, and `WithMessage` to supply extra context to the output

```c#
new[] { typeof(OldType), typeof(NewType) }
    .WithKnownOffenders(1)
    .ByDoomsday(new DateTime(2015,11,18))
    .WithWarningWithin(TimeSpan.FromDays(14))
    .WithMessage("Names must start with 'New'")
    .MustConformTo(Convention.NameMustStartWith("New"));
```

When using `Known Offenders` or `Doomsday` tests, you *must* make sure to configure the default failure and warning assertions using [Conventional's configuration](#configuration)

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

## Assembly Conventions

### Sample Usage

Strongly typed
```c#
typeof(MyType)
	.Assembly
	.MustConformTo(Convention.MustNotReferenceDllsFromBinOrObjDirectories)
    .WithFailureAssertion(Assert.Fail);
```

Pattern matched
```c#
TheAssembly
	.WithNameMatching("MySolution.MyProject")
	.MustConformTo(Convention.MustNotReferenceDllsFromBinOrObjDirectories)
    .WithFailureAssertion(Assert.Fail);
```

### Supplied Assembly Conventions

- Must not reference dlls from bin or obj directories
- Must have all files be embedded resources

## Configuration

### Default failure assertion

To configure a default failure assertion method, use the default failure assertion callback (example using NUnit) in your global test setup
```c#
ConventionConfiguration.DefaultFailureAssertionCallback = Assert.Fail
```

Alternatively, you can assert failure by using the fluent syntax displayed in the above samples.

### Default warning assertion

To configure a default warning assertion method, use the default warning assertion callback (example using NUnit) in your global test setup
```c#
ConventionConfiguration.DefaultWarningAssertionCallback = Assert.Inconclusive
```

### Default current date resolution

Certain Conventional features rely on Conventional being able to resolve the current date, which by default it will via `DateTime.UtcNow`. If this does not work in your scenario, set the default current date resolver in your global test setup 
```c#
ConventionConfiguration.DefaultCurrentDateResolver = DateTime.Now
```

### Dealing with a funky folder structure?

Conventional assumes that your solution root will be three folders (..\\..\\..\\) from where the tests are running. If it is not set your solution root in your global test setup
```c#
KnownPaths.SolutionRoot = @"c:\projects\MySolutionRoot"
```

## Examples

Not sure how to get started with Conventional? Check out [The sample repository](https://github.com/andrewabest/Conventional.Samples) which contains a bunch of real-world usage examples
