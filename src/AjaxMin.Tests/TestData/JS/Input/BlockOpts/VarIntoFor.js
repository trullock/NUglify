function test1(a,b)
{
    // c and d are both in the previous var statement, so the var statement
    // can be moved into the for statement
    var c;
    var d;
    var e;
    for(c=1, d=10;c < d; ++c)
    {
        b += c * d;
    }
}

function test2(a,b)
{
    // a is not in the previous var statement, which means the
    // previous var cannot be moved into the for statement
    var c;
    var d;
    var e;
    for(c=1, d=10, a=2;c < d; ++c)
    {
        b += c * d;
    }
}

function test3(a,b)
{
    // var can be moved into the for, both assignments must be maintained
    var c = a;
    var d;
    var e;
    for(c=1, d=10;c < d; ++c)
    {
        b += c * d;
    }
}

function test4(a,b)
{
    // the initializer is not an assignment or series of assignments -- cannot move the
    // var-statement into the initializer
    var c;
    for(foo();c < 10; ++c)
    {
        b += a;
    }

    function foo()
    {
        c = a + b;
    }
}

function test5(a,b)
{
    // the initializer is not an assignment or series of assignments -- cannot move the
    // var-statement into the initializer
    var c;
    for(foo(), b="";c < 10; ++c)
    {
        b += a;
    }

    function foo()
    {
        c = a + b;
    }
}

function test6(a, b)
{
    var c = a / b;
    for(;c < a;++c)
    {
        b += c;
    }
}
