function test1(arg)
{
    var temp = arg;
    return temp += "";
}

function test2(arg)
{
    var temp = arg;
    return temp += "foo", temp += "bar";
}

function test3(arg)
{
    var temp = arg;
    return temp += "foo", temp += n, temp += "bar";
}