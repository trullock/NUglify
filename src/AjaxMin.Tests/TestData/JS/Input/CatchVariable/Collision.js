// this parameter is referenced inside the inner function expression
// IF the catch variable is not local only to the catch scope.
// otherwise the inner function will reference the catch scope variable,
// which will be undefined.
+function outer(e)
{
    +function tryCatch(foo, bar)
    {
        try
        {
            foo += bar
            function inner(txt)
            {
                alert(txt + e)
            }
            
            inner("e: ");
        }
        catch(e)
        {
            alert(e);
        }
    }(10,20);
}(10);
