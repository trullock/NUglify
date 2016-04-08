/* @bind(MyType) */
function myobj(foo)
{
    this.x = foo;
}

/* @returns(String) */
function func1(/* @type(Number) */x) 
{
   var /* @type(Number) */ i = 10;
   return x + i;}

//@returns(String)
function func2(/*@type(String)*/ x) 
{
   var /*@type(Number)*/ i = 20;
   return "" + x + i;}

/*@bind(OtherType)*/
function construct(foo)
{
    this.y = foo;
}