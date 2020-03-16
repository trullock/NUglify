async function getNumberAfterTimeout(value) {
    return new Promise((resolve) => {
        window.setTimeout(() => resolve(value), 2000);
    });
}

async function sum(x) {
    var a = getNumberAfterTimeout(20);
    var b = getNumberAfterTimeout(30);
    return x + await a + await b;
}

(async () => { return x + await a(t) + await b; })()

// keep grouping
(async () => { return x + (await a(t) + await b); })()