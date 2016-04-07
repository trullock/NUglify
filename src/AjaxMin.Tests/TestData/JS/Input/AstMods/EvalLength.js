function yes1()
{
    // the single reference means the variable gets replaced.
    // the unicode escape only counts towards a single character.
    var a = "one\u0032Three";
    return a.length;
}

function yes2()
{
    // the join will get evaluated to a string, 
    // then concatenated with the other literal,
    // then the length will get evaluated on the resulting string
    return ([1,2,3].join() + 6).length;
}


function no1()
{
    // even thought the \v will always be a single character in all browsers
    // (just not the SAME single character), because the literal will "have issues,"
    // don't evaluate the length
    return "1\v2\v3".length;
}
