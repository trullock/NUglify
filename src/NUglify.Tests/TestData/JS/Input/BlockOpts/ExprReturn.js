function test1(a, b, c)
{
    // transform to return a(b)
    // c is a local variable (parameter) so there is no real reason to
    // assign to it and then exit. change to return a(b) and remove
    // the reference to c (which then removes it from the parameter list).
    c = a(b);
    return c;
}

// actually for these two, we want to combine the previous expression
// with the return operand via a comma-operator -- we don't want to make
// it "return a.foo.bar=b+c" because that would return the value of b+c,
// but assigning the value to the property might have side-effects, and the
// subsequent get might not be the same value that was set!
function test2(a, b, c)
{
    a.foo.bar = b + c;
    return a.foo.bar;
}

function test3(a, b, c)
{
    a[b + c] = a + b + c;
    return a[b+c];
}

// for this one, the lookup is for an outer variable, so
// don't remove the assignment. Other code in other scopes 
// might be looking at it.
function test4(a, b, c)
{
    function foo(a, b)
    {
        // c is an outer variable, so don't remove the
        // assignment. But we can remove the operand reference and
        // just make it return c=a+b
        c = a + b;
        return c;
    }

    return foo(a, b);
}
