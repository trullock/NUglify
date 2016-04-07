var a, b, c, d;

// block-level shortcuts - don't transform
a && b();
a || b();

// block-level shortcuts - transform
!a && b();
!a || b();

// comma-level shortcuts - don't transform
a, b && c();
a, b || c();

// comma-level shortcuts - transform
a, !b || c();
a, !b && c();

// not a statement -- don't transform
if (!a && b())
{
    c();
}
if (!a || b())
{
    c();
}

// comma, separated and not a statement -- don't transform
if (d, !a && b())
{
    c();
}
if (d, !a || b())
{
    c();
}
