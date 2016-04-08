// the rest operator puts the rest of the parameters into an array with the given name.
function foo(bar, ...ack)
{
    return bar + ack.join( " and " );
}
