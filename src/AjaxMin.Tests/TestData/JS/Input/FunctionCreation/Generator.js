
// generator function
function * myGenerator( array, max )
{ 
    for(var item of array)
    {
        yield item;
    }
}

