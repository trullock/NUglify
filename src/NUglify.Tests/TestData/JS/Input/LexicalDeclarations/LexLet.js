function foo(max)
{
    for(var ndx = 0; ndx < max; ++ndx)
    {
        // the let is only scoped to the block
        let squared = ndx * ndx;
        alert(squared);
    }

    // this should be an undefined global!
    alert(squared);
}