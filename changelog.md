# Changelog

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
