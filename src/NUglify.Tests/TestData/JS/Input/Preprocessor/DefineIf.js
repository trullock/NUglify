

///#UNDEF notdefined
///#DEFINE FooBar

///#IF foobar
var a = "foobar";
///#ELSE
var a = "not foobar";
///#ENDIF

///#undef      foobar

///#if FOOBAR        
var b = "foobar";
///#ELSE
var b = "not foobar";
///#ENDIF

///#if Ackbar ==   Admiral   
var c = "Admiral Ackbar";
///#else
var c = "not Admiral Ackbar";
///#endif

///#if Version == 4.0 
var v = 4.0;
///#else
    ///#if version <= 3.5  
    var v = 3.5;
    ///#else
    var v = "unknown";
    ///#EndIf
///#endif

///#IF meow ==  
function meow()
{
    alert("blank MEOW!");
}
///#Else
    ///#If meow
    function meow()
    {
        alert("not blank MEOW!");
    }
    ///#endif
///#ENDIF

///#if version < 2
alert("version less than 2");
///#endif

///#if version <= 2
alert("version less than or equal to 2");
///#endif

///#if version > 2
alert("version greater than 2");
///#endif

///#if version >= 2
alert("version greater than or equal to 2");
///#endif

///#if version != 2
alert("version not equal to 2");
///#endif

///#if version != 2.0
alert("version not equal to 2.0");
///#endif

///#if version === 2
alert("version EXACTLY equal to 2");
///#endif

///#if version !== 2
alert("version not EXACTLY equal to 2");
///#endif

