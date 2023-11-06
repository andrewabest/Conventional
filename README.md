Conventional [![Build status](https://ci.appveyor.com/api/projects/status/b34y026n60v9oe16/branch/master?svg=true)](https://ci.appveyor.com/project/andrewabest/conventional/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Best.Conventional.svg)](https://www.nuget.org/packages/Best.Conventional/)
============

Conventional provides a suite of ready-made tests for enforcing conventions within your types, assemblies, solutions and databases to make sure your duckies are all in a row.

Conventional targets .NET Standard 2.0, and as of Conventional 7.x, Conventional ❤'s Linux!

![](https://raw.github.com/andrewabest/Conventional/master/duck.png)

## To install from NuGet

    Install-Package Best.Conventional

## Documentation

To get started with Conventional, please check out [the wiki](https://github.com/andrewabest/Conventional/wiki) for a comprehensive list of included conventions, sample usages, and configuration information.

## Examples

Not sure how to get started with Conventional? Check out [the sample repository](https://github.com/andrewabest/Conventional.Samples) which contains a bunch of real-world usage examples

## Roslyn-based conventions

[![Build status](https://ci.appveyor.com/api/projects/status/qrxqsfh0d5jwc5ns/branch/master?svg=true)](https://ci.appveyor.com/project/andrewabest/conventional-roslyn/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Best.Conventional.Roslyn.svg)](https://www.nuget.org/packages/Best.Conventional.Roslyn/)

As of [This PR](https://github.com/andrewabest/Conventional/pull/73) Roslyn conventions are now supported on Dotnet Core.

    Install-Package Best.Conventional.Roslyn

Rolsyn-based conventions target `net6.0`. Check out [the documentation](https://github.com/andrewabest/Conventional/wiki/Roslyn-Conventions) for more information.

## Contributing

Conventional's test suite requires a default named `.\SQLEXPRESS` instance. If you have another instance you would like to use for development, create a copy of `development.settings.example` in the solution root and rename to `development.settings`, and supply your own connection string.

## License

Licensed under the terms of the [MS-PL](https://opensource.org/licenses/MS-PL) license
