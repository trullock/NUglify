var a, b, c;

var test = 6 * (x/2);

// this should work around the string literal
var d = 5 + (26 - "+0x6") + 0x0f; // 46-"+0x6"

//
// addition and subtraction
//

// far to the right eval
a = 5 + (a + 20);           // 5+(a+20) DON'T EVAL -- don't know if the a+5 will be a string cat!
b = 5 + (a - 20);           // a-15
c = 5 - (a + 20);           // 5-(a+20) DON'T EVAL -- don't know if the a+5 will be a string cat!
d = 5 - (a - 20);           // 25-a (swap operands)

//
// multiplication and division
//

// far to the right eval
a = 5 * (a * 20);           // a*100
a = "5" * (a * 20);         // a*100
a = 5 * (a * "20");         // a*100
a = "5" * (a * "20");       // a*100
b = 5 * (a / 20);           // a/4
b = "5" * (a / 20);         // a/4
b = 5 * (a / "20");         // a/4
b = "5" * (a / "20");       // a/4
c = 5 / (a * 20);           // .25/a (swap operands)
c = "5" / (a * 20);         // .25/a (swap operands)
c = 5 / (a * "20");         // .25/a (swap operands)
c = "5" / (a * "20");       // .25/a (swap operands)
d = 5 / (a / 20);           // 100/a (swap operands)
d = "5" / (a / 20);         // 100/a (swap operands)
d = 5 / (a / "20");         // 100/a (swap operands)
d = "5" / (a / "20");       // 100/a (swap operands)

e = 20 / (a * 5);           // 4/a (swap operands)
e = "20" / (a * 5);         // 4/a (swap operands)
e = 20 / (a * "5");         // 4/a (swap operands)
e = "20" / (a * "5");       // 4/a (swap operands)
f = 20 * (a / 5);           // 4*a (swap operands)
f = "20" * (a / 5);         // 4*a (swap operands)
f = 20 * (a / "5");         // 4*a (swap operands)
f = "20" * (a / "5");       // 4*a (swap operands)
g = 20 / (a / 5);           // 100/a (swap operands)
g = "20" / (a / 5);         // 100/a (swap operands)
g = 20 / (a / "5");         // 100/a (swap operands)
g = "20" / (a / "5");       // 100/a (swap operands)
