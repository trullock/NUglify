// ES6 template literals

// no substitutions
var nosub = `now is the time for all good men`;
var nosubFunc = safehtml`<a href="#">h\x65ad</a>`;

var multiline = `now is the time \
    for all good men
    to come to the aid
    of their country.`;

// substitutions
var total = 34.99;
var msg = `Total = ${total}, plus tax is ${ total * 1.09800 }`;

// with escapes
var esc = `\u0ca0_\u{ca0}`;

// object literal inside an expression
var obj = `Via ${ {http: "http", https: "SSL", file: "file system"}[location.protocol] }!`;

// nested
var nested = `one two ${ `double my total is ${ 2 * total }` } three`;
