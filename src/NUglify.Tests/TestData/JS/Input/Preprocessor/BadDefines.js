
// no identifier
///#DEFINE
foo = 1;

///#UNDEF 
foo = 2;

// more than just the identifier should be ignored
///#DEFINE foobar = 2

///#IFDEF foobar
// make sure the name got defined
alert("foobar is defined");
///#ENDIF

///#UNDEF foobar = 0

