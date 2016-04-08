function yes1(p1)
{
    // the single reference means the variable gets removed,
    // then the array contructor gets replaced with an array literal,
    // then the array literal length property gets evaluated
    var a = new Array(0,1,2,3,4);
    return(a.length);
}

function yes2()
{
    // the missing values still count towards the length
    return [1,,,,5].length;
}

function yes3()
{
    // recurse the spreads
    return [1, 2, ...[3, ...[4, 5], 6], 7].length;
}

function no1()
{
    // the trailing commas mean there are cross-browser differences in the
    // length of the literal, so don't evaluate it
    return [1,2,3,,].length;
}

function no2()
{
    // one of the spreads isn't actually an array literal, so don't
    // evaluate it, even though it's a constant.
    return [1, 2, ...[3, ...4, 5], 6].length;
}
