
// four is the only referenced argument.
// last three bindings can be entirely deleted.
// the first two can be emptied, but not deleted.
function test1({one, two}, [,,three,,,], four, {five, six}, [seven], eight)
{
    return four;
}

// the trailing commas can be removed, but
// the leading commas have to stay so refd picks up the right index.
function test2([,,,refd,,,])
{
    return refd;
}

// the first and last property are unreferenced and can be deleted
function test3({one, two, three})
{
    return two;
}

function test4()
{
    // no initializer! This should throw an error, AND since none of the
    // bindings are actually referenced, the whole thing should be removed.
    var [foo, bar];
    return "fail";
}

function test5(coll)
{
    // we don't actually reference the binding name, so we can delete it
    // from the pattern. However, we need to leave SOMETHING, so the empty
    // pattern needs to remain behind.
    for(var {name: propertyName} in coll)
    {
        return true;
    }
}

function test6(coll)
{
    // the binding name isn't referenced inside the loop, but the binding pattern
    // itself is a reference. So we wouldn't be deleting it. HOWEVER, the previous
    // var statement is declaring the name of the only binding in the for-in's variable
    // property, so the var gets moved into the for-in statement. At that point the
    // variable THEN has no references, so it DOES get cleaned up to an empty binding.
    var prop;
    for({prop} in coll)
    {
        return true;
    }
}

function test7(coll)
{
    // a var containing a binding for multiple names, all of which
    // are in the for-in variable. The matching bound names in the declaration
    // should be moved to the for-in (but ONLY the matching ones).
    var {one, two, three} = {};
    for({one,two} in coll)
    {
        return one+two+three;
    }
}

function test8(coll)
{
    // the outside declarations should get moved to the inside
    var {one, two} = coll;
    var ndx = 0;
    for(var {protocol,hostname} = location; ndx < 10; ++ndx)
    {
        alert(protocol + hostname + ndx + one + two);
    }
}
