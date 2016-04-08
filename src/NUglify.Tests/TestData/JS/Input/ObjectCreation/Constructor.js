function Func(p1)
{
    p1 = new String("Hi");
    
    if (p1.constructor == String)
    {
        alert("Hi");
    }
}
function MyFunc() 
{
  // because there are four references to the "this" literal,
  // hypercrunch should add a local variable initialized to the "this" literal,
  // and use that variable instead of the source literals.
  this.Foo = "1";
  this.Bar = "2";
  this.Ack = "3";
  this.Gag = "4";
  
  function arf()
  {
    // but these two this literals are not directly in the parent function's scope
    // (they're in a child scope), so they should not be replaced with the same
    // generated local variable
    this.Ralph = "first";
    this.Cramden = "last";
  }
  
  this.Barf = new arf();
}
var y = new MyFunc;

if (y.constructor == MyFunc)
{
    alert("My");
}
