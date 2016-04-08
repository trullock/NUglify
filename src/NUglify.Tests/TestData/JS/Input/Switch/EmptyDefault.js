function f(a,b,c,d,e)
{
  var x = "0";
  switch(a)
  {
      case 1:
          x = "1";
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
      case 2:
          x = "2";
          break; 
  }
  switch(c)
  {
      default: // empty default is the only statement
          break;
  }

  switch(d)
  {
    case 1:
    default: // this default gets removed because it falls through to an empty block
    case 2:
    case 3:
      break;
      
    case 4:
      a = 0;
      break;
  }
  
  switch(e)
  {
    case 1: a = 1; break;
    default:
  }
}
