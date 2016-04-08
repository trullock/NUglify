
// multiple parameters, but one is not referenced. Conscise body.
(a, b, c) => a + b;

// single parameter (no parens), concise body, needs to wrap in parens
(txt => alert("FOO: " + txt))("HOWDY!");

// single parameter that's an object binding patter, multiple statements in the body
var del = ({id, message, error}) => { var elem = document.getElementById(id); return message + ": " + elem.className };


function doIt(txt)
{
    // no parameters, block body, but can be reduced to a concise body
    return () => {alert(txt); return txt == ""; };
}

// single object-pattern parameter at the start of the line -- needs to wrap in parens
({foo}) => alert(foo);

// within a setTimeout argument list
var ndx = 0;
setTimeout( () => alert(++ndx), 1000 );

// arrows with rest operator
var onlyRest = (...a) => alert(a.length);
var finalRest = (a, b, ...c) => alert(a + b + c.length);

// arrow with unused rest parameter
var unused = (a, b, ...c) => alert(a + b);

// muladd is a function that takes a multiplier and returns another function
// that is passed two values that are added together and then multiplied by the multiplier.
var muladd = a => (b, c) => a * (b + c);
muladd(5)(2,4); // return 30 [5*(2+4)]

// if the only statement is a return, just concise it
var arr = (a,b,c) => {return a+b+c;};

// this body is not a concise body -- it returns undefined. But it's
// an assignment expression, so don't turn it into a concise body or it
// will return the expression!
var nc = (a, b) => {document.name = "foobar" + b + a;};

