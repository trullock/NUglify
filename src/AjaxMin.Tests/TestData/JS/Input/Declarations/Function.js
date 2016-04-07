function foo()
{
  var bar = 7;
  
  // duplicate name
  function bar() { return 42; }
  
  with(bar)
  {
    // function defined within a with scope
    function arf()
    {
      return 21;
    }
    arf(5,6,,8); // missing parameter in list
  }
  
  // no one references this functions, so we can remove it
  // unless we specify the -unused:keep
  function ack()
  {
    alert("no one calls");
  }
}
