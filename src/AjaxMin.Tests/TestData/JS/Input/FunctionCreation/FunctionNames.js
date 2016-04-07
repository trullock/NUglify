// normal function name
function foo()
{
    // old JScript-style extension for hooking event handlers.
    // function name is OBJECT::EVENT. Don't rename it, don't remove it.
    function MyButton::OnClick()
    {
        alert("CLICK!");
    }

    return 0;
}

// normal named function expression
var f1 = function ralph() { return ralph; };

// unnamed function expression
var f2 = function() { return 2; };

// this is technically invalid, but we want to keep the name as-is in the output
// because some versions of IE allow this. Throw an error, though.
function window.onload()
{
    alert("ONLOLAD!");
}

// error - no name for a function declaration
function(param1)
{
    return param1 + param1;
}

// error - name, but no param list
function bar { return 3; }

// error - no name OR parameters
function{return 4;}

// just to make sure recovery worked.
alert(f1);


