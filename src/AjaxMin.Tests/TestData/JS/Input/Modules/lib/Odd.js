// even is imported as the default export of the module "Even"
import even from "Even";

// define default export as function expression "odd"
export default function odd(n) {
    return n != 0 && even(n - 1);
}
