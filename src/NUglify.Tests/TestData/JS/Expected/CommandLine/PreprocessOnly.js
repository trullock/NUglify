/*! This is an important comment
    with all sorts of important information
    like license stuff and other things
    that I want left in.
 */

 // define a closure that takes the window and the document
 // object so we can have crunched variables referencing them.
 (function(window, document)
 {
            // over-indented
            // sometimes IE fails when the dev toolbar isn't open, so cache this
            var console = window.console || {log:function(){}};

            // do some pre-processing!
            
            console.log ("DEBUG MODE: output something fun");
            


            // alert the location, in case the user forgot
            window.alert(document.location.href);

        // do something meaningless that I can write a comment for.
        switch(window.foo)
        {
            case            1:
                // just a regular comment
                break;

            case            2:
                break;

            case            3:
                console.log("Do something");

            default:
                break;
        }


        // do some division and some regular expressions, and a little of both
        var re = /foobar/gi;
        var number = /(\d+)\//.exec(window.foo)[1];
        var piOver2 = 22 / 7 / 2;

        RegExp.prototype.toString = function(){ return "16"; };
        alert(/http[s]?\:\/\///4); // actually alerts "4"!

 })(window, document);



 /*! a few blank lines here */


