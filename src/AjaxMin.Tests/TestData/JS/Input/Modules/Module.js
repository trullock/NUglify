// module examples should compile fine

// external module gets bound such to the local variable such that all exports are
// properties on that local variable.
module crypto from "crypto";
crypto.encrypt("encrypt me");

module "foo"
{
    export let x = 42;
}

module "bar"
{
    export default function() { console.log("hello!"); }
}

module "math" {
    export function sum(x, y) { return x + y; }
    export var pi = 3.1415927;
}

// myMath should be defined as the entire module
module myMath from "math";
myMath.sum(myMath.pi, myMath.pi);

// same for myJson
module myJson from "http://json.org/modules/json2.js";
alert(myJson.stringify({la:"teda"}));

module "Even" {
    // odd is imported as the default export of module "Odd"
    import odd from 'Odd';

    // define default export as function expression "even"
    export default function even(n){
        return n == 0 || odd(n - 1);
    }
}

module "Odd" {
    // even is imported as the default export of the module "Even"
    import even from "Even";

    // define default export as function expression "odd"
    export default function odd(n) {
        return n != 0 && even(n - 1);
    }
}



