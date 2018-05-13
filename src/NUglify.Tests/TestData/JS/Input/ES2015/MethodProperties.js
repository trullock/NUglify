// http://es6-features.org/#MethodProperties
obj = {
    foo(a, b) {
        return a + b;
    },
    bar(x, y) {
        return x + y;
    },
    *quux(x, y) {
        return x - y;
    }
}