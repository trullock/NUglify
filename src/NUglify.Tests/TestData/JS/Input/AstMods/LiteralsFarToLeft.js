var a, b, c, d;

//
// addition and subtraction
//

// far to the left eval
a = (5 + a) + 20;           // 5+a+20 DON'T EVAL -- don't know if the 5+a will be a string cat!
b = (5 - a) + 20;           // 25-a
c = (5 + a) - 20;           // 5+a-20 DON'T EVAL -- don't know if the 5+a will be a string cat!
d = (5 - a) - 20;           // -15-a

//
// multiplication and division
//

// far to the left eval
a = (5 * a) * 20;           // 100*a
a = ("5" * a) * 20;         // 100*a
a = (5 * a) * "20";         // 100*a
a = ("5" * a) * "20";       // 100*a
b = (5 / a) * 20;           // 100/a
b = ("5" / a) * 20;         // 100/a
b = (5 / a) * "20";         // 100/a
b = ("5" / a) * "20";       // 100/a
c = (5 * a) / 20;           // a/4
c = ("5" * a) / 20;         // a/4
c = (5 * a) / "20";         // a/4
c = ("5" * a) / "20";       // a/4
d = (5 / a) / 20;           // .25/a
d = ("5" / a) / 20;         // .25/a
d = (5 / a) / "20";         // .25/a
d = ("5" / a) / "20";       // .25/a

a = (20 / a) / 5;           // 4/a
b = (20 * a) / 5;           // 4*a
c = (20 / a) * 5;           // 100/a

