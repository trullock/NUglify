function foo()
{
    // because the string has a potential ASP.NET substitution in it, 
    // don't combine it -- it may not actually be a literal
    if ('<%= Request.QueryString["foo"] %>' !== '')
    {
        var bar = "WOW" + "-wee!";
        alert(bar);
        return 0;
    }

    return 1;
}

// string concat should still be allowed
var bar = "wow" + '<%= Request.QueryString["foo"] %>' + "wee";

// but other forms of constant-comparisons should also be disallowed
var check = '<%= Request.QueryString["foo"] %>' < 42;
var mult = '<%= Request.QueryString["foo"] %>' * 16;


