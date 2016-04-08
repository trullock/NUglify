module "test1"
{
    function sum(a, b)
    {
        return a + b;
    }

    var ralph = "bus driver";
    var ed = "sewer rat";

    export {ralph, ed, sum, window as w};
}

