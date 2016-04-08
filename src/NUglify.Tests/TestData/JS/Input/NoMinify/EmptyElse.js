// this file should not get any blocks removed
function test1( foo )
{
    // no blocks; just empty statements
    if (foo)
        ;
    else 
        ;

    // blocks containing nothing
    if (foo)
    {
    }
    else
    {
    }

    // blocks containing empty statement
    if (foo)
    {
        ;
    }
    else
    {
        ;
    }

    // blocks containing nested empty blocks
    if (foo)
    {
        {}
    }
    else
    {
        {}
    }
}
