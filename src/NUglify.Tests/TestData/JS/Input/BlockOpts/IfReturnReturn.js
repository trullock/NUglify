function test1(a)
{
    if (a != 0)
    {
        // doesn't apply to a non-function-level block
        if (a == 20) return;
        return;
    }

    // either way, we return undefined, so just replace both statements with the condition.
    if (a === "foo") return;
    return;
}

function test2(a,b)
{
    if ( foo(a,b) ) return a + b;
    return a + b;

    function foo(a,b)
    {
        return typeof a === typeof b;
    }
}

