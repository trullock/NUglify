# Changelog

## 1.9.5
- Adds support for Optional Chaining
- Adds support for Numeric Separators

## 1.9.4
- Fixes bug with `crlf` immediately before a closing template literal

## 1.9.3 (15 September 2020)
- Fixes bug with single argument functions with a default parameter value

## 1.9.2 (9 September 2020)
- ~Fixes bug with `crlf` immediately before a closing template literal~ this wasnt properly fixed, see 1.9.4 for the real fix
- Fixes bug with `async` shortand object initializer syntax

## 1.9.1 (25 August 2020)
- Improves handling of rogue closing tags in HTML, manifesting at least in how HtmlToText behaves. There shouldn't be any regressions but we've gone up a minor revision just in case.

## 1.8.1 (24 August 2020)
- Removes deprecated lineCount from SourceMaps
- Adds `MakePathsRelative` true/false to `V3SourceMap` to control path munging
- Fixes JS bug with getters and setters, primarily when used within `defineProperty`

## 1.7.2 (24 August 2020)
- Adds support for HTML attribute removal
- Adds support for HTML attribute reordering

## 1.7.1 (24 August 2020)
- Changes (fixes) how HTML5 comments are handled. Previously valid/invalid comments may now be handled differently (correctly)

## 1.6.6 (21 August 2020)
- Fixes overloading of `async` keyword

## 1.6.5 (19 August 2020)
- Adds Exponent `**` and Exponent Assign `**=` support
- Adds Null Coalesce `??` operator support

## 1.6.4 (6 July 2020)
- Fixes issue with const name redeclaration

## 1.6.3 (10 June 2020)
- Fixes further module variable bugs ([(PR #113](https://github.com/xoofx/NUglify/pull/113))

## 1.6.2 (10 June 2020)
- Fixes module variable bugs ([(PR #112](https://github.com/xoofx/NUglify/pull/112))

## 1.6.1 (10 June 2020)
- Updates Nuget package details

## 1.6.0 (10 June 2020)
- Fix for whitespace preceeding a textarea ([(PR #103)](https://github.com/xoofx/NUglify/pull/103))
- Include application/ld+json in list of processable javascript type blocks ([(PR #102)](https://github.com/xoofx/NUglify/pull/102))
- Fixes infinite loop when dealing with malformed switch statements  ([(PR #99](https://github.com/xoofx/NUglify/pull/99))
- Fixes arrow function parenthesis issue ([(PR #98](https://github.com/xoofx/NUglify/pull/98))
- Fixes arrow functions + sourcemaps bug ([(PR #97](https://github.com/xoofx/NUglify/pull/97))
- Fixes spread operator bug ([(PR #91](https://github.com/xoofx/NUglify/pull/91))
- Fixes issue with mutliple await return ([(PR #90](https://github.com/xoofx/NUglify/pull/90))
- Fixes issue with parameterless arrow functions ([(PR #89](https://github.com/xoofx/NUglify/pull/89))
- Fixes bug with KeyFrames in css ([(PR #107](https://github.com/xoofx/NUglify/pull/107))
- Fixes bug with ShortBooleanAttribute=true incorrectly removes value tags from inputs ([(PR #108](https://github.com/xoofx/NUglify/pull/108))
- Fixes bug with css calc, flex and 0px ([(PR #109](https://github.com/xoofx/NUglify/pull/109))

## 1.5.14 (9 Mar 2020)
- Fix async/await JS minifier
- Add support for CSS @supports
- Adding a options to turoff escape decoding in CSS files.

## 1.5.13 (04 Jun 2019)
- Allow async function expressions ([(PR #65)](https://github.com/xoofx/NUglify/pull/65))
- Fix for(let x of y.prop) ([(PR #69)](https://github.com/xoofx/NUglify/pull/69))
- Fix #71 "Invalid arrow-function arguments" error on Firefox ([(PR #72)](https://github.com/xoofx/NUglify/pull/72))

## 1.5.12 (21 Aug 2018)
- ECMAScript 6 Support for computed names in object property definitions
- Add support for async and await.
- Add support for netstandard2.0

## 1.5.11 (6 May 2018)
- Support for ES6 arrow functions ([PRs](https://github.com/xoofx/NUglify/pulls?utf8=%E2%9C%93&q=is%3Apr+author%3Asamjudson+created%3A%3C2018-05-07))

## 1.5.10 (5 Apr 2018)
- Support for CSS variables ([(PR #46)](https://github.com/xoofx/NUglify/pull/46))
- Implementing Razor escape support in CSS ([(PR #41)](https://github.com/xoofx/NUglify/pull/41))

## 1.5.9
- Respect self-closing tags with "RemoveOptionalTags" option disabled

## 1.5.8
- Fix Check for break outside of a loop but inside a label block (#31)

## 1.5.7
- Fix issue in Html compact with pre/textarea tags not keeping whitespaces      

## 1.5.6
- Add new HtmlSettings.KeepTags to selectively keep a list of optional start/end tags even when  settings.RemoveOptionalTags is true

## 1.5.5
- Fix possible NullReferenceException when using HtmlSettings.RemoveJavaScript = true

## 1.5.4
- Fix regression when collapsing spaces introduced previously

## 1.5.3
- Add option HtmlSettings.KeepOneSpaceWhenCollapsing to keep at least one space when collapsing them

## 1.5.2
- Add support for keeping knockout comments
- Don't minify (0,eval)('this') expressions

## 1.5.1
- Fix bug while minimizing HTML style CSS attribute

## 1.5.0
- Add support to dotnet RTM, switch to netstandard1.6
