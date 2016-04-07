
// should not be exporting from a function
function foo(bar)
{
    var a = bar * bar;
    export {a as BarSquared};
    return a;
}
