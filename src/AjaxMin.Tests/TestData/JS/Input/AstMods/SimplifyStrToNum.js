// common technique for making sure a variable value is 
// a number before we do math on it is to subtract 0 from it.

var a = "1234";

// because a is a string, this ends up being a string concatenation operation.
// the result is "12345"
var b = a + 5;

// subtracting zero converts to number, and then the + is a numeric addition
// the result is the number 1239. 
var c = (a-0) + 5;

// same thing here -- the result is 1239
var d = 5 + (a-0);

// whereas this results in 51234
var e = 5 + a;

// and if we are evaluating literal expressions, this should nest.
// (5 - 5) should evaluate to 0, which should then be recognized
// as our "lookup - 0" pattern and get replaced.
var f = 5 + (a - (5-5));