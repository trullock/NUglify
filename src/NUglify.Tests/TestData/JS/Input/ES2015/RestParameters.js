// http://es6-features.org/#RestParameter
function f (x, y, ...a) {
    return (x + y) * a.length
}
f(1, 2, "hello", true, 7) === 9

let y = ({ a, b, ...rest }) => a + b + rest.length;
y(1,2,3,4);