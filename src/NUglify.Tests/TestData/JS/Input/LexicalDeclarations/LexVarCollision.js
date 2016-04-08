// because of the collision, keep the names in sync.
// but the var is a global, so it shouldn't get renamed. So
// don't rename the lexical var, either.
for(let ndx = 0; ndx < 100; ++ndx)
{
    var ndx = 50;
    alert(ndx);
}

{
    var count = 0;
    let foo = 42;
    do
    {
        var foo = "Howdy!" + ++count;
        document.write(foo);
    } while(--foo);
}
