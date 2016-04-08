function foo(bar)
{
    if (bar)
    {
        // make sure the member access via string reference works
        // (must only be a single string constant within the brackets)
        alert(Strings[ "Message" ] + bar);
    }
    else
    {
        // the direct property access should work as well
        alert(Strings.Message + "[null]" + "!" + "!" + b);
    }
}
foo("bar!");

