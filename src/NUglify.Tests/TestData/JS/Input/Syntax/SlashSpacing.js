var str = "abcd34/hijk";

// must have a space between the divide-operator and the regular expression
console.log(64 / /(\d+)/.exec(str)[1]);

// must NOT add one before the closing terminator of the regular expression
console.log(/(\w+)\//.exec(str));

RegExp.prototype.toString = function(){return "16"};
// no space needed between the RE and the operator, either.
console.log(/abc//4);