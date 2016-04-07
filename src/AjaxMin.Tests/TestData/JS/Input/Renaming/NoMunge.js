function test1(arg1, arg2)
{
    // a prologue directive that's really a minification hint
    "arg2:nomunge";

    return arg1 + arg2;
}

function test2(arg1, arg2)
{
    console.log("arg1=" + arg1 + "; arg2=" + arg2);

    // a minification hint that isn't a directive prologue
    "arg2:nomunge";
    return arg1 + arg2;
}

function test3(arg1, arg2)
{
    // multiple hints, including a field that doesn't exist
    "arg1:nomunge, arg2:nomunge, arg3:nomunge";
    return arg1 + arg2;
}

function test4(arg1, arg2)
{
    var outerField = arg1 + arg2;
    function foo(one, two)
    {
        // hint that tries to rename outer field doesn't, but should
        // still rename current field
        "outerField:nomunge, one:nomunge";
        return outerField + one + two;
    }

    return foo(arg1, arg2);
}

function test5(arg1, arg2)
{
    var v1 = arg1 + arg2;
    // missing IDENT means don't rename ANY variables defined in this scope
    ":nomunge";
    console.log("v1:" + v1);
    return v1;
}

function test6(arg1, arg2)
{
    var v1 = arg1 + arg2;
    // asterisk IDENT means don't rename ANY variables defined in this scope
    "*:nomunge";
    console.log("v1:" + v1);
    return v1;
}
