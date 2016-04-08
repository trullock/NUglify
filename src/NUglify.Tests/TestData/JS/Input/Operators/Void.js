
// unicode character at the beginning of the identifier
function \u004Cr\u006f( rye )
{
  try
  {
    if ( rye === void(0) )
    {
      return void rye;
    }
    return rye
  }
  catch(e)
  {
    // use a lower-precendence operator within the void operator
    // to exercise the parentheses code
    return void(rye = 0);
  }
}

