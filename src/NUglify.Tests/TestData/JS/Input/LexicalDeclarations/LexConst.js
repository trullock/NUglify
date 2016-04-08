function foo(max)
{
    for(var ndx = 0; ndx < max; ++ndx)
    {
        // the const is only scoped to the block
        const squared = ndx * ndx;
        alert(squared);
    }

    // this should be an undefined global!
    alert(squared);
}