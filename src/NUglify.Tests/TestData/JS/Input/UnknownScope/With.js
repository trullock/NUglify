
// start everything off with a function scope
(function()
{
    // the foo variable is not referenced in the with scope, so it's okay
    // to crunch it. The ack variable, however, is referenced, so it should
    // not get crunched.
    var foo = "bar";
    var ack = "gag";

    // nested function that contains an with-statement
    function doIt(txt)
    {
        // this variable shouldn't get crunched because it is referenced
        // inside the with-statement
        var suffix = ";";
        
        with (txt)
        {
            // this is a local reference inside the with statement. If the txt
            // object has a "ralph" property, it will refer to that property. 
            // if not, it will refer to a variable in the outer scope.
            var ralph = "a";
            
            // the suffix and ack variables may well be pointing to the locals in the
            // parent scopes, or they may be pointing to properties on the txt
            // object. We don't know. 
            // But the bar variable isn't defined anywhere.
            // it's either an undefined global or a property on the txt object.
            // Either way, we shouldn't *error*, but we should report it as 
            // a possible undefined global.
            alert(gag(bar + ack + suffix));
        }
    }

    // this function is referenced inside the with clause, so its name
    // should not get crunched.
    function gag(txt)
    {
        return '"' + txt + '"';
    }

    // this function isn't referenced inside the with scope,
    // so it should get crunched just fine.
    function another()
    {
        // this var should get crunched, and so should foo
        var delim = foo + " boy-howdy! ";
        
        // foo should get crunched, but ack should not
        return foo + delim + ack;
    }

    // call the functions for a reference
    doIt("alert('" + another() + "')");
})();

function test(a)
{
    with(a)
    {
        $Debug.Alert(foo);
    }
}

