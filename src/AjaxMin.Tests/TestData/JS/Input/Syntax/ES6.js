function test1()
{
    // interating over an Array
    for (var word of ["one", "two", "three"]) {
        alert(word);
    }

    // iterating over a Set 
    var s = Set([1, 3, 4, 2, 3, 2, 17, 17, 1, 17]);
    for (var v of s) {
        alert(v);
    }
 
    // Iterating over a Map produces key-value pairs: arrays of length 2.
    var m = new Map;
    m.set("one", 1);
    m.set("two", 2);

    // TODO: need to update var statement to allow for the deconstruction syntax
    // at which point we can move the var into the for
    for (var [name, value] of m) {
        alert(name + " = " + value);
    }

    // object pattern binding using implicit property names
    var {href, protocol, hostname, pathname} = location;
}
