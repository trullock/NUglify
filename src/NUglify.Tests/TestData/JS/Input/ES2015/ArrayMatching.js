// http://es6-features.org/#ArrayMatching
var list = [ 1, 2, 3 ]
var [ a, , b ] = list;
[ b, a ] = [ a, b ]