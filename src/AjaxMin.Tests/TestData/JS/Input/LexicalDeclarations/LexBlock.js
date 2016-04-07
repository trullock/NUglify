for(var ndx = 1; ndx <= 10; ++ndx)
{
    console.log(ndx);

    // this nested block should not get un-nested because of the lexical declarations within it
    {
        let square = ndx * ndx;
        if (ndx == 5)
        {
            square += ndx;
        }

        console.log(square);
    }
}