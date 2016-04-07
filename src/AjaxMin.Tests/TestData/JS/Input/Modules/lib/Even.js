// odd is imported as the default export of module "Odd"
import odd from 'Odd';

// define default export as function expression "even"
export default function even(n){
    return n == 0 || odd(n - 1);
}

