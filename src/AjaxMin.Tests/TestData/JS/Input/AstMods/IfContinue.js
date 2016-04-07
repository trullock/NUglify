// simple if-continue
for(var ndx = 0; ndx < 10; ++ndx)
{
    if (ndx == 5)
    {
        continue;
    }
    alert("ndx: " + ndx);
}

// continue with a label that is NOT the parent
for(var ndx = 0; ndx < 10; ++ndx)
{
    if (ndx == 5)
    {
        continue ack;
    }
    alert("ndx: " + ndx);
}

// continue with a label that IS the parent
loop:for(var ndx = 0; ndx < 10; ++ndx)
{
    if (ndx != 5)
    {
        continue loop;
    }
    alert("ndx: " + ndx);
    break;
}

// continue with a label that IS the parent, nested
foo:bar:for(var ndx = 0; ndx < 10; ++ndx)
{
    if (ndx == 5)
    {
        continue foo;
    }
    alert("ndx: " + ndx);
}

var ndx = 0;
while(++ndx < 10)
{
    // the two ifs will get combined
    if (ndx == 5)
    {
        continue;
    }
    if (ndx % 2 == 1)
    {
        break;
    }
}
