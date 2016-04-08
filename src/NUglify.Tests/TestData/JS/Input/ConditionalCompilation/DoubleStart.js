function foo() {
/* for Internet Explorer */
/*@cc_on@*/
/*@if (@_win32)
addWindowOnload = false;

// this statement doesn't need the /* at the front -- we're already INSIDE a conditional comment
// but don't throw an error, and don't preserve it in the output.
/*@end@*/ 
}
