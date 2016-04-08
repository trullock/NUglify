// this function expression doesn't need parens
var t = new function()
{
  // no parens around this function expression -- good
  this.foo = function() { return (function(){return "foo"}); }
  
  // need to remove the parens around this function expression.
  // they could cause problems with Safari
  this.bar = (function() { return (function(){return "bar"}); });
  
  // don't need the parens around these function expressions -- remove them if they exist
  // these are roots for members
  var x = function(){return "woof!";}.ToString();
  var y = (function(){return "bark!";}).ToString();
  // these are func for calls
  var p = function(){return this;}();
  var q = (function(){return this;})();
  
  // function expression as parameter
  var z = setTimeout(function arf(){return "foo";}, 5000); // 5000 should get crunched to 5e3
};
t.foo();

// need to KEEP the parens around this function or there will be a syntax error
// because otherwise the function keyword would be first in the statement, and it
// would try to parse a declaration, not an expression.
// this one is a member
(function(p)
{
  return p * 10;
}).as("foo");
// this one is a call
(function ralph(foo)
{
  alert("ralph");
})();
// this one is a call buried in some other structure
(function(){return true;})() == true ? alert("true") : alert("false");

// pseudo-realworld example
var addEvent = function()
{
   function ie_addEvent(el, evt, fn)
   {
      el.attachEvent('on' + evt, fn);
   }

   function w3c_addEvent(el, evt, fn, useCap)
   {
      el.addEventListener(evt, fn, useCap);
   }

   if (typeof window.addEventListener !== 'undefined')
   {
      return w3c_addEvent;
   }
   else if (typeof window.attachEvent !== 'undefined')
   {
      return ie_addEvent;
   }
}();   // <= This is the "magic" part!

// this is an MSN-VOODOO construct for defining a method on an existing
// prototye, if it doesn't already exist
String.addMethod("trim", function(){return this;});
