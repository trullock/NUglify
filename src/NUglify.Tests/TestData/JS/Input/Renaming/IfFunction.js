function woot(bar)
{
    if (bar != null)
    {
        // ES6 defines this function inside the block scope of the if-statment.
        // ES5 says this function is invalid, and SOME browsers define it in the variable-scope
        // on parse, and SOME won't define it until the block is executed. Either way, we 
        // should make sure the name doesn't collide with anything outside the block
        // scope to keep the cross-browser issues at bay.
        function foo(arf)
        {
            alert(arf);
        }
        foo("Hi");
    }

    alert(bar);
}
