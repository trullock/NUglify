function test1()
{
    "use strict"

    // can't have duplicate arg in strict mode
    function foo(one, two, three, one, four)
    {
        return one + two + three + four;
    }

    return foo();
}

function test2()
{
    // no error, just a perf warning
    function bar(one, two, one, three)
    {
        return one + two + three;
    }

    return bar();
}
