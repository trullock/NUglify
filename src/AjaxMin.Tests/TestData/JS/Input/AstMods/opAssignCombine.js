function test1(arg)
{
    foo = arg
    foo += foo.indexOf("?");
}

function test2(arg)
{
    var foo = arg;
    foo += (foo.indexOf("#") >= 0 ? "" : "#") + "fix";
}

function test3(arg)
{
    var foo = arg;
    foo += "bar";
    foo += foo.indexOf("?");
    foo += "bat";
}