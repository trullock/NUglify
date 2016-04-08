var a, b;

// 1/3 won't get replaced with .33333333333333333, but the 6 will multiply with the 1 and 
// rotate the division up, and then the 6 divided by the 3 *will* get replaced
var c = 6*(1/3);    // 2

// this should work around the string literal
var d = 5 + (26 - "+0x6") + 0x0f; // 46-"+0x6"

//
// string concatenation
//

// to the right eval
a = 10 + ("foo" + a);       // "10foo"+a
b = "foo" + ("bar" + a);    // "foobar"+a

//
// addition and subtraction
//

// to the right eval
c = 5 + (20 + a);           // 5+(20+a) DON'T EVAL -- don't know if the 20+a will be a string cat!
d = 5 - (20 + a);           // 5-(20+a) DON'T EVAL -- don't know if the 20+a will be a string cat!
a = 5 + (20 - a);           // 25-a
b = 5 - (20 - a);           // a-15 (swap the operands to keep it a minus; can't be -15+a)

//
// multiplication and division
//

// to the right eval
c = 5 * (20 * a);           // 100*a
c = "5" * (20 * a);         // 100*a
c = 5 * ("20" * a);         // 100*a
c = "5" * ("20" * a);       // 100*a
d = 5 / (20 * a);           // .25/a
d = "5" / (20 * a);         // .25/a
d = 5 / ("20" * a);         // .25/a
d = "5" / ("20" * a);       // .25/a
a = 5 * (20 / a);           // 100/a
a = "5" * (20 / a);         // 100/a
a = 5 * ("20" / a);         // 100/a
a = "5" * ("20" / a);       // 100/a
b = 5 / (20 / a);           // a/4 (swap the operands)
b = "5" / (20 / a);         // a/4 (swap the operands)
b = 5 / ("20" / a);         // a/4 (swap the operands)
b = "5" / ("20" / a);       // a/4 (swap the operands)


c = 20 / (5 * a);           // 4/a
d = 20 * (5 / a);           // 100/a
a = 20 / (5 / a);           // 4*a