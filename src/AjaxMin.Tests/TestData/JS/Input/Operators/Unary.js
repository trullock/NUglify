
function bar(foo)
{
  var fn1 = (++foo).coo();
  var fn2 = (--fn1)(woo);
  var v = void ++foo;
  var t = typeof new Ack(v);
  var a = !foo || !(a || b);
  var bn = ~foo & ~(a || b);
  delete foo.bar;
  delete (foo || bar);
  var ty = typeof(foo);
  
  // any time there are three pluses or three minuses, they are grouped as (++)+ or (--)-
  // +++++ is invalid because it would be (--)(--)- which doesn't make any sense
  var a = foo+++(++bar);  // foo+++ ++bar
  var b = foo++ + bn;     // foo+++bn
  var b2= foo+++bn;       // foo+++bn
  var b3= foo+(++bn);     // foo+ ++bn
  var c = -(--foo);       // - --foo
  var d = (foo--) - bn;   // foo---bn
  var e = foo-- - bn;     // foo---bn
  var f = foo---bn;       // foo---bn (same as foo-- - bn)
  var g = foo - (-- bn);  // foo- --bn
  var h = - -g;           // h=- -g
  var i = +0;             // i=0
  var j = -0;             // i=-0

  // use a read to make sure we don't combine the assignments)
  foo(a,b,c,d,e,f,g,h,i,j);
  
  f = - +g; // don't need any spaces, since -+ isn't a separate operator
  f = - +4; // negating a positive numeric literal is just the negative of the signless literal
  h = +(+g); // put a space between the two unary ops
  h = -(-g); // put a space between the two unary ops
  g = !!h; // not-not returns a boolean value (true or false)
  
  h = g + +f; // keep a space between the binary + and the unary +
  a = b - -c; // keep a space between the binary - and the unary -
  d = b - -4; // keep a space between the binary - and the negative number
  e = e + +5; // plus a positive number is just plus the number
  f = - -g; // keep a space between the unary - and the negative number
  f = - -4; // unary - of a negative numeric literal is just the positive number
  g = + +5; // unary positive on a numeric literal is just the numeric literal, and +5 is just 5
  h = + +g; // keep a space between the two unary pluses
  i = + +0; // gets rid of the positives
  i = - +0; // get rid of the positive, but keep the negative
  j = - -0; // keep the negations, but make sure there's a space between the operators
  j = + -0; // the -0 is a unary negation on the zero, so the positive is not technically against
            // a numeric literal -- so we keep it. don't need a space because +- is not a separate operator.
}

var p = -3; // unary on a literal
var q = -(-4); // unary on an expression that is a single literal
var r = -(1 + 2) // unary on an expression that is literals in a binaryop
var s = !(!4); // s=true
var t = !(!s); // s=!!s
