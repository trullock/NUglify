var a = "a";
var b = "b";
var c;
var d = "d";

// the declaration for f should NOT be combined with the declaration for e,
// even though they are adjacent while parsing. They are in different control structures!
if ( d ) var e = "e";
var f = "f";

var g = "g";
