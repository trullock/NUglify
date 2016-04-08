var a, b = {};

// be sure to use a keyword as the property name in the 
// member-bracket expressions, or it will get
// converted to memeber-dot notations :)

// operator need spaces on both sides
a instanceof Array;

// operator doesn't need space to the left
b["if"] instanceof Array;

// oprtator doesn't need space to the right
a instanceof (b || Array);

// operator needs no spaces
b["if"] instanceof (b || Array);
