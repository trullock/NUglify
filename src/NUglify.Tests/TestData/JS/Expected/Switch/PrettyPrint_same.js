function foo() {
    var a = 1,
        b = 2,
        c = "three",
        d;
    try {
        d = a * b;
        while (d > 0) {
            switch (c) {
                case"three":
                    c = 10;
                    break;
                case 0:
                    c = -1;
                    break;
                default:
                    c = c * 2;
                    break
            }
            if (!c) {
                debugger;
                c = 1
            }
            else {
                d = d * c;
                a *= d
            }
        }
    }
    catch(e) {
        for (var f = 0; f < b; ++f) {
            a = a * d
        }
    }
    finally {
        if (!a) {
            b = -1
        }
        else
            b = a
    }
}
(function() {
    try {
        var a = {}
    }
    catch(ex) {}
})()
