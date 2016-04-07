// the if-condition is a constant for true -- replace with 1
if ("foobar")
{
    throw "true";
}

// replace with 0
if (null)
{
    throw "false";
}


//@cc_on
//@if( 1234 )
alert("true");
//@elif ( false )
alert("elif");
//@else
alert("false");
//@end
