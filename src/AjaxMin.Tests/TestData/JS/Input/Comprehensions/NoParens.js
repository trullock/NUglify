// don't use the parens around the clauses. It should still parse correctly,
// throwing errors, but outputting the proper comprehension syntax.

var arr = [for x in [1, 2, 3] for y in [1, 2, 3] x + y];
var gen = (for x in [1, 2, 3] for y in [1, 2, 3] x + y);
