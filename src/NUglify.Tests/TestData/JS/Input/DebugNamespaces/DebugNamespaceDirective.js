///#debug=Foo.Bar
function foo(arf, bat)
{
    // by default, Foo would be an unknown global. But if we define Foo.Bar
    // as a debug namespace, Foo should also be recognized as a known global.
    Foo.Bar.Log("Arf: " + arf + "; bat: " + bat);
    return arf + bat;
}
