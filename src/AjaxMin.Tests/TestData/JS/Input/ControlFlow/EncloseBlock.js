//
// if an if-else-statement has a true-block that is technically only one statement,
// normally the braces would get crunched away around the block.
// However, if that "one" statement is an if-statement that doesn't have an else
// block, then we still need to wrap the true-block with braces or the
// outer if-statement's else block will get confused as belonging to the 
// inner if-statement. 
//
var p1;

// if inside if-else
if ( p1 )
{
  if ( p1 === null )
  {
    p1 = 0;
  }
}
else
{
  p1 = 1;
}

// if-else inside if-else
if ( p1 )
  if ( p1 === null )
    p1 = 0;
  else
  {
    if ( p1 === 1 )
      p1 = 2;
  }
else
  p1 = 1;

// labeled if inside if-else
if ( p1 )
{
  foobar : if ( p1 === null )
    p1 = 0;
}
else
{
  p1 = 1;
}

// if inside for inside if-else
if ( p1 )
{
  for(var n = 0; n < 10; ++n)
    if ( p1 === null )
      p1 = 0;
}
else
{
  p1 = 1;
}

// if inside for-in inside if-else
if ( p1 )
{
  for( var n in p1 )
    if ( p1 === null )
      p1 = 0;
}
else
{
  p1 = 1;
}

// if inside while inside if-else
if ( p1 )
{
  while( p1 != 1 )
    if ( p1 === null )
      p1 = 0;
}
else
{
  p1 = 1;
}

// if inside with inside if-else
if ( p1 )
{
  with(p1)
  {
    if ( p1 === null )
    {
      p1 = 0;
    }
  }
}
else
{
  p1 = 1;
}

// and don't forget the case where there is an empty else block
// on the inner if. the else will get crunched away, so it still
// needs to generate the right script
if ( p1 )
{
  if (p1 == "a")
  {
    p1 = 0;
  }
  else
  {
    {} // put in another nested empty block, just for fun
  }
}
else
{
  p1 = 1;
}