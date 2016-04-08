(function()
{    
    // just make sure this variable is crunched to "a" so we don't
    // accidentally get a false-positive if the catch variable just defaults
    // to the first possible variable name
    var bar = 12;
    bar *= (bar + 13) % bar;
    
    try
    {
        // "undefined variable" error
        alert(foo);
    }
    catch(err)
    {
        alert(err);
    }
})();
