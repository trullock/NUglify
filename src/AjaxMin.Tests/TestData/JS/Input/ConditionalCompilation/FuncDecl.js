var a = "start";

//@cc_on
//@if (@_jscript_version < 5.6)

// don't move this function because it's inside a conditional-compilation
// @if-statement!
function onlyIE(foo)
{
    alert(foo + " BAR!");
}

//@end

// we can move THIS function
function foo(bar, bell)
{
    var x = "|" + bar + "-" + bell;
    /*@
    // but DON'T move this one because it's entirely inside a conditional-comment
    function ieOnly(t)
    {
        return acr(t);

        // but I CAN move this one because nothing can be moved 
        // to be outside the comment
        function acr(txt)
        {
            return "IE: " + txt;
        }
    }
    @*/

    return x + typeof(ieOnly);
}