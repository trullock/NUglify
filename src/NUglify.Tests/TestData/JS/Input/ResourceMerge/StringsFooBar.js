function foo()
{
    var first = Strings.Foo.Bar.First;
    var not = Strings.Ack.Bar.Not;

    var second = Strings["Foo"].Bar["Second"];
    return first + ' ' + second;
}
