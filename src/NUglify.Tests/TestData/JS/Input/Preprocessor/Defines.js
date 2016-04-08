

///#UNDEF notdefined
///#DEFINE FooBar = BatMan

///#IFDEF foobar
///#if foobar === batman
var a = "foobar is batman";
///#else
var a = "foobar not batman";
///#endif
///#ELSE
var a = "not foobar";
///#ENDIF

///#undef      foobar

///#IFDEF FOOBAR
var b = "foobar";
///#ELSE
var b = "not foobar";
///#ENDIF

///#ifdef ackbar    some other crap we'll ignore
var c = "ackbar";
///#else
var c = "not ackbar";
///#endif

///#IFDEF meow
function meow()
{
    alert("MEOW!");
}
///#ENDIF

