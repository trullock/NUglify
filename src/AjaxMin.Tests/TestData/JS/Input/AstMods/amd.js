
define("foo", ["ralph"], function (ralph) { alert(ralph); });

var x = 42;

define("bar", function () { return { one: "two" }; });
define("foo", ["bar"], function (bar) { alert(bar.one); });
