var Strings={Extra:'And a string we ain\'t gonna "use"',"Id with \"quotes\"":"some other string we aren't going to use",Message:"You called the 'foo' function ",\u4f60\u597d:"means hello!"};
///#source 1 1 TESTRUNPATH\TestData\JS\Input\ResourceMerge\ResourceResx.js
function foo(bar)
{
    if (bar)
    {
        alert(Strings.Message + bar);
    }
    else
    {
        alert(Strings.Message + "[null]" + "!" + "!" + b);
    }
}
foo("bar!");

