var a;

// different browsers treat strings of negative hex values differently when converting to number.
// IE follows the ECMA spec: a sign is *not* part of the HexIntegerLiteral grammar. But some other
// important browsers will return a negative number. Because of the difference, we shouldn't evaluate
// any numeric literal operations where an operand is a string with a hex number format preceded by a sign.

// IE and Safari would evaluate this as b=NaN (as per the ECMA spec), but others (Firefox, Chrome, Opera)
// will evaluate as b=40
var b = 24 - "-0x10";       // 24-"-0x10"

// IE and Safari would evaluate this correctly as c=NaN, but others as c=10
var c = 26 - "+0x10";       // 26-"+0x10"

// everyone agrees on this one: d=10
var d = 26 - "0x10";        // 10

//
// string concatenation
//

a = "foo" + "bar";          // "foobar"
b = "foo" + 10;             // "foo10"
c = 10 + "foo";             // "10foo"
d = "" + 10;                // "10" (just make sure this specific case works)

a = true + "foo";           // "truefoo"
b = false + "foo";          // "falsefoo"
c = "foo" + true;           // "footrue"
d = "foo" + false;          // "foofalse"
a = null + "foo";           // "nullfoo"
b = "foo" + null;           // "foonull"

a = NaN + "foo";            // "NaNfoo"
b = "foo" + NaN;            // "fooNaN"
c = Infinity + "foo";       // "Infinityfoo"
d = "foo" + Infinity;       // "fooInfinity"
a = -Infinity + "foo";      // "-Infinityfoo"
b = "foo" + -Infinity;      // "foo-Infinity"
c = "foo" + 0;              // "foo0"
c = "foo" + -0;             // "foo0" (-0 converted to string loses the sign)
c = "foo" + (-5/Infinity);  // "foo0" (-0 converted to string loses the sign)

//
// addition and subtraction
//

d = 5 + 20;                 // 25

a = 20 - 5;                 // 15
a = "20" - 5;               // 15
a = 20 - "5";               // 15
a = "20" - "0x5";           // 15

b = NaN + 5;                // NaN
b = 5 + NaN;                // NaN
b = NaN + NaN;              // NaN
b = NaN - 5;                // NaN
b = 5 - NaN;                // NaN
b = NaN - NaN;              // NaN
b = "Q" - 5;                // NaN
b = 20 - "Q";               // NaN
b = "Q" - "R";              // NaN
c = Infinity - 5;           // Infinity
c = Infinity + 5;           // Infinity
c = "Infinity" - 5;         // Infinity
d = -Infinity - 5;          // -Infinity
d = -Infinity + 5;          // -Infinity
d = "-Infinity" - 5;        // -Infinity
a = Infinity + NaN;         // NaN
a = -Infinity + NaN;        // NaN
a = Infinity + -Infinity;   // NaN
b = Infinity + Infinity;    // Infinity
c = -Infinity + -Infinity;  // -Infinity
d = -0 + -0;                // -0
a = +0 + +0;                // 0
a = 0 + -0;                 // 0
a = -0 + 0;                 // 0
a = 5 + -5;                 // 0
b = 0 + 5;                  // 5

//
// multiplication and division
//

a = 5 * 20;                 // 100
a = "5" * 20;               // 100
a = 5 * "20";               // 100
a = "0x5" * "20";           // 100

b = 20 / 5;                 // 4
b = "20" / 5;               // 4
b = 20 / "5";               // 4
b = "20" / "0x5";           // 4

// special multiplication
c = NaN * 5;                // NaN
c = 5 * NaN;                // NaN
c = NaN * NaN;              // NaN
c = NaN / NaN;              // NaN
c = "Q" * 5;                // NaN
c = Infinity * 0;           // NaN
c = "Infinity" * 0;         // NaN
c = -Infinity * 0;          // NaN
c = "-Infinity" * 0;        // NaN

d = Infinity * Infinity;    // Infinity
d = -Infinity * -Infinity;  // Infinity
d = Infinity * 5;           // Infinity
d = -Infinity * -5;         // Infinity
a = Infinity * -Infinity;   // -Infinity
a = -Infinity * Infinity;   // -Infinity
a = Infinity * -5;          // -Infinity
a = -Infinity * 5;          // -Infinity

