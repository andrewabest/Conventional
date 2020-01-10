Conventional [![Build status](https://ci.appveyor.com/api/projects/status/b34y026n60v9oe16?svg=true)](https://ci.appveyor.com/project/andrewabest/conventional) 
[![NuGet](https://img.shields.io/nuget/v/Best.Conventional.svg)](https://www.nuget.org/packages/Best.Conventional/)
============

Conventional provides a suite of ready-made tests for enforcing conventions within your types, assemblies, solutions and databases to make sure your duckies are all in a row.

Conventional targets .NET Standard 2.0, and as of Conventional 7.x, Conventional ❤'s Linux!

![](https://raw.github.com/andrewabest/Conventional/master/duck.png)

## To install from NuGet

    Install-Package Best.Conventional

Looking for Roslyn-based conventions? Check out [the documentation](https://github.com/andrewabest/Conventional/wiki/Roslyn-Conventions) for more information.

Conventional.Roslyn does not target .NET Standard 2.0 due to there being [no official .NET Core support](https://github.com/dotnet/roslyn/issues/17974) for `MSBuildWorkspace`, a core building block of Roslyn. 

## Documentation

To get started with Conventional, please check out [the wiki](https://github.com/andrewabest/Conventional/wiki) for a comprehensive list of included conventions, sample usages, and configuration information.

## Examples

Not sure how to get started with Conventional? Check out [the sample repository](https://github.com/andrewabest/Conventional.Samples) which contains a bunch of real-world usage examples

## Contributing

Conventional's test suite requires a default named `.\SQLEXPRESS` instance. If you have another instance you would like to use for development, create a copy of `development.settings.example` in the solution root and rename to `development.settings`, and supply your own connection string.

## License

Licensed under the terms of the [MS-PL](https://opensource.org/licenses/MS-PL) license
