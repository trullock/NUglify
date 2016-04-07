// These two literals should NOT be automatically
// calculated with the == operator because one of them
// contains a \v character. That is the ECMAScript "vertical tab" escape,
// but IE doesn't recognize it in versions before IE9. 
// so because of the cross-browser issues, leave it be.
var IE="\v" == "v";

var ie=!+"\v1";

