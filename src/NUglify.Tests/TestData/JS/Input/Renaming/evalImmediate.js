(function (Foo)
{
    function bar()
    {
        // becaue this function has an eval, when we use the -evals:immediate switch,
        // we won't rename variables in this function. AND it already has a variable
        // named "n" -- and since it references the outer "Foo" variable, we CANNOT rename
        // "Foo" to "n" or we'll be pointing to the wrong variable inside here!
        var n;
        for (n = 0; n < 10; ++n)
        {
            Foo.bar = n;
        }
        eval("alert('hi')");
    }
    return bar;
})(window.Foo || (window.Foo = {}));