// special division
b = NaN / 5;                // NaN
b = 5 / NaN;                // NaN
b = "Q" / 5;                // NaN
b = Infinity / Infinity;    // NaN
b = -Infinity / Infinity;   // NaN
b = Infinity / -Infinity;   // NaN
b = -Infinity / -Infinity;  // NaN
b = "Infinity"/"-Infinity"; // NaN
c = Infinity / 0;           // Infinity
c = -Infinity / -0;         // Infinity
c = Infinity / 5;           // Infinity
c = -Infinity / -5;         // Infinity
d = Infinity / -0;          // -Infinity
d = -Infinity / 0;          // -Infinity
d = Infinity / -5;          // -Infinity
d = -Infinity / 5;          // -Infinity
a = 5 / Infinity;           // 0
a = -5 / -Infinity;         // 0
a = 0 / 5;                  // 0
a = -0 / -5;                // 0
b = -5 / Infinity;          // -0
b = 5 / -Infinity;          // -0
b = -0 / 5;                 // -0
b = 0 / -5;                 // -0
c = 0 / 0;                  // NaN
c = 0 / -0;                 // NaN
d = 5 / 0;                  // 5/0   (Infinity is longer than the whole expression, so don't replace it)
d = -5 / -0;                // -5/-0 (Infinity is longer than the whole expression, so don't replace it)
a = -5 / 0;                 // -5/0  (-Infinity is longer than the whole expression, so don't replace it)
a = 5 / -0;                 // 5/-0  (-Infinity is longer than the whole expression, so don't replace it)

a = Infinity/(-5/Infinity); // -Infinity  (Infinity/-0)

//
// modulo
//
b = 20 % 5;                 // 0
b = "20" % 5;               // 0
b = 20 % "5";               // 0
b = "20" % "5";             // 0
b = 20 % -5;                // 0
c = -20 % 5;                // -0
c = -20 % -5;               // -0
d = 23 % 5;                 // 3
a = -23 % 5;                // -3
b = 46.5 % 6.25;            // 2.75
b = "46.5" % 6.25;          // 2.75
b = 46.5 % "6.25";          // 2.75
b = "46.5" % "6.25";        // 2.75
c = 46.8 % 6.1;             // 4.1
d = 46.5 % 6.3;             // 2.4000000000000012 is too long -- will stay 46.5%6.3

// special modulo
a = NaN % 5;                // NaN
a = 5 % NaN;                // NaN
a = NaN % NaN;              // NaN
a = 20 % "Q";               // NaN
a = Infinity % 5;           // NaN
a = -Infinity % 5;          // NaN
a = 20 % 0;                 // NaN
a = 20 % -0;                // NaN
a = Infinity % 0;           // NaN
b = 20 % Infinity;          // 20
b = 20 % -Infinity;         // 20
c = -20 % Infinity;         // -20
c = -20 % -Infinity;        // -20
d = 0 % Infinity;           // 0
d = 0 % -Infinity;          // 0
a = -0 % Infinity;          // -0
a = -0 % -Infinity;         // -0

//
// bitwise shift
//
b = 1 << 6;                 // 64
b = "1" << 6;               // 64
b = 1 << "6";               // 64
b = "1" << "6";             // 64
b = 4096 >> 6;              // 64
b = "4096" >> 6;            // 64
b = 4096 >> "6";            // 64
b = "4096" >> "6";          // 64
c = -1 >> 20;               // -1
d = -1 >>> 20;              // 4095 -- BUT -1 isn't in the unsigned int range, so we have a cross-browser issue; don't combine

// only lower 5-bits are used for the shift value!
a = 1 << 0x26;              // 64
b = 0x7fffffff >> 51;       // 4095
b = -1 >>> 52;              // 4095 -- BUT -1 isn't in the unsigned int range, so we have a cross-browser issue; don't combine

// special shifts
// NaN, +0, -0, +Infinity and -Infinity are all converted to zero
b = 4095 << 0;              // 4095
b = 4095 >> 0;              // 4095
b = 4095 >>> 0;             // 4095
b = 4095 << -0;             // 4095
b = 4095 >> -0;             // 4095
b = 4095 >>> -0;            // 4095
b = 4095 << NaN;            // 4095
b = 4095 >> NaN;            // 4095
b = 4095 >>> NaN;           // 4095
b = 4095 << "NaN";          // 4095
b = 4095 >> "NaN";          // 4095
b = 4095 >>> "NaN";         // 4095
b = 4095 << "Q";            // 4095
b = 4095 >> "Q";            // 4095
b = 4095 >>> "Q";           // 4095
b = 4095 << Infinity;       // 4095
b = 4095 >> Infinity;       // 4095
b = 4095 >>> Infinity;      // 4095
b = 4095 << "Infinity";     // 4095
b = 4095 >> "Infinity";     // 4095
b = 4095 >>> "Infinity";    // 4095
b = 4095 << -Infinity;      // 4095
b = 4095 >> -Infinity;      // 4095
b = 4095 >>> -Infinity;     // 4095
b = 4095 << "-Infinity";    // 4095
b = 4095 >> "-Infinity";    // 4095
b = 4095 >>> "-Infinity";   // 4095
c = 0 << 15;                // 0
c = NaN << 15;              // 0
c = Infinity << 15;         // 0
c = -Infinity << 15;        // 0
c = 0 >> 15;                // 0
c = NaN >> 15;              // 0
c = Infinity >> 15;         // 0
c = -Infinity >> 15;        // 0
c = 0 >>> 15;               // 0
c = NaN >>> 15;             // 0
c = Infinity >>> 15;        // 0
c = -Infinity >>> 15;       // 0

