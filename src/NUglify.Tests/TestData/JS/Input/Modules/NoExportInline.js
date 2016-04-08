module "foo"
{
    export function bar(txt) { alert("bar: " + txt); }
    export function bat(txt) { alert("bat: " + txt); }
}


import { arf } from "foo";
arf("one more time");