let a = true;
let b = (a ?? false) || true;
let c = (a ?? false) && true;
let e = true || (a ?? false);
let f = true && (a ?? false);