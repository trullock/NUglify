// we're going to set a debug namespace in our code
// using the ///#DEBUG=(namespace) syntax. If the namespace
// is blank, then it removes all the debug namespaces; otherwise
// it adds the namespace to the list of debug namespaces.

///#DEBUG=Foo.Bar
function ack(bar)
{
    // here is a call to a property under the debug namespace
    // we defined above. In debug mode it should remain; but in
    // retail mode (default or -debug:0) it should be removed.
    Foo.Bar.alert(bar);
    return bar * bar;
}