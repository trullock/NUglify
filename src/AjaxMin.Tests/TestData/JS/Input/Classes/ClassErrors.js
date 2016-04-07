//declaration with no identifier
class extends String
{
    construtor(){}
}

// binding patterns aren't allowed
class [foo,bar] extends Array {
    method() {return 42;}
}

// extends should be followed by an expression. If it's not,
// the opening curly-brace of the class body will get parsed 
// as an object literal for the heritage, and then the class will
// have no body!
class foo extends {
    get bar() {return 42;}
};

// leave off the trailing close-curly
class bar {
    *Iter(arr){ for(var i of arr){ yield i; } }
