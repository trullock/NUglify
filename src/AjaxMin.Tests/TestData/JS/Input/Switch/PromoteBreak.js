function foo()
{
  for(var a=0; a < 10; ++a)
    switch(a)
    {
      case 1:
        a = 0;
        // fall through...
        
      default:
      case 2:
      case 3:
      case 4:
        break; // this break gets added to end of case 1
        
      case 5:
        a = 1;
        continue;
        
      case 6:
      case 7:
        break; // this break does NOT get added to case 5
      
      case 8:
        a = 2;
        break;
        
      case 9:
        break; // doesn't get added to case 8
        
      case 10:
        a = 3;
        throw "error";
      
      case 11:
        break; // doesn't get added to case 10
        
      case 12:
        a = 4;
        return -1;
        
      case 13:
        break; // doesn't get added to case 12
    }
}
