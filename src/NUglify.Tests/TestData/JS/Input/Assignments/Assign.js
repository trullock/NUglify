function Func(p1)
{
    var _a = 1+p1;   // start with an underscore
    var $b = 2;   // start with a dollar-sign (why?)
    var C = 3;    // starts with capital letter
    var d = 4;    // starts with lower-case letter
    var 你好 = 5; // starts with UNICODE character
    
    \u4f60\u597d += 18; // use the escaped name (should come out the same as the non-escaped above)

    // we can replace C with 3 and d with, but we can't evaluate them together to be 7.
    // if p1 is a string, then a is a string, and if a is a string, a+$b*$b is a string.
    // and string + 3 + 4 is NOT the same as string + 7!
    return a + $b*$b + C + d;
}