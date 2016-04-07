// we would normally try to combine literal expressions
// but sometimes the operands might cause overflow or underflow
// when done out of order from the execution. If that's the case,
// we don't want to evaluate them.

var smallNum = 2e-200;
var largeNum = 2e+200;

// when evaluated, the result is 8e+300. But if we try to be smart and
// evaluate the the literals, they result in an overflow (Infinity) and
// we'd get different results.
var a = smallNum * 2e+300 * 2e+200;

// same for underflow. If we combine 2/2e300, we would get an underflow zero, which is 
// not necessarily the same as multiplying largeNum by 2e-100 THEN dividing by 2e300.
var b = largeNum * 2e-100 / 2e+300;