function foo(arr)
{
  // normal hypercrunch (-H) will crunch this local variable name.
  // but -HL will not crunch L_ variables 
  var L_Error_Text = "now is the time";
  for(var ndx = 0; ndx < arr.length; ++ndx)
  {
    if ( !arr[ndx] && L_Error_Text)
    {
      bar(function()
      {
        alert(L_Error_Text);
      });
    }
  }
}

