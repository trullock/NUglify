if (location)
{
    // can't redefine a lexical declaration
    let x = location.href;
    let x = 42;
    alert(x);
}

for(let ndx = 0; ndx < 10; ++ndx)
{
    const ndx = 42;
    alert(ndx * ndx);
}
