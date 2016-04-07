
// global scope
function foo(a)
{
    // function declarations should only be at global scope, or as direct children
    // of another function declaration. They should technically NOT be inside if-statements
    // and such. Different browsers handle those situations different, as the ECMA spec says
    // functions are not allowed there at all.
    if (a)
    {
        bar(a);

        // this function should NOT be at this location. We should throw a warning, as Firefox
        // will actually throw a "bar not defined" error when it tries to call it in the
        // previous statement.
        function bar(x)
        {
            alert(x);
        }
    }
}

