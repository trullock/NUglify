function foo(ndx)
{
    // the assignment should be moved into the vardecl,
    // reducing the refcount to 1. But it's not a constant,
    // so it shouldn't get eliminated.
    var a;
    a = 10 + ndx;

    // just a statement to separate the return from the assignment
    switch(ndx)
    {
        case 1:
            ndx += 42;
        default:
            ndx *= 2;
            break;
    }

    return ndx + a;
}