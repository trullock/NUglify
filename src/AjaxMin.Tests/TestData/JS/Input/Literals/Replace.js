function test1(str)
{
    // replace the variable with the literal
    var foo = /onetwothree/g;
    return foo.exec(str);
}

function test2(str)
{
    var match;

    // do NOT replace the variable with the literal because it's
    // maintaining state and needs to be re-evaluated in the condition
    // each time.
    var re = /the/g;
    try
    {
        while((match = re.exec(str)))
        {
            alert(match[0]);
        }
    }
    catch(e)
    {
        // just wrapping in a try so the while doesn't get converted to 
        // a for by moving the var inside the initializer.
    }
}

function test3()
{
    // replace the variable with the literal
    var arr = ["one","two","three","four"];
    return arr.foo();
}

function test4(ndx)
{
    // do NOT replace the variable with the literal because
    // it's in the condition and needs to be re-evaluated each loop
    var ndx = 0;
    var arr = ["one","two","three","four"];
    do
    {
        alert(++ndx);
    } while(arr.shift());

    return ndx;
}

function test5(ndx)
{
    // do NOT replace the variable with the literal because
    // it's in the condition and needs to be re-evaluated each loop
    var ndx = 0;
    var arr = ["one","two","three","four"];
    for(var first; (first = arr.shift()); ++ndx)
    {
        alert(first);
    }

    return ndx;
}

function test6()
{
    // do NOT replace the variable with the literal because
    // it's in the iterator and needs to be re-evaluated each loop
    var arr = ["one","two","three","four"];
    for(var first = "zero"; first; (first = arr.shift()))
    {
        alert(first);
    }
}

function test7(str)
{
    // do NOT replace the variable because of the iteration
    var re = /the/g;
    while(true)
    {
        var match = re.exec(str);
        if (!match) break;
        alert(match[0]);
    }
}