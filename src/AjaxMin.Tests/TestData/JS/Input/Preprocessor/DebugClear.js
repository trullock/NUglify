// normally in retail mode all the statements within this function
// would be stripped because they all touch on one of the default
// debug namespaces. But this comment will clear the set:
///#DEBUG=
function foo(bar)
{
    Msn.Debug.Log("foobar!");
    WAssert("foobar again!");
    $Debug.Trace.This;
    Debug.Assert(bar != null);
    Web.Debug("narf");
}