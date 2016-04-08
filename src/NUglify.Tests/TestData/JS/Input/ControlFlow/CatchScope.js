function foo()
{
    var err = "local";
    var anotherLocal = 10;
    try
    {
        // throw an error
        var anotherLocal = 100 / 0;
    }
    catch(err)
    {
        // is the e parameter in the catch statement re-using the e variable in the function scope?
        // or is it a new catch-scoped variable?
        alert(err);
        var anotherLocal = 5;
    }
    alert(err);
    alert(anotherLocal);
}
