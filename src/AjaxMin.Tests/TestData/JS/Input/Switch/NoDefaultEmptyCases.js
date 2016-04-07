function f(a,b,c)
{
  var x = "0";
  switch(a)
  {
      case 1: // empty first case gets removed
          break;
      case 2:
          x = "2";
          break; 
      default: // empty default is the last statement
          break;
  }
  switch(b)
  {
      case 1:
          x = "1";
          break;
      default: // empty default is NOT the last statement
          break;
      case 2: // empty last case gets removed 
          break; 
  }
  switch(c)
  {
      case 1:   // empty cases gets removed
      case 2:
          break;
      default: // empty default is the only statement
          break;
  }
}

