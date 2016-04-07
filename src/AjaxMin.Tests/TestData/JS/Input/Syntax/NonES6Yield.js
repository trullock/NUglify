// ES6 has the yield operator.
// but if we aren't ES6, yield can be used as an identifier!
function test1(a)
{
    // create a yield variable
    var yield = +a;

    // use a multiplier operator to REALLY confuse the issue,
    // since IE6 uses yield* as a delegator operation.
    alert(yield * 42);
}

// Mozilla generator functions aren't marked with *, but yield
// is still an operation.
function myGenerator(arr)
{
    for(var item in arr)
    {
        yield item;
    }
}

