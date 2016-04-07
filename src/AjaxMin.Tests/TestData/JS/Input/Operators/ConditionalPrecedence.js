function test1(a, b, c)
{
    // conditional operator NEEDING parentheses
    return (a ? b : c) ? "A" : "B";
}

function test2(a, b)
{
    // conditional operator NOT needing parentheses
    return a ? "A" : (b ? "B" : "C");
}

function test3(a, b)
{
    // make sure we don't add any parens
    return a ? "A" : b ? "B" : "C";
}
