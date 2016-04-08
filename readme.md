# NUglify [![Build status](https://ci.appveyor.com/api/projects/status/vep1cdnie9cls48p?svg=true)](https://ci.appveyor.com/project/xoofx/ajaxmin)

NUglify enables you to improve the performance of your web applications by reducing the size of your Cascading Style Sheet and JavaScript files.

This repository is a fork of the [Microsoft Ajax Minifier](http://ajaxmin.codeplex.com/)

> NOTE: The repository is under migration/refactoring. See the [Background](#background) section below for more information.

## Documentation

The original documentation of the project is available [here](doc/readme.md)

## Background

You may wonder why this fork? Here are a few reasons:

- Ron Logan announced that he is no longer able to maintain this project [here](http://ajaxmin.codeplex.com/discussions/587925)
- While ASP.NET 5.0+ is now relying on the **node.js** ecosystem for its client side tooling (e.g: minify), NUglify is **still useful** in scenarios where we need to access this tooling from a .NET application (not necessarily an ASP one) without having to install another developer platform. I can't believe that Ajaxmin could be left behind while it is still valuable!
- The original code was also hosted on codeplex making it a much less appealing code source platform to collaborate compare to github.
- It was not possible to keep the history of the commits, as the Ajaxmin SVN codeplex seems to be completely down
- I wanted the library to be compatible with CoreCLR/dotnet-cli scenarios
- I may also take the time to cleanup a bit the code. See the [Status](#status) section below

> **NOTE**: This is an open-source project and I don't claim to be the maintainer of this project, so contributors and PR are much welcome!

## Status

- [x] Migrate code from codeplex
- [x] Port code to `xproj` + `project.json` for .NET3.5, .NET4.x+ and NETCore
- [ ] Rename namespaces
- [ ] Rename files and put them in a single class per file as much as possible
- [ ] Publsih a nuget package
- [ ] Try to evaluate a some point if we couldn't collaborate with a project like [Jint](https://github.com/sebastienros/jint) to leverage on a common JavacScript parser infrastructure

## License

This software is released under the [BSD-Clause 2 license](http://opensource.org/licenses/BSD-2-Clause).
The original Microsoft Ajax Minifier was released under the [Apache 2.0 license](http://www.apache.org/licenses/LICENSE-2.0)

## Author

Microsoft Ajax Minifier was created and maintained by Ron Logan, with contributions from Eugene Chigirinskiy, Rafael Correa, Kristoffer Henriksson, and Marcin Dobosz.

Nuglify was ported and refactored to github by Alexandre Mutel aka [xoofx](http://xoofx.com)
