try 
{
  print("Outer try running..");
  
  try 
  {
    print("Nested try running...");
    throw "an error"; // safari-quirks mode (-m) will not strip this semi-colon out
  }
  catch(e) 
  {
    print("Nested catch caught " + e);
    throw e + " re-thrown"; // safari-quirks mode (-m) will not strip this semi-colon out
  }
  finally 
  {
    print("Nested finally is running...");
  }   
}
catch(e) 
{
  print("Outer catch caught " + e);
}
finally 
{
  print("Outer finally running");
}

function print(s)
{
   document.write(s);
}

// empty try block, no catch (don't know why, but it's syntactically valid)
try
{
}
finally
{
  alert()
}

// empty catch block, no finally
try
{
  a = 10 / 0;
}
catch(e)
{
  // ignore errors -- must not be stripped out
}

// empty catch block with a finally
try
{
}
catch(e)
{
  // ignore errors -- must not be stripped out
}
finally
{
  alert();
}

// empty catch block with an empty finally block
try
{
}
catch(e)
{
  // ignore errors -- must not be stripped out
}
finally
{
  // empty finally block should be stripped out
}

// empty finally block and no catch
try
{
}
finally
{
  // must not be stripped out
}


