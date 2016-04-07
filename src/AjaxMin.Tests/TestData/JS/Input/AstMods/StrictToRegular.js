function test()
{
    // the types are known and the same -- reduce the strictness
    var a = "foo" === "bar",    // false
        b = "foo" !== "bar",    // true
        c = typeof test === "function",
        d = typeof test !== "undefined",

    // these types are known and NOT the same -- reduce to boolean
        e = "foo" === 10,       // false
        f = "bar" !== true,     // true
        g = 10 === typeof test, // false
        h = 0 !== typeof test;  // true
}
