function Func(p1)
{
    var i, s, numargs = arguments.length;
    s = numargs;  
    
    if (numargs < 2)
    {
        s += " argument was passed to ArgTest. It was ";        
    }
    else
    {
      s += " arguments were passed to ArgTest. They were " ;                
    }
    for (i = 0; i < numargs; i++)
    {
         s += arguments[i] + " ";
    }
    return(s);
}