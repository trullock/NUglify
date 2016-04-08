//! start my implicit module

function sum(a, b)
{
    return a + b;
}

export { sum };

export function mul( a, b )
{
    return a * b;
}

function square( a )
{
    return mul(a, a);
}

export {square};

export const pi = 3.1415927;
export const negOne = -1;
export var accumulator = 0;
export var foo = "bar";

function pow(a, n)
{
    if (n == 0) { return 1; }
    var pow = 1;
    while(--n)
    {
        pow = mul(pow, a);
    }
    return pow;
}

export { pow };

//! end my implicit module
