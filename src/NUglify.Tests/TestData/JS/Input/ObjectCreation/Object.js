function Func(p1)
{
    var obj = new Object(1);    // argument causes not to crunch to object literal
    var foo = new Object();     // no argument; crunch to {}
    var bar = new Object({      // one argument that is an object literal.
        "bar": 42,              // just replace the constructor with the argument
        "while": 16,            // (the constructor would just pass it through as-is anyway)
        1     :      "bar"
    });  
    
    var ack = {
      "bar" : "bar",            // string name; not identifier - will remove quotes
      "while" : "bar",          // string name; identifier, so quotes will always remain
      ack : "bar",              // identifier literal name, so won't be quoted
      42 : 16,                  // integer name
      get foo() {return 1},     // mozilla getter
      set foo(x) {this.ack=x;}  // mozilla setter
    };
    
    // force another reference to obj so hypercrunch will crunch obj to "b"
    // (most-often referenced variables get the lower names, and the generated
    //  variable pointing to "bar" will be referenced the most, so it will be "a"
    //  and the obj value will be the next-referenced and get "b")
    obj.foo = "bar";

    // this is NOT mozilla getter/setter. It should still parse properly.
    foo.b = 
    {
      get : 42,
      set:  "x"
    };

    // end the list with a comma. We should remove it because it doesn't add anything to the object
    bar.c = {
        one: 1,
        two: 2,
    };

    // make sure empty object literal is parsed properly
    ack.d = {};

    return p1(foo,bar,ack);
}