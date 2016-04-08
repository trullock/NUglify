function foo(root)
{
  function level1a(a)
  {
    root &= 65535;
    function level2a(c)
    {
      return c + 'foobar' + 6.5535e04;
    }
    
    if (a == "foobar")
    {
      return level2a(a);
    }
    else
    {
      return "foobar";
    }
  }
  
  function level1b(b)
  {
    function level2b(d)
    {
      return d.indexOf("foobar") & 0xffff;
    }
    
    return level2b(b);
  }
  
  if (root > 65535)
  {
    root -= 0xffff;
  }
  return level1a(root).substring(level1b(root));
}

function globalFunc()
{
  // since the common scope between this function and the previous
  // function is the global scope, then this one will not share the 
  // same literal shortcut
  return "foobar";
}