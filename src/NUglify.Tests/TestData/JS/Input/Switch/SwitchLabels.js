foo:for(var a = 0; a < 10; ++a)
{
  bar:switch(a)
  {
    case 1:
      a *= 1;
      break;
      
    case 2:
      a *= 2; // fallthrough gets promoted break
      
    case 3:
    case 4:
      break bar; // remove cases because it matches switch
      
    case 5:
      break foo; // do not remove; doesn't match switch
      
    default:
      break bar; // remove because it matches switch label
      
    case 99:
    {             // braces should be removed
      a /= 10;
      break;      // last break should be removed
    }
  }
  switch(a)
  {
    case 2:
      a *= 2;
      break;
      
    default:
      break foo; // do NOT remove -- label doesn't match switch (switch has no label)
  }
  ack:switch(a)
  {
    case 3:
      a *= 3;
      break ack;
      
    default:
      break foo; // do NOT remove -- label doesn't match switch
  }
}
