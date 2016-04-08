// this is the only @cc_on that should stick around (it's the first)
//@cc_on

// so we've already had a @cc_on statment -- all others are superfluous
var a = 0;

/*@cc_on@*/
//@if(@_win32)
a = 1;
//@end

var b/*@cc_on=1@*/

// get rid of this one, too
//@cc_on

if(a)
    alert("win32");
