function foo(bar)
{
    if (bar)
    {
        alert(Strings.Message + bar);
    }
    else
    {
        alert(Strings.Message + "[null]" + "!" + "!" + b);
    }
}
foo("bar!");


// this string won't exist; when we encounter this situation, the
// property should be replaced by an empty string literal
var a = Strings.DoesntExist; 

