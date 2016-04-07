
function noTrueReturnOpNoElse(a)
{
    // use void 0 for the true part of the condition.
    // would be nice if we could just negate the if condition and move the final return to the true clause.
    if (!a)
    {
        return;
    }
    return a.toString();
}

function noTrueReturnOpWithElse(a)
{
    // use void 0 for the true part of the condition.
    // would be nice if we could just negate the if condition and move the else return to the true clause.
    if (!a)
    {
        return;
    }
    else
    {
        return a.toString();
    }
}

function no1(a)
{
    // DON'T collapse this one -- it works out to be 4 more bytes
    if(a == 1)
    {
        return true;
    }
}

function yes1(a)
{
    if (a == 1)
    {
        return true;
    }
    return false;
}

function yes2(a)
{
    if (a == 1)
    {
        return true;
    }
    else return false;
}

function yes3(a)
{
    // wrap up all four
    if (a == 1)
    {
        return "one";
    }
    else if (a == 2)
    {
        return "two";
    }
    if (a == 3)
    {
        return "three";
    }
    return "more";
}

function yes4(a)
{
    // same
    if (a == 1)
    {
        return "one";
    }
    else if (a == 2)
    {
        return "two";
    }
    else if (a == 3)
    {
        return "three";
    }
    return "more";
}

function yes5(a)
{
    // same
    if (a == 1) return "one";
    if (a == 2) return "two";
    if (a == 3) return "three";
    return "more";
}

// this one is more fun. The penultimate if has no exit, so the ultimate if
// should get totally removed
function yes6(a)
{
    // same
    if (a == 1)
    {
        return "one";
    }
    else
    {
        return "not one";
    }
    // here on down should get axed....
    if (a == 3)
    {
        return "three";
    }
    return "more";
}

function yes7(a)
{
    if (a == 1) return "one";
    if (a == 2) return "two";
    if (a == 3) return "three";
}

// not at the function level
function no2(a)
{
    while(a)
    {
        if (a == 1) return "one";
        if (a == 2) return "two";
        if (a == 3) return "three";
    }
}
// not at the function level
function sorta(a)
{
    while(a)
    {
        if (++a > 0)
        {
            if (a == 1) return "one";
            if (a == 2) return "two";
            if (a == 3) return "three";
            else return "more";
        }
    }
}

function removeIf(test)
{
    if (test())
    {
        return;
    }
}
