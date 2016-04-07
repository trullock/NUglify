function foo(cond)
{
    // ideally we'd even get rid of the var altogether
    // and just return the string operation:
    // function foo(cond){return"START "+(cond?"TRUE":"FALSE")+" END"}
    var text = "START ";

    // 1. this if can be changed to text += cond ? "TRUE" : "FALSE"
    if (cond)
    {
        text += "TRUE"
    }
    else
    {
        text += "FALSE"
    }

    // 2. this gets changed to return text+" END";
    // 3. then that gets combined with the previous expression to return text+(cond?"TRUE":"FALSE")+" END";
    text += " END";
    return text;
}