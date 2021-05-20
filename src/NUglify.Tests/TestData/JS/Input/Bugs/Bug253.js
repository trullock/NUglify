function testInInline() {
    for (const i in ["foo"]) {
        console.log(i);
    };
}
function testOfInline() {
    for (const i of ["foo"]) {
        console.log(i);
    };
}
function testIn() {
    let i;
    for (i in ["foo"]) {
        console.log(i);
    };
}
function testOf() {
    let i;
    for (i of ["foo"]) {
        console.log(i);
    };
}