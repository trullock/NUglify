function test(a,b)
{
    // try the different primitives
    typeof(a) === "foo";
    typeof(a) === 123;
    typeof(a) === true;
    typeof(a) === null;

    // try different binary operators
    typeof(a) === (a = "arf");
    typeof(a) === (a,b);

    // these are numeric operators, so this should always be false
    typeof(a) === (a & b);
    typeof(a) === (a &= b);
    typeof(a) === (a | b);
    typeof(a) === (a |= b);
    typeof(a) === (a ^ b);
    typeof(a) === (a ^= b);
    typeof(a) === (a / b);
    typeof(a) === (a /= b);
    typeof(a) === (a << b);
    typeof(a) === (a <<= b);
    typeof(a) === (a - b);
    typeof(a) === (a -= b);
    typeof(a) === (a % b);
    typeof(a) === (a %= b);
    typeof(a) === (a * b);
    typeof(a) === (a *= b);
    typeof(a) === (a >> b);
    typeof(a) === (a >>= b);
    typeof(a) === (a >>> b);
    typeof(a) === (a >>>= b);

    // these are all boolean operators, so they should also be false
    typeof(a) === (a == b);
    typeof(a) === (a > b);
    typeof(a) === (a >= b);
    typeof(a) === (a in b);
    typeof(a) === (a instanceof b);
    typeof(a) === (a < b);
    typeof(a) === (a <= b);
    typeof(a) === (a == b);
    typeof(a) === (a === b);
    typeof(a) === (a !== b);

    // the plus operators might be string or numeric
    typeof(a) === (a + b); // both unknown
    typeof(a) === (a += b); // both unknown
    typeof(a) === (typeof(b) + "foo"); // both string - string
    typeof(a) === (b + "foo"); // right is string - string
    typeof(a) === ("foo" + b); // left is string - string
    typeof(a) === (12 + false); // neither is string (but they are both known) - numeric

    // logical and and logical or return either the first or second operand
    // based on whether they evaluate to true or not
    typeof(a) === (a && b); // unknown
    typeof(a) === (a || b); // unknown
    typeof(a) === ("wow" && 12); // first is non-empty string, return second type (numeric)
    typeof(a) === (12 && "foo"); // first is non-zero number, return second type (string)
    typeof(a) === (false && "foo"); // first is false, return first type (boolean)
    typeof(a) !== ("" || 12); // first is empty string, return second type (numeric)
    typeof(a) === (0 || "foo"); // first is zero number, return second type (string)
    typeof(a) !== ("bar" || true); // first is non-empty string, return first type (string)
}
