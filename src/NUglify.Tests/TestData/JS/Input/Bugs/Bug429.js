const test = await import('lib/math');
console.log(test.pi);

if (import.meta !== undefined) {
    console.log(`imported url ${import.meta.url}`);
    console.log(`resolved path ${import.meta.resolve(test.pi)}`);
}