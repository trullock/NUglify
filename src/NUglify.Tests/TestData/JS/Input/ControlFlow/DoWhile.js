function Func(p1)
{
    var i = 0;
    do
    {
        if (i == p1)
        {
            break;
        }
        i++;
   }
   while (i < 100);
}


// if-else clauses with do-while as the only statement need to have 
// curly-braces surrounding them or IE will throw script errors
// (although other browsers will work just fine)
var a = 0;
if(true){
  do
  {
    a = 1;
  } while(false);
}else
  do
  {
    a = 2;
  } while(false);

// and if-clause that contains a do-while but doesn't have an else clause
// doesn't need to wrap the if-clause in curly-braces
if (true)
  do
    a = 3;
  while (false);
  
// while statements don't seem to need to wrap a single do-while with curly-braces
while(false)
  do 
    a = 4;
  while(false);
  
// nor for statements
for(var ndx=0; ndx< 10; ++ndx)
  do
    a = ndx;
  while(false);
  
// but do-whiles need do-whiles wrapped in curly-braces as well
do
  {do a=6;while(false)}
while(false);


// and it has to recurse
if (0)
  for(ndx=0; ndx<10; ++ndx)
    while(0)
      do a = ndx; while(0);
else
  a = 10;

  
alert(a);

// this is a single statement within the do-while
// so there shouldn't be any braces
// the last statement is a throw -- for MacSafari compatibility
// it should always end in a semi-colon. But the code
// in do-while needs to recognize that it already HAS a semicolon
// before it tries to append one after the only statement in the block
do
{
    if (a)
    {
        throw "error";
    }
} while (a);
