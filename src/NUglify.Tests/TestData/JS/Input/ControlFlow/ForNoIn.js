function foo(obj, name)
{
    // the in-operator will get moved into the initializer of the for-statement
    // but it can't have an in-operator by itself, so make sure it gets wrapped in parens
    var count = 0, i, isThere = name in obj;
    for (i = 0; i < 10; ++i)
    {
        if (isThere) ++count;
    }
    return count;
}

function bar(name, obj)
{
    var i = 0, n = obj ? name in obj : 0, m = !(name || name in obj);
    for (; i < 10; ++i)
    {
        return i;
    }
}

function ack(name, obj, isIn)
{
    var i = 0;
    isIn = name in obj;
    for (; i < 10; ++i)
    {
        return i;
    }
}
