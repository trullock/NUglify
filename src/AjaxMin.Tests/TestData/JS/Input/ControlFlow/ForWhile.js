function Func(p1)
{
    var i = 0, j = 0;
    while (i < 100)
    {
        if (i == p1)
        {
            break;
        }
        i++;
    }

    for(i=0; i < 100; i++)
    {
        if (i == p1)
        {
            break;
        }
    }
   
    for (i = 0; i < 10; i++, j++)
    {
       p1 = i + j;
    }
    
    for(;;)
    {
        break;
    }

    // opportunity for more crunching:
    // after changing the while to a for, move the preceeding expressions
    // into the initializer.
    var a = 10;
    var b;
    var c = 42;
    while(1)
    {
        if (++a > c)
            break;
    }
    
    // empty body on the while
    while(--i > 0);
    
    // empty body on the for
    for(var t = 10, y=5; t > 0; t--, y+=y)
    {
    }

   return(i);
}