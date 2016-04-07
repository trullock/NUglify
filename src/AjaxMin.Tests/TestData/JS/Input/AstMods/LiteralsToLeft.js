var a, b, c, d;

//
// string concatenation
//

// to the left eval
a = (a + "foo") + "bar";    // a+"foobar"
b = (a + "foo") + 10;       // a+"foo10"

//
// addition and subtraction
//

// to the left eval
c = (a + 5) + 20;           // a+5+20 DON'T EVAL -- don't know if the a+5 will be a string cat!
d = (a + 5) - 20;           // a+5-20 DON'T EVAL -- don't know if the a+5 will be a string cat!
a = (a - 5) + 20;           // a- -15 (doesn't get us anything, but could cascade)
b = (a - 5) - 20;           // a-25

//
// multiplication and division
//

// to the left eval
c = (a * 5) * 20;           // a*100
c = (a * "5") * 20;         // a*100
c = (a * 5) * "20";         // a*100
c = (a * "5") * "20";       // a*100
d = (a * 5) / 20;           // a/4 (a*.25 gets inverted to a/4)
d = (a * "5") / 20;         // a/4 (a*.25 gets inverted to a/4)
d = (a * 5) / "20";         // a/4 (a*.25 gets inverted to a/4)
d = (a * "5") / "20";       // a/4 (a*.25 gets inverted to a/4)
a = (a / 5) * 20;           // a*4
a = (a / "5") * 20;         // a*4
a = (a / 5) * "20";         // a*4
a = (a / "5") * "20";       // a*4
b = (a / 5) / 20;           // a/100
b = (a / "5") / 20;         // a/100
b = (a / 5) / "20";         // a/100
b = (a / "5") / "20";       // a/100
c = (a * 20) / 5;           // a*4
d = (a / 20) * 5;           // a/4
a = (a / 20) / 5;           // a/100

b = a | 0xf | 0x10;         // a|31
c = a & 0xf & 0x3;          // a&3
d = a ^ 0xf ^ 0x3;          // a^12