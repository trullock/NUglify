//
// these are the items that are changed with the MacQuirks flags
//
var foo;
var bar;

// throw statements should always end with a semi-colon with the
// macquirks flag on because Safari will throw a syntax error if
// it doesn't.
if (foo == bar)
{
  alert(foo);
  throw "foo equals bar"; // normally the last statement in a block doesn't need a semi-colon
}

// Safari also throws a syntax error if a block only contains a function declaration and
// it isn't surrounded with curly-braces
if (foo != bar)
{
  function foobar()
  {
    return foo || bar;
  }
}
