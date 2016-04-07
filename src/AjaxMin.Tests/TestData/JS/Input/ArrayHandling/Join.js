function Func(p1)
{
    var a, b;
    a = new Array(0,1,2,3,4);
    b = a.join("-");
    return(b);
}

function yes1()
{
    return ["one", "two", "three"].join(',');
}

function yes2()
{
    // not passing a separator means to use a comma
    return ["one", 2, "three"].join();
}

function yes3()
{
    return ["one", 2, "three or " + 42 + " more"].join("-");
}

function yes4()
{
    return "and a " + [1,2,3].join(" and a ") + " [music]";
}

function yes5()
{
    return "missing [" + [1,,3].join() + "] number 2";
}

function no1(a)
{
    // not a constant join separator
    return ["one", "two", "three"].join(a);
}

function no2()
{
    // more than one parameter -- that ain't right
    return ["one", "two", "three"].join(',', 42);
}

function no3(a)
{
    // not all the array literal elements are constants
    return ["one", "two", "three", a].join(',');
}

function no4()
{
    // the joined string is longer than the code
    return ["a", "b", "c"].join(" always comes before ");
}

function no5()
{
    // the decimal double is a no-go -- we don't know if there is
    // a cross-browser or cross-platform difference in the precision
    // or the text conversion
    return ["one", -1234567890.12345678, "three"].join();
}

function no6()
{
    // trailing commas is a no-go, too, because of cross-browser issues
    return [1,2,].join();
}
