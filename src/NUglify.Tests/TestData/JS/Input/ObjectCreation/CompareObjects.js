function CompareObjects()
{
    var add = new Function("x", "y", "return(x+y)");
    var add1 = new Function("x", "y", "return(x+y)");
    
    if(add == add1)
    {
        alert("same function");
    }
    
    delete add;
    delete add1;
}