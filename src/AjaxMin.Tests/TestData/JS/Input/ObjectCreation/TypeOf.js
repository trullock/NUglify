var x = Math.PI;
var y = "Hello";
var z = [10];
var t = typeof(y = "foo"); // use a lower-precedence operator inside the typeof to force the parens

document.write("The type of x (a double) is " + typeof x  );
document.write("The type of y (a String) is " + typeof(y) );
document.write("The type of z (an int[]) is " + typeof(z) );