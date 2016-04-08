// this file starts with an opening parenthesis,
// so when concatenated to a file that matches the pattern (function())() and DOESN'T
// end in a semicolon, it will get parsed as a call to the result of the previous file's
// expression, not as the start of a new expression statement, which is incorrect.
// NUglify will need to separate the two files with a semicolon to keep it right.
(function($)
{
    $(function(){alert("Hi, too!");});
})(jQuery)