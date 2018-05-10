// http://es6-features.org/#ObjectMatchingShorthandNotation
var { op, lhs, rhs } = getASTNode()
// http://es6-features.org/#ObjectMatchingDeepMatching
var { op: a, lhs: { op: b }, rhs: c } = getASTNode()
// http://es6-features.org/#ObjectAndArrayMatchingDefaultValues
var obj = { a: 1 }
var list = [ 1 ]
var { a, b = 2 } = obj
var [ x, y = 2 ] = list