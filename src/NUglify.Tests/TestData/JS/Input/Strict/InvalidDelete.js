"use strict";

function test1(a, b)
{
    function foo(x)
    {
        return x*x;
    }

    var c = a + b;

    // can't delete an argument
    delete a;

    // can't delete a variable
    delete c;

    // can't delete a function
    delete foo;
}
