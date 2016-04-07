// this is a good test: the code needs to be smart enough to know that the first token
// after the assign is a regular expression, NOT a "/=" (divide-assign)
var pattern = /=|\^=|\$=|\*=/;

// the literal should not be surrounded by parens just because of the member (.) operator!
/^("(\\.|[^"\\\n\r])*?"|[,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t])+?$/.test('{"foo":42}');

var str = "Now is the time for all good men";
var rep = str.replace(/\s+/g,'+');

var re = /foo\s+bar/m;
re = /book/i;

var trim = /^\s*(.*)\s*$/m.match(str);


// don't use standard string escapes for string literals passed to RegExp construtor
// REVISION: no, it's perfectly fine to use regular string escapes in regular expression constructors
var r1 = new RegExp("\x07\x08\x09\x0a\x0b\x0c\x0d\x0e"); 
var r2 = new RegExp("\x07\x08\x09\x0a" + str + "\x0b\x0c\x0d\x0e"); // even as part of an expression


