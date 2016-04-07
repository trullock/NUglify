
function arf(one, two, three, four)
{
    // convert an undefined value to the string representation
    if (one == undefined)
    {
        one = "undefined";
    }

    ///#if porkpie < 69
    two += four;
    ///#endif

    foobar(one, two, three);
    alert("called foobar");
    document.location = "#arf";
}