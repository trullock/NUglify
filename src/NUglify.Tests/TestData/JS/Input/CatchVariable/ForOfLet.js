function go(scope) {
    for (let test in scope.prop) {
        console.log(test);
    }
}

go({ prop: [1, 2, 3] });