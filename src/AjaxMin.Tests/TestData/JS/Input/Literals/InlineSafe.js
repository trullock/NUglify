
// split these for safe, leave it alone for not safe
var foo1 = "</script>";
var foo2 = "]]>";

var foo3 = "while(0)</script>";
var foo4 = "foobar]]>";

// leave these alone for safe; combine them for not safe
var bar1 = "</script" + ">";
var bar2 = "</scrip" + "t>";
var bar3 = "</scr" + "ipt>";
var bar4 = "]]" + '>';
var bar5 = "]" + "]>";


