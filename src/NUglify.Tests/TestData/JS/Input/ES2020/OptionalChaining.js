let x = foo?.bar;
let y = foo?.();
let z = foo?.()?.[];

// should be treated as a conditional expression
let p = foo?.3 : 0;

return x + y + z;