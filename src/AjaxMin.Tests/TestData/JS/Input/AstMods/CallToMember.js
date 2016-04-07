// change from a member-bracket notation to a member-dot notation
// if the argument inside the brackets is a string literal which can
// be an identifier but not a keyword


var obj;

alert(obj["property"]); // should get changed to obj.property
alert(obj["while"]); // keyword -- do not change

// if the string literals are concatenated, we should be
// able to change this to obj.property
alert(obj["prop" + "erty"]);
// and multiple concatenations
alert(obj["p" + "rop" + "er" + "ty"]);
// and this is multiple concats, but one is not a string, so don't convert it
alert(obj[p + "rop" + "er" + "ty"]);

// the combined string would be a keyword, so don't convert it
alert(obj["wh" + "il" + "e"]);

// the combined string would not be a valid identifier
alert(obj["1" + "world"]);

// nested inside other member nodes
var a = window.obj["foo"].bar(); // should be var a=window.obj.foo.bar

// but keywords that are okay to be identifiers *could* be convertable
// to property access mode, but let's not do it. The developer for some reason
// wanted it to be a bracket-call, so just leave it as-is
alert(obj["enum"]);

// some Unicode category characters, although okay in the ECMA-262 spec, cause
// some browsers to throw a syntax error if they are in identifiers. Therefore we're
// NOT going to perform this transformation if the potential identifier contains
// any characters not in the standard ASCII range.
obj["\u00e3\u0ae9\u0ae9"];

// these should NOT be changed because they aren't string literals
obj[null];
obj[undefined];
obj[true];
obj[false];
obj[3.1415927];

// these should NOT be changed because they would be "special" identifiers
obj["null"];
obj["true"];
obj["false"];

// this should NOT be changed because it wouldn't be an identifier at all
obj["3.1415927"];

// this CAN be changed because "undefined" is not a reserved identifier
obj["undefined"];
