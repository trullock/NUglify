// basic ES6 classes

// base class
class Foo
{
    bar = 0;
    baz = 1;

    constructor()
    {
        this.foo = 42;
    }

    get foo() { return this.foo; }
    set foo(v){ this.foo = v; }
}

// child class
class Bar extends Foo
{
    Bat(one, two, three)
    {
        alert(one + two);
    }

    *Iter()
    {
        if (this.bat)
        {
            yield this.bat;
        }

        yield super.foo;
    }
}

