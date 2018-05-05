function test1(a)
{
    return { x: a + 1, y: a - 1 };
}

function test2(a)
{
    // ES2015 Object initializer
    return {
        a
    };
}

function test3(a)
{
    delete a.x;
    return a;
}