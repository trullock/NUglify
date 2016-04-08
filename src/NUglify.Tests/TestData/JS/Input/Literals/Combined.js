// the null literal will be combined here into a local variable
// within the top-most function because that's the closest common
// scope root. The name of that variable MUST not collide with
// any other names in the child scopes.
function arf()
{
  var ralph = 10;
  function foo(arg)
  {
    if (arg == null)
    {
      bar(null);
      garp(ralph);
    }
  }
  
  function bar(arg)
  {
    if (arg != null)
    {
      alert(arg);
    }
  }
  
  function garp(arg)
  {
    if (arg == null)
    {
      alert(null);
    }
  }
  
  foo();
}


function foo(bar)
{
  // just mention "undefined" a few times so it will get combined
  if (typeof bar == "undefined")
  {
    return 'undefined'; // use a different delimiter -- should still pick up as the same string
  }
  else
  {
    var ack;
    try
    {
      ack = bar.foo();

      // literals inside a with-scope should NOT get picked up. There's no way to know if the 
      // variable name we eventually use will be a property on the with-object
      with(ack)
      {
        alert(ack.bar + "undefined");
      }
    }
    catch(err) // try this both with catch as local and catch scoped
    {
      // should still get combined if catch is scoped
      ack = "undefined";
    }
    if (ack == "undefined")
    {
      alert("undefined");
    }
    return ack;
  }
}