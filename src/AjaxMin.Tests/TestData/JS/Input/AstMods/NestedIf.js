function foo(a,b)
{
    if (a !== null)
    {
        if (typeof b === "string")
        {
            return a + b;
        }
    }

    return "";
}