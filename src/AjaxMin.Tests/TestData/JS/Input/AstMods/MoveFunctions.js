(function(log)
{
    // this is where the code starts
    log = Math.LN2(unknownGlobal);

    // the var will be after the function, but the assignment will stay put
    var outerVar = 42;

    // this will get moved to the top of the function
    function outer()
    {
        var x = 0;
        for(var ndx = 0; ndx < 100; ++ndx)
        {
            if (ndx == 50)
            {
                ndx += ++x;

                // don't move this function -- it's inside an if-statement
                // and that means cross-browser differences. So leave it alone
                function foo(x)
                {
                    return x+x;
                }

                alert(foo(ndx));
            }
        }
    }

    // some more code
    switch(log)
    {
        case 0: return arf(0);
        default: return outer(log);
    }

    var another = 10;

    // another function
    function arf(d)
    {
        return "" + d;
    }
})();