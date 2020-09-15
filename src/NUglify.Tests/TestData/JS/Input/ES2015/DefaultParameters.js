// http://es6-features.org/#DefaultParameterValues
function f (x, y = 7, z = 42) {
    return x + y + z;
}

f(1) === 50;
let f2 = (x = 1) => x^2;