function test1(a,b)
{
    if (a < b)
    {
        // non-function-level block does NOT end in an implicit return,
        // so we can break that out.
        return a == 0 ? void (0) : void 123;
    }

    // both branches return void, and the function-level block
    // ends in an implicit return, so get rid of the return.
    return a==b ? void(0) : void(123);
}

function test2(a,b)
{
    if ( a > b )
    {
        // because we aren't a function-level block, we can't break the return
        // statement out and still save bytes
        return a == 0 ? void 999 : a - b;
    }

    // put a statement in between the if and the return just to make sure they
    // don't get combined
    var c = b;

    // we are at the function level, so break it out
    return c instanceof String ? void 10000 : a;
}

