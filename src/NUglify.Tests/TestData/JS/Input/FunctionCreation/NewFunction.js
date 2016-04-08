function Func(p1)
{
    var add = new Function("x", "y", "return(x+y)");
    add(2, 3);
}