//
// bitwise operators
//
a = 0xaa & 0x0f;            // 10
a = "0xaa" & 0x0f;          // 10
a = 0xaa & "15";            // 10
a = "170" & "0xf";          // 10
b = 0xaa | 0x0f;            // 175
b = "0xaa" | 0x0f;          // 175
b = 0xaa | "0x0f";          // 175
b = "0xaa" | "0x0f";        // 175
c = 0xaa ^ 0x0f;            // 165
c = "0xaa" ^ 0x0f;          // 165
c = 0xaa ^ "0x0f";          // 165
c = "170" ^ "15";           // 165

// special conversion - 0, -1, NaN, Infinity and -Infinity all convert to zero
d = 0xaa & -0;              // 0
d = 0xaa & "Q";             // 0
d = 0xaa & NaN;             // 0
d = 0xaa & Infinity;        // 0
d = 0xaa & -Infinity;       // 0
d = 0 | -0;                 // 0
d = 0 | "Q";                // 0
d = 0 | NaN;                // 0
d = 0 | Infinity;           // 0
d = 0 | -Infinity;          // 0
d = 0 ^ -0;                 // 0
d = 0 ^ "Q";                // 0
d = 0 ^ NaN;                // 0
d = 0 ^ Infinity;           // 0
d = 0 ^ -Infinity;          // 0

//
// logical operators
//

// these only value by the left-hand value, depending on whether or not ToBoolean is true
// false: false, null, +0, -0, NaN, ""
// true: true, Infinity, +Infinity, finite non-zero number, non-empty string

// or: return left operand if true, otherwise right operand
a = false || 100;           // 100
a = null || 100;            // 100
a = 0 || 100;               // 100
a = -0 || 100;              // 100
a = NaN || 100;             // 100
a = "" || 100;              // 100

a = 100 || 42;              // 100
b = true || 100;            // true
c = Infinity || 100;        // Infinity
d = -Infinity || 100;       // -Infinity
a = "Q" || 100;             // "Q"

// and: return left operand if false, otherwise right operand
b = false && 100;           // false
c = null && 100;            // null
d = 0 && 100;               // 0
a = -0 && 100;              // -0
b = NaN && 100;             // NaN
c = "" && 100;              // ""

d = 42 && 100;              // 100
d = true && 100;            // 100
d = Infinity && 100;        // 100
d = -Infinity && 100;       // 100
d = "Q" && 100;             // 100

// octal literals should not be changed because of possible cross-browser issues.
// so these operations should NOT be combined
var o1 = 0377 - 55; // NOT changed to 200
var o2 = 0xff - 012; // NOT changed to 245
var o3 = 020 * 04; // NOT changed to 64

// these comma operators have a constant left-hand side; we'll replace the comma with the right-hand side.
// BUT... the right-hand side is also a constant. Check to see if the new constant can change the member-bracket
// operator into a member-dot
var m1 = o1[123, "foo"];    // yes
var m2 = o2[true, 456];     // no
var m3 = o3["ack", "1bar"]; // no

// conditional operators
var ct1 = "foo" ? a : b;
var ct2 = 12345 ? a : b;
var ct3 = true ? a : b;
var ct4 = 12 + "ack" ? a : b;
var cf1 = "" ? a : b;
var cf2 = 0 ? a : b;
var cf3 = false ? a : b;

// typeof's for constants can be known
var t1 = typeof "my string";    // string
var t2 = typeof 1234;           // number
var t3 = typeof true;           // boolean
var t4 = typeof null;           // object
var t5 = typeof {};             // object

// bitwise not
var bn1 = ~1;                   // -2
var bn2 = ~0xff;                // -256
var bn3 = ~-10;                 // 9

// unary plus converts to number
var up1 = +"-1";                // -1
var up2 = +"foobar";            // NaN -- so don't evaluate
var up3 = +true;                // 1
var up4 = +false;               // 0
var up5 = +null;                // 0
var up6 = +"1234";              // 1234
var up7 = +"0xff";              // 255
