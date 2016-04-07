
function conditionals(left, right)
{
  var t = left == right ? 1 : 0; // doesn't need any parentheses
  var u = 5 + (left == right ? 5 : -5); // needs to be wrapped entirely in parens
  var v = (left, right) ? 10 : 11; // needs parens around the condition
  
  // even though the assign is a lower precedence than the conditional operator,
  // we don't need parens in these examples because the conditional operator's types
  // for the true and false branches are AssignOperator
  var a = t ? u = left : right;
  var b = t ? (u = left) : right;
  var c = t ? left : u = right;
  var d = t ? left : (u = right);
  
  if (left == right) return 1;
  if (left != right) return 2;
  if (left === right) return 3;
  if (left !== right) return 4;
  if (left > right) return 5;
  if (left < right) return 6;
  if (left >= right) return 7;
  if (left <= right) return 8;
}
