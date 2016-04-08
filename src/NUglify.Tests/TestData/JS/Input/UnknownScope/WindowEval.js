// same as Eval.js, but with a window.eval instead
// start everything off with a function scope
(function()
{
    // couple variables that would normally get crunched in hypercrunch,
    // but the eval in a lower scope turns crunching off
    var foo = "bar";
    var ack = "gag";

    // nested function that contains an eval
    function doIt(txt)
    {
        // this variable shouldn't get crunched because of the eval
        var suffix = ";";
        window.eval(txt + suffix);
    }

    function another()
    {
        // this var should get crunched
        var delim = foo + " boy-howdy! ";
        return foo + delim + ack;
    }

    // call the eval for a reference
    doIt("alert('" + another() + "')");
})();
