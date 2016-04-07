function Func(p1)
{
    p1 = new Object;                // crunched to {}
    p1 = new Array();               // crunched to []
    p1 = new Array(1, 2, 3);        // crunched to [1,2,3]
    
    p1 = new Object("foo");         // argument causes not to be crunched to object literal
    p1 = new Array(12);             // single numeric argument is array size -- not crunched to array literal
    
    p1 = {};                        // object literal
    p1 = [];                        // array literal
    p1 = new Date("Jan 5 1996");    // date constructor
    
    p1 = new function(arf)          // singleton creation
    {
        return arf*10;
    };
}