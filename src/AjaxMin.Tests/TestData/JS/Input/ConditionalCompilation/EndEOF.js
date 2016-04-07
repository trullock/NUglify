//@cc_on
//@if (@IsDebug)
(function foo(bar)
{
    alert(bar);
})("ack");

// don't have a line-feed after the @end -- just end the file
//@end