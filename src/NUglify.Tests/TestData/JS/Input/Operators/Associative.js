var a, b, c, x;

// comma
// all parens should be removed
a, b, c;
(a, b), c;
a, (b, c);

// now these get complicated by the assignment operator!
x = a, b, c; // x is assigned a, the statement evaluates to c (no paren)
x = (a, b), c; // x is assigned b, the statement evaluates to c (KEEP PAREN!)
x = (a, b, c); // x is assigned c, the statement evaluates to c (KEEP PAREN!)
x = a, (b, c); // x is assigned a, the statement evaluates to c (no paren)

// same, associative operators
// BUT parenthesis should NOT be removed from the last one!
// if a == "foo" and b == 1 and c == 2, then:
//  a+b+c == (a+b)+c == "foo12" BUT a+(b+c) == "foo3"
// SO, unless we know whether the two operators are both addition or both string concat,
// we cannot take out the parentheses -- a mix of the two operators makes for different results!
x = a + b + c;
x = (a + b) + c;
x = a + (b + c);

// same, associative operators
// all parenthesis should be removed
x = a * b * c;
x = (a * b) * c;
x = a * (b * c);

x = a && b && c;
x = (a && b) && c;
x = a && (b && c);

x = a || b || c;
x = (a || b) || c;
x = a || (b || c);

x = a & b & c;
x = (a & b) & c;
x = a & (b & c);

x = a | b | c;
x = (a | b) | c;
x = a | (b | c);

x = a ^ b ^ c;
x = (a ^ b) ^ c;
x = a ^ (b ^ c);

// same NON-associative operators
// the first two for each operator don't need parens, the last one DOES need to keep them
x = a - b - c;
x = (a - b) - c;
x = a - (b - c);

x = a / b / c;
x = (a / b) / c;
x = a / (b / c);

x = a % b % c;
x = (a % b) % c;
x = a % (b % c);

// equality
x = a == b == c;
x = (a == b) == c;
x = a == (b == c);

// relational
x = a < b < c;
x = (a < b) < c;
x = a < (b < c);

// shift
x = a << b << c;
x = (a << b) << c;
x = a << (b << c);

// different operators, different precedence, left higher than right
// first two remove parens, last one needs to keep them
x = a * b + c;
x = (a * b) + c;
x = a * (b + c);

// different operators, different precedence, left lower than right
// first and last remove parens, middle one needs to keep them
x = a + b * c;
x = (a + b) * c;
x = a + (b * c);

// different operators, same precedence
// first two remove the parens, last one needs to keep them
x = a / b * c;
x = (a / b) * c;
x = a / (b * c);



