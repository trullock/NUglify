function Func(p1, p2)
{
    var r;
    r = p1 * p2;
    return(r);
}
function foo()
{
  try
  {
    // whatever
  }
  catch(e)
  {
    // return from inside catch scope
    return 0;
  }
  return 1;
}
function bar()
{
  with(obj)
  {
    // return from inside with scope
    return 0;
  }
  return 1;
}