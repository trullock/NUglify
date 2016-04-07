// the literal should not be surrounded by parens because of the call (.) operator!
/^("(\\.|[^"\\\n\r])*?"|[,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t])+?$/.test('{"foo":42}');

var f = true.toString(); // no paren around boolean literal
var obj = {}.hasOwnProperty(); // no paren around object literal
var sort = [5,2,4,3,1].sort(); // no paren around array literal
var exp1 = (123456).toExponential(); // need paren around numerals with no decimal points
var exp2 = 123456.0.toExponential(); // need paren around numeral because .0 will be stripped
var exp3 = 123456.5.toExponential(); // no paren; numeral has decimal point
var match = /bar/g.exec("foo bar"); // no paren around regex
var rep = "Now is the time for all".replace(/\s/g,"+"); // no paren around string

// the function called will be a if it exists, or b if a doesn't exist
// needs to keep the parens
var ret = (a || b).foo();

// this is a keyword that CAN'T be an identifier
// should throw an error but still output as-is
f = ret.while(exp1);

// this is a keyword that can be an identifier
// don't throw an error or a warning becasue this is okay
// (and quite common)
f = ret.get(exp2);
