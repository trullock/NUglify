"use strict";

function test1()
{
    // can't assign to global eval object
    eval = function(txt)
    {
        alert("eval: " + txt);
    };

    // can't assign to arguments object
    arguments = {};
}

function test2()
{
    // can't be for the pre- or post-fix increment/decrement operators, either
    ++eval;
    eval--;

    --arguments;
    arguments++;
}
