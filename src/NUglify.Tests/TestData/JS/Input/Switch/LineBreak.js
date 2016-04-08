var text = "the quick brown fox jumped over the big log";
var foo = 42;

function bar(first, second)
{
    if (first > second)
    {
        return foo + ':' + second + first + text;
    }
    
    return foo + ':' + first + second + text;
}
