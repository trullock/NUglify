function foo(a, b)
{
    // this if will get converted to an expression
    if (a(b))
    {
        b = "foo";
    }

    // and then should be combined into the following if condition
    if (b !== null)
    {
        // just add a few statements to ensure that this if-statement
        // doesn't get converted into an expression
        var bar = "";
        for(var p in b)
        {
            bar += p + b[p];
        }
        return bar;
    }
}
