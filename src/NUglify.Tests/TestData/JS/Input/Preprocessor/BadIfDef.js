
// no identifier
///#IFDEF
var one = 1;
///#ELSE
var one = 2;
///#ENDIF

// no identifier on the same line -- the identifier for the ifdef needs
// to be on the same line
///#IFDEF
foobar
alert("hi");
///#ENDIF

// bad identifier
///#IFDEF 123
var two=2;
///#ELSE
var two=3;
///#ENDIF

// extra stuff after the identifier should be ignored
///#IFDEF foo = 3
var foo=3;
///#ELSE
var foo="not 3";
///#ENDIF

// because anything after the identifier is ignored, this can get confusing
///#DEFINE bar = 12
///#IFDEF bar = 3
var bar=3;
///#ELSE
var bar="not 3";
///#ENDIF


