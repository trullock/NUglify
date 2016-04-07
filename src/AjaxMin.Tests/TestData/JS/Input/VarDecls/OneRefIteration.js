function test1(callback, arr, max)
{
    // callback will add stuff to the object, so create it once and keep
    // passing it in to subsequent callbacks
    var obj = {};
    for (var ndx = 0; ndx < max; ++ndx)
    {
        callback( arr[ndx], obj );
    }
}

function test2(callback, arr, max)
{
    // callback will appended to the array, so create it once and keep
    // passing it in to subsequent callbacks
    var list = [];
    for (var ndx = 0; ndx < max; ++ndx)
    {
        callback( arr[ndx], list );
    }
}
