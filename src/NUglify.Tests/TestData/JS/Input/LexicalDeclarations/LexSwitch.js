var foo;
switch(foo)
{
    case 14:
        let xtra = 42;
        foo = foo / xtra * xtra;
        break;

    default:
        const foobar = "bat" + foo.x;
        foo += foobar + foo;
        break;
}
