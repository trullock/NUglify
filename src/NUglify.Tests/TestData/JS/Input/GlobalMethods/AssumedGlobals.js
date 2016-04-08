function foo(txt)
{
    if (window.bar)
    {
        bar(txt);
    }
}

function ack(txt)
{
    if (window["bat"])
    {
        bat(txt);
    }
}