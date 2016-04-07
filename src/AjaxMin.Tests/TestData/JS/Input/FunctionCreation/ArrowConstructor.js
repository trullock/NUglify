// this needs to throw an error
var obj = new ((a,b,c) => {this.sum = a + b + c;})(1,2,3);
