let p = null;
let r = p ?? true;
let s = null ?? 3;
let x = false ?? "string";

function y() {
    return 1 ?? x;
}
let z = y() + s;