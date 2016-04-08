function test1(a, b)
{
    var c = a(b);
    window.console && console.log("ready to return");
    return c;
}

function test2(a)
{
    if (window.console)
    {
        console.log("a=" + a);
    }
    return a;
}