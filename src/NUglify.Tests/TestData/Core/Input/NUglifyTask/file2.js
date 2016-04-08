function foo(a)
{
    if (a === null)
    {
        return;
    }

    var b = a*a;
    var d = 1;
    for(var c = 0; c < b; ++c)
    {
        d += c;
    }
    return d;
}


function bar(a, b)
{
    if (a === null)
    {
        return;
    }
    if (b === null)
    {
        return;
    }

    var d = 1;
    for(var c = 0; c < b; ++c)
    {
        d += c;
    }
    return d;
}

function ack(a, b, c)
{
    if (a === null)
    {
        return "null" + c;
    }
    if (b === null)
    {
        return "null" + c;
    }

    var foo = a + b;
    if (c != null)
    {
        foo += c;
    }

    return foo;
}

function ret(a, b)
{
    if (!a)
    {
        return;
    }

    a(b);
}
