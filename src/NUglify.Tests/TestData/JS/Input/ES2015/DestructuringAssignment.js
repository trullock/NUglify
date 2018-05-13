// http://es6-features.org/#FailSoftDestructuring
var list = [ 7, 42 ]
var [ a = 1, b = 2, c = 3, d ] = list
a === 7
b === 42
c === 3
d === undefined