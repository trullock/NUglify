
function foo(a,b,c,d,e)
{
    // the throw statement can be passed a string, a number, a boolean or an object
    if (a)
    {
        throw "now is the time"; // don't need a space between the statement and the string
    }

    if (b)
    {
        throw 42; // gotta have a space for the rest of them....
    }

    if (c)
    {
        throw true;

        // this will get removed
        return a+b+c;
    }

    if (d)
    {
        throw false;
    }

    if (e)
    {
        throw new foobar();
    }
}
