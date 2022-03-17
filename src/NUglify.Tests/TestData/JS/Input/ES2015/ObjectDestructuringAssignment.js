// http://es6-features.org/#ObjectMatchingShorthandNotation
let { op, lhs, rhs } = getASTNode();
let { foo, bar } = fooBar;

// similar but assigning without declaring variables (parenthesis are required in these cases)
({ op, lhs, rhs } = getASTNode());
({ foo, bar } = fooBar);