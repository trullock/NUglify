+function(init)
{
    // apparently (and I'm not all that sure) initializers are scoped to the inner scope. 
    // Which doesn't really make a lot of sense to me, since nothing is defined yet.
    function test1( a = init, b = [location.href, document.domain] )
    {
        var init = "inner"
        alert( "a=" + a + "; b=[" + b[0] + "," + b[1] + "]; init=" + init );
    }

    test1();
}("outer");
