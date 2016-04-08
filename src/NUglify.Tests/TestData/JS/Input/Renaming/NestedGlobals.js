(function(){

// this is a global reference
alert(g);
function foo()
{
    var aa, ab, ac, ad, ae, af, ag, ah, ai, aj, ak = location.href, al, am, an, ao, ap, aq, ar, as, at, au, av, aw, ax, ay, az;
    function bar(a)
    {
        // this is a reference to the same global an outer function scope references
        if (g(a)) bar(a-1);
    }

    bar(10);
}

foo();
})();
