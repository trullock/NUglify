    var arrayObj1 = new Array();            // crunch to []
    var arrayObj2 = new Array(1);           // single numeric argument is size, not initializer. Don't crunch to literal.
    var arrayObj3 = new Array(1, 2, 3, 4);  // crunch to [1,2,3,4]
    var arrayObj4 = new Array;              // crunch to []
    var arrayObj5 = [["Names", "Beansprout", "Pumpkin", "Max"], ["Ages", 6,, 4]]; // missing array item
    var arrayObj6 = [6, 5, 4];              // crunch to [6,5,4]
    var arrayObj7 = new Array("foo");       // single non-numeric is not size; crunch to ["foo"]
    
    // single argument, but we don't know the type. don't crunch to literal
    var arrayObj8 = new Array(arrayObj7.length);
    var arrayObj9 = new Array(arrayObj1);
    var arrayObj10= new Array(foo());

    // missing items, ending with a single comma - for most browsers we could get rid of the trailing
    // comma, HOWEVER -- some other browsers might [incorrectly] think the trailing comma means a missing
    // value on the end. So leave it as-is -- the developer knows best.
    // throw a cross-browser warning (sev-2) if there are any trailing commas because of this delta.
    var arrayObj11 = [1, , 3, 4,];

    // missing items, ending with TWO commas - do NOT remove the trailing commas!
    // the trailing commas affect the length of the array because we need to keep that
    // missing value intact. This array has 5 elements.
    var arrayObj12 = [1, , 3, 4, ,];

    // Same with this one. This array has 6 elements.
    var arrayObj13 = [1, , 3, 4, ,,     ];

    // make sure the empty array literal is parsed okay
    var arrayObj14 = [
        ];

    // make sure an array literal with a single missing value is parsed correctly
    var arrayObj15 = [,];
