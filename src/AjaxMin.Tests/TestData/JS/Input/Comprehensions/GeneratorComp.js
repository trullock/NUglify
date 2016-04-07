function test1()
{
    for(var n in (for(x of [1, 2, 3, 4, 5]) if (x % 2) x * x))
    { 
        alert(n);
    }
}