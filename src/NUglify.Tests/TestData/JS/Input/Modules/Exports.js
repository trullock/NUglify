// export examples that should have no errors

// re-export from outside module
export * from "crypto";

// foo and bar should NOT defined in our scope - they are just passed through the exports
export { foo, bar } from "mylib";

export default function(){ alert("wow!"); };

export const RHO = 1.32471795724474602596;

export function foobar(a, b, c)
    {
        if (a < b)
        {
            return c;
        }

        return c + a - b;
    }

