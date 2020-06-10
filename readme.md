# NUglify [![Build status](https://ci.appveyor.com/api/projects/status/b2o4d1j7nhsttd7l?svg=true)](https://ci.appveyor.com/project/trullock/nuglify)  [![NuGet](https://img.shields.io/nuget/v/NUglify.svg)](https://www.nuget.org/packages/NUglify/)

<img align="right" width="160px" height="160px" src="images/nuglify.png">

NUglify provides minify and compression methods for CSS, JavaScript and HTML files.

This repository is a fork of the [Microsoft Ajax Minifier](http://ajaxmin.codeplex.com/) + additional features (e.g: HTML compressor) 

# Notice

> This project is an attempt to keep alive the original AjaxMin project developed by Ron Logan & contributors.
> I (xoofx) highly invite people to help to fix issues they find in this project, as I can't afford myself to be the sole maintainer of this project.
> In other words, Pull Requests (PR) are much welcome and most likely the only way for you to get your issue fixed and to help to keep this project alive.
> Thank you!

## Features

- JS minification (from AjaxMin)
- Css minification (from AjaxMin)
- HTML minification (**!!New and Exclusive!!**)
  - Can help to reduce by 5-10% a standard HTML document
  - Supports several minifications methods: remove comments, collapse whitespaces, remove optional tags (p, li...), remove quoted attributes, decode HTML entities, compress inline style and script using NUglify
  - No regex involved, full HTML parser
  - Supports HTML5, works best on valid HTML documents (but can still work on invalid documents)
  - Similar to the popular [html-minifier in JS](https://github.com/kangax/html-minifier)
  - Super fast and GC friendly parser and minifier, 10x times faster than existing html compressor for .NET
  - Method `Uglify.HtmlToText` that allows to extract the text from an HTML document
- Compatible with `NET3.5`, `NET4.0+` and `CoreCLR` (`netstandard1.3+`)
> NOTE: The repository is under migration/refactoring. See the [Background](#background) section below for more information.

## Download

NUglify is available as a NuGet package: [![NuGet](https://img.shields.io/nuget/v/NUglify.svg)](https://www.nuget.org/packages/NUglify/)

## Usage

The main entry point for the API is the `Uglify` class:

For JavaScript:

```csharp
var result = Uglify.Js("var x = 5; var y = 6;");
Console.WriteLine(result.Code);   // prints: var x=5,y=6
```

For Css:

```csharp
var result = Uglify.Css("div { color: #FFF; }");
Console.WriteLine(result.Code);   // prints: div{color:#fff}
```

For Html:

```csharp
var result = Uglify.Html("<div>  <p>This is <em>   a text    </em></p>   </div>");
Console.WriteLine(result.Code);   // prints: <div><p>This is <em>a text</em></div>
```

Extract text from Html:

```csharp
var result = Uglify.HtmlToText("<div>  <p>This is <em>   a text    </em></p>   </div>");
Console.WriteLine(result.Code);   // prints: This is a text
```

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

> **NOTE**: This is an open-source project and I don't claim to be the (only) maintainer of this project, so contributors and PR are much welcome!

## Status

- [x] Migrate code from codeplex
- [x] Port code to `xproj` + `project.json` for .NET3.5, .NET4.x+ and NETCore
- [x] Rename namespaces
- [x] Rename files and put them in a single class per file as much as possible
- [x] Publsih a nuget package
- [x] Add HTML compressor/minify
- [ ] Try to evaluate a some point if we couldn't collaborate with a project like [Jint](https://github.com/sebastienros/jint) to leverage on a common JavaScript parser infrastructure
- [ ] Try to evaluate to leverage on [AngleSharp](https://github.com/AngleSharp/AngleSharp) for the HTML/Css minification

## License

This software is released under the [BSD-Clause 2 license](http://opensource.org/licenses/BSD-2-Clause).
The original Microsoft Ajax Minifier was released under the [Apache 2.0 license](http://www.apache.org/licenses/LICENSE-2.0)

## Author

Microsoft Ajax Minifier was created and maintained by Ron Logan, with contributions from Eugene Chigirinskiy, Rafael Correa, Kristoffer Henriksson, and Marcin Dobosz.

Nuglify was ported and refactored to github by Alexandre Mutel aka [xoofx](http://xoofx.com)

## Credits

The logo Monster is by [Joel McKinney](https://thenounproject.com/joel.mckinney/) from the Noun Project
