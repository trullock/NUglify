// test module that will be imported

export function foo(txt) { alert("foo: " + txt); }

function bar(txt) 
{ 
    alert("bar: " + txt); 
}

export var bat = 42;

export {bar};

