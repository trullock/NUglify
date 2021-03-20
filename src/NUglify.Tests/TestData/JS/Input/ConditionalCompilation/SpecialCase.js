// should keep the conditional comment
var ie/*@cc_on=1@*/;

// should keep the conditional comment and the conditional variable and don't
// add a cc_on to the comment (we already encountered one and therefore don't need it)
var ver //@ =@_jscript_version
	;
// keep the conditional comment, don't add the cc_on, and don't throw an error
// because there is no space between the @ and the =
var ack //@=2
	;
// combination of variables and preprocess values
// (and don't add a cc_on)
var foo //@cc_on = (ver + !@bar) * 12
	;
// this fits another special-case pattern: just the ! inside a conditional comment
var isMSIE = /*@!@*/0;

// another example of a ! inside a conditional-comment, but this one is in a function
// that gets relocated to the top of the file, so it will need to keep its cc_on
// but the others should be eliminated if we are removing duplicate cc_ons.
function isIe()
{
    if (/*@cc_on!@*/false)
    {
        return true;
    }
}

// this does not fit any special-case patter, so disregard the comment
var bar = /*@!true+@*/1;

// this is just so we know that last one doesn't kill the entire processing and can recover okay
alert(ver);