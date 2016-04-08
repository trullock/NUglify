// the ZWNJ and ZWJ characters are valid identifier parts
// they are zero-width, so it will LOOK like there's nothing there in the output
// when the encoding is UTF-8, but they are there. The ASCII encoding will
// have the proper \uXXXX escaping.
var p\u200c\u200doodoo = 42;


var \while = 10; // escaping the identifier seems to make it an ident token and not a statement token
var \for = function() {};

for(var i = 0; i < 10; ++i)
{
  \for(\while);
}

while( \while < 200 )
{
  \while += 90;
}

// funky unicode names - globals won't get renamed
var ಠ_ಠ = "mad eyes";
var ლ_ಠ益ಠ_ლ = "Y U NO WORK?!";

// module should not be used, should throw a warning, but should not error
var module = "my module";

// the unicode-escaped identifier should still match up with the non-escaped identifier
function 〱〱(Ⅳ, \u0422\u0410\u041a\u0421\u0418)
{
    // the global won't get renamed, but the locals would
    alert(\u0ca0_\u0ca0 + \u2163 * ТАКСИ);

    // two variables that are NOT the same name:
    var ma\u00F1ana = Ⅳ++;
    var man\u0303ana = ТАКСИ * ТАКСИ;
    alert(ma\u00F1ana == man\u0303ana );
}

