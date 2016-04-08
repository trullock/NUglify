"use strict";

function foo(arf)
{
    if (arf)
    {
        // STRICT MODE! This function declaration should cause an error
        function bar()
        {
            alert("wow");
        }

        bar();
    }
}
