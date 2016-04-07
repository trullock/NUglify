/*@cc_on@*/
// the cc_on for this statement doesn't need to be there because we've already turned them on in the previous line 
var a//@cc_on=1

// just make sure this one doesn't get a cc_on added. And the line-terminator should insert a semi-colon
// and not throw an error on the alert statement on the next line
var b//@=2
alert(!!a)
