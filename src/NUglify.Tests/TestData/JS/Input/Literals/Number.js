// negative zero is a valid numeric value different than positive zero
// positive zero is the same as zero with no sign
var dz1 = 0, dz2 = -0, dz3 = +0;

// decimal
var d1=10, d2=1.23e10, d3=1.2e+5, d4=1.3e-5, d5=1.45, d6=-42, d7=-1.414e-0, d8=.666, d9=+5, d10=+.6, d11=+1.66e10;

// hexadecimal
var h1=0x19, h2=0xa, h3=0xffff, h4=0x0001, h5=0xfedcba9876, h6=-0xff;

// octal
// because octals are deprecated and may have cross-browser issues, we should leave them
// as-is in our code IF they would be different decoded as decimal. 
// That fifth one, though, isn't an octal!
var o1 = 0377, o2 = 012, o3 = 00, o4 = 06, o5 = 089, o6 = 07 + 03;

// literal representing the maximum numeric value. should suggest developer replace with Number.MAX_VALUE
// while leaving the value literal crunched but still numeric
var max = 1.7976931348623157E+308;
// literal representing the minimum numeric value. should suggest developer replace with Number.MIN_VALUE
// while leaving the value literal crunched but still numeric
var min = -1.7976931348623157E+308;

// these values are commonly used to represent min and max values, but they are NOT correct.
// browsers evaluate them as Infinity and -Infinity. We need to throw an error telling the developer
// that these values cause an overflow, but we should echo them unchanged into the output --
// not crunched at all or replace with Numeric.POSITIVE_INFINITY or anything in case that would cause
// unforeseen side-effects
var pos = 1.79769313486232E308;
var neg = -1.79769313486232E+308;

// and this one is just a blatant greater-than-max infinity value that should throw an error
// but be replaced as-is without crunching
var tooBig = 1E999;

// boundary conditions for floating-point
var a = 123456789012345678901;
var b = 12345678901234567891;
var c = 123456789012345678901 + 12345678901234567891;

// overflow for an object-literal field name works in browsers. for instance,
// if obj[1e999] = 2 then obj[1e999] === obj[1e309] evaluates to true because
// the index is just Number.POSITIVE_INFINITY.
var obj = {1e969: "pos inf"};

// this would be formatted to e-notation as 7.2e6 - but we can squeeze another byte out of it
// by getting rid of the decimal point: 72e5
var foo = 7200000;

// try some ES6 octal literals. these can get converted and combined.
var oct1 = 0o3774;
var oct2 = 0O46 + 0o7;

// and some ES6 binary literals. these can get converted and combined.
var bin1 = 0b00000110;
var bin2 = 0B00010011101010011001000101010011 & 0x0000ffff;
