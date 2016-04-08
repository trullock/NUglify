function foo(name)
{
    return ["www", "us"][{"uno": -1, "dos": -1, "tres": -1, "quatro": -1, "foo": 1, "bar": 1, "ack": 1, "gag": 1}[name] || 0] || name;
}

function bar(meth)
{
    // no parens around the string literal
    var a = "foobar"[3];

    // no parens around the numeric literal
    var c = 1.234e3[meth]();

    // convert the [] to ., and wrap the number in parens so the member-dot
    // doesn't get confused as a decimal point
    var b = 1.234e3["toString"]();
    return a + b;
}

var issues = (1000000000000000128).toFixed(0);
var noDec = (9).toFixed();
var dec = (0.9).toFixed();
var hex = (0xC9).toFixed();
var oct = (037).toFixed();
var zero = (0).toFixed(0);
var negZero = (-0).toFixed(0);
var badOct = (009).toFixed();
var neg = (-42).toFixed(2);
var op = (2-5).toFixed();
var exp1 = 1000000..toFixed();
var exp2 = 8e-05.toFixed(4);

