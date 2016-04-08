// some global variables
var oneGlobal = 10;
var twoGlobal = 20;
var threeGlobal = 30;
var globalObj = {
    "nameOne": 1, // can be a member-dot
    "你好": 2, // should not get converted to a member-dot
    "while": 3 // reserved word should not be converted to member-dot
};

// a global function with parameters
function globalFunction(oneParam, twoParam)
{
    // a few local variables
    var oneLocal = oneParam + twoParam;
    var twoLocal = oneParam - twoParam;

    // the arguments object should never be renamed
    // outer reference to global fields
    var threeLocal = arguments.length > 2 ? arguments[2] : twoGlobal + oneGlobal;

    return localFunction(oneLocal, twoLocal);

    // a local function with one parameter that matches the parent scope parameter,
    // and another parameter that doesn't.
    function localFunction(oneParam, anotherParam)
    {
        try
        {
            // oneParam is local; others references are outer
            // even though we are VARing an "arguments" variable, it's still the
            // arguments field. We just clobber it's value when we assign to it.
            // so because we might still be able to use the arguments object's contents
            // before we assign to it, we should not rename it. 
            var arguments = oneParam + twoParam + threeGlobal + threeLocal;

            // isNaN is a predefined name -- never rename it
            return isNaN(anotherParam) ? arguments : anotherParam;
        }
        catch(e)
        {
            return e.Message;
        }
    }
}

globalFunction("one", "2");

function arf()
{
    // replace the member dot name and should always be converted to member-dot, event after eval
    var item1 = globalObj.nameOne;
    var item2 = globalObj["nameOne"];
    var item3 = globalObj["name" + "One"];

    // already a member-dot; that's okay -- but don't convert to a member dot
    item1 = globalObj.你好;
    item2 = globalObj["你好"]; 
    item3 = globalObj["你" + "好"];

    // cannot ever be a member-dot, but should get the replacement
    item1 = globalObj["while"];
    item2 = globalObj["whi" + "le"];
}


