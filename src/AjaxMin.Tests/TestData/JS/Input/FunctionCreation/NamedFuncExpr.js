function test1()
{
    // example of an ambiguous function expression name.
    // IE puts the name in the containing scope when it should only be within its own scope.
    // IE will throw an error with this code; non-IE will work just fine.
    var foo = 10;
    (function foo(count)
    {
        if (--count > 0)
        {
            // IE throws an error here because foo has been set to 10 in the outer scope
            // and it doesn't overide the name with the function within the function's scope.
            // all other browsers can tell the difference.
            foo(count); 
        }
    })(10);
    alert(foo); //

    // this is a local named function expression whose name
    // if never referenced, and therefore the name can be
    // excluded from the crunched output unless we want to keep all function names
    var bar = function local_never_referenced() { };

    // only one safe reference to the function name (within the function itself).
    // crunch the name because it's a local, but because IE puts the name in the containing
    // scope (not just the funtion's scope), we need to make sure the crunched name doesn't
    // interfere with any other names in the containing scope
    (function local_one_self_ref(count) { if (--count > 0) { local_one_self_ref(count) } })(10);

    // also test in a function scope whether it's okay to assign the named function expression
    // to a variable of the same name -- it should be. If the named function expression isn't used
    // within itself, then the name can be omitted unless we ask for them to be kept. 
    var ack = function ack() { };
    ack.bar = 10;

    // But if it is, the name needs to stay (but it can be crunched)
    // AS LONG AS THE CRUNCHED NAME OF THE FUNCTION EXPRESSION IS THE SAME
    // AS THE OUTER VARIABLE NAME.
    var trap = function trap() { trap(); };
    trap.bar = 11;

    // just to reference bar
    return bar();
}

// this global named function expression should never have any
// references to the name, and therefore the name can be excluded
// from the crunched output unless we want to keep them all
var foo = function global_no_references() { };

// a global named function with a single self-reference
// can't remove the name because we are referencing it within the scope (the proper way to do it).
// can't crunch the name because it affects the global namespace in IE
foo = function global_one_self_ref(count) { if (--count > 0) { global_one_self_ref(count) } };

// but it should be okay to set a global variable with the same name as the function expression.
// this particular function expression does NOT self-reference, so the function name should be
// removed when crunched unless we've explicitly said we want to keep them all.
var ack = function ack() { };
ack.bar = function () { return ack[12]; };

// but this one DOES self-reference. We might think we can rename the inner reference because we
// are detaching it from the outer, but WE CAN'T. IE browsers will create that function name in the
// outer scope, which in this case is the global scope -- we don't want to rename globals, so we
// can't rename the function expression name.
var trap = function trap() { trap(); };
trap.bar = 13;

// testing the various "ambiguous" errors
function test1()
{
    var a1;
    var b = function a1() {};  // ERROR, function name removed normally
}

function test2()
{
    var b = function a2() { alert(a2) }; // function name kept, still has to match var
    var a2; // ERROR
}

function test3()
{
    var b = function a3() { alert(a3) }; // keep function name
    var c = function a3() { }; // NO ERROR, normally remove function name
    var d = function a3() { }; // NO ERROR, normally remove function name
}

function test4()
{ 
    // none of these function names should be kept normally
    var b = function a4() { };
    var c = function a4() { }; // NO ERROR
    var d = function a4() { }; // NO ERROR
    var a4; // ERROR
}

function test5()
{
    var a5 = function a5() { }; // NO ERROR, normally remove the function name
}
