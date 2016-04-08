// a couple adjacent const statements should get combined
const a = 1;
const b = 2;
const c = 3;

// put a couple var-statements -- they should get combined, but not with the consts
var d = 4;
var e = 5;

// couple more that should get combined together but not with the var-statements
const f = 6;
const g = 7;
