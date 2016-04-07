function foo(arf)
{
    if (arf)
    {
        // not in strict mode, so this function declaration shouldn't cause an error
        function bar()
        {
            alert("wow");
        }

        bar();
    }
}
