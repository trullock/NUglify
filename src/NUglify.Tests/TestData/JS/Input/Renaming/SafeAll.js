(function()
{
    // if we are making ALL eval scopes safe, then this scope is not known because a CHILD scope contains an eval and we cannot rename n.
    // but if we are making IMMEDIATE eval scopes safe, then this scope is okay and n can be renamed.
    var n = "outer n";

    function foo(code)
    {
        // this scope is not known because it contains an eval
        return eval(code);
    }

    function bar(arg1, arg2)
    {
        // this scope is known.
        // we reference the outer n variable, which may be in an unknown scope.
        // if it is, then we won't renamed it and arg1, which would normally be renamed to n,
        // will have to use some other name.
        alert(n + arg1 + arg2);
    }

    // make sure bar is referenced more than n so that it will get named n if
    // this scope is marked as known (and safe for renaming)
    bar("one", "two");
    bar("three", "four");
    foo("debugger");
})();
