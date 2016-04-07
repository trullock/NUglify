// this if statement has no false block, and the
// true block is just a single call node. Replace it with
// an and expression
if (p1.foo)
{
    p1.foo("bar");
}

// BUT don't do it if we're calling the onclick method of a function
// and passing a parameter
if (p1.foo)
{
    p1.foo.onclick("bar");
}

// it's okay if we're not passing any arguments
if (p1.foo)
{
    p1.foo.onclick();
}

function Func(p1,a)
{
    if(p1 || (a ? 1 : 0))
    {
        alert("true");
    }
    else
    {
        p1 = p1 > 1 ? 2 : 3;
        alert("false");
    }
    
    p1 = p1 > 3 ? 2 : 1;
}

// empty true block with an else block, should logical-not the
// condition and swap the false and true branches
if (p1 == 42) // should get changed to != 42
{
}
else
{
    p1 = 0;
    alert("foo");
}

// try a few more conditions to test whether logical-not or || operator is used
if (p1 == 0); else p1 = 0; // ||
if (p1 != 0); else p1 = 0; // ||
if (p1 === 0); else p1 = 0; // ||
if (p1 !== 0); else p1 = 0; // ||
if (p1 < 0); else p1 = 0; // ||
if (p1 <= 0); else p1 = 0; // ||
if (p1 > 0); else p1 = 0; // ||
if (p1 >= 0); else p1 = 0; // ||
if (p1 % 2); else p1--; // ||
if (!p1); else p1 = 0; // p1&&(p1=0)
if (--p1); else p1 = 1; // ||
if (!(p1 == 0)); else p1 = 0; // p1==0&&(p1=0)

// empty true-block at the end of the file
if ( p1 ) {}
