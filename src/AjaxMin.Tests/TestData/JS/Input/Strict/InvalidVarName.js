"use strict"

function test1(obj)
{
    // eval and arguments not allowed to be a var name
    var eval;
    for(var arguments in obj)
    {
        eval += obj[arguments];
    }
    return eval;
}

// eval and arguments are not allowed to be argument names
function test2(eval, arguments)
{
    return eval + arguments;
}

function test3(a, b)
{
    // eval is not allowed to be a function name
    function eval(txt)
    {
        alert(txt);
    }

    // arguments is not allowed to be a function name
    function arguments()
    {
        return {};
    }

    return eval(a) + arguments()[b];
}

function test4(a)
{
    try
    {
        return foo[a];
    }
    catch(eval) // eval not allowed to be catch parameter
    {
        return "";
    }
}

function test5(a)
{
    try
    {
        return foo[a];
    }
    catch(arguments) // arguments not allowed to be catch parameter
    {
        return "";
    }
}

