
// the typeof operand can be an undefined lookup -- it won't throw a Reference error, it will just return "undefined"
// if it's off the window object and the name doesn't resolve to anything else,
// then take the "window." part off.
var foo = typeof window.bar;

// don't do it if window will resolve to something else
// (even if it is the window object at run-time)
(function(window)
{
    alert(typeof window.foobarbatack);
})(window);

// and don't do it if it's inside a with-statement
with(window.ackbar)
{
    alert(typeof window.qwerty);
}

// and don't do it if the name would map to something NOT in the global space
function test(foop)
{
    // actually, if the parameter gets renamed, then "foop" WOULD resolve to the
    // global namespace. Might want to consider that possibility!
    alert(foop + typeof(window.foop));
}
