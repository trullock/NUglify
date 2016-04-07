for(var i=0; i < 10; ++i)
{
  var t = 20;
  {
    var a = 5;
    {
      var b = 6;
      t /= (a + b);
    }
    if ( i * t < 10 )
    {
       i += 2;
       t = 12;
    }
    i = (i + a) / t;
  }
  i *= 2.3;
}
