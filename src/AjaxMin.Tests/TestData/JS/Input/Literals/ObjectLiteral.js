var o = {
  get ack() { return 42; },
  set ack(v) { alert(v); },
  123 : 4.56e+03,
  789.2 : true,
  "help" : "me",
  "while" : 45.67,
  foo : function() {return "bar";},
  goto : "what?",
  "你好" : "hello"
  };
 
alert(o.goto);

// just to make sure the expression statements before and after don't get combined
while(0);

// start a statement with an object literal needs to 
// be wrapped in parens so it doesn't get parsed as a bad
// statement block
({foo: 42, showMe: function() { document.write("<h1>" + this.foo + "!</h1>") } }).showMe();

function resetTop(elem)
{
    var $elem = $(elem);
    var prev = { marginTop: $elem.css("margin-top") };
    $elem.css({marginTop: "auto"});
    return prev;
}

var es6 = {
        // regular property/value syntax
        myProperty: 42,

        // ES5 getter/setter syntax
        get foo() { return this.foo; },
        set foo(value) { this.foo = value; },

        // ES6 implicit property name from a lookup name
        location,

        // ES6 method (with an unused parameter)
        myMethod(one, two, three) { alert(one + two); },

        // ES6 generator method (with an unsed parameter)
        *myGenerator(array, max) { for(var item of array) yield item; },
    };