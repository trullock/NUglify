// class expressions

// base class, no name
var base = class{ construtor(v) { this.arf = v; } };

// derived class, no name
var derv = class extends base { 
    constructor(v) { super(v); } 
    get Arf() { return this.arf; } 
};

// derived class, unreferenced name
var noref = class Bar extends derv { constructor(g) { super(g*g); } };

// derived class, referenced name
var self = class Foo extends derv 
    {
        constructor(a, b) { super(a+b); } 
        static Bat(a) { alert( this.Arf + a ); return a; };
        DoIt(far) { return Foo.Bar(far); }
    };
