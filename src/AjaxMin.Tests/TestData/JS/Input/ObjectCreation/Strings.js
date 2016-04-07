

var str1 = "The quick brown fox jumped over the lazy dog."; // normal crunch
var str2 = "How now \"brown\" cow?"; // delimiters should change to single-quotes
var str3 = 'Who\'s the boss?'; // delimiters should change to double-quotes
var str5 = "\x01\x05\x12\x13"; // ascii less than 32 (escape to octal \ooo) - REVISION: don't use octal; always use hex

// escape sequences. same number double and single quotes -- use double quotes as delims
// because doubles are used as delims, escape for single-quote should be stripped
var str4 = "\t\n\r\f\b\\\'\""; 

// embedded Unicode string. Output depends on output encoding.
// default is ascii, so characters should each be \uNNNN encoded.
// UTF-16 should be the same as input (unescaped raw Unicode chars)
var str6 = "你好！"; 
