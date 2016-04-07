(function(window, document, undefined)
{
    // start of our closure
    

;var foo;
var bar = 153;

function myFunc(param)
{
    // alert some expression designed to reference variables
    alert(param + foo + bar + 42);

    // arf is defined elsewhere
    arf(param, foo+bar, bar - foo);
}

// export
window.myFunc = myFunc;

function arf(one, two, three, four)
{
    // convert an undefined value to the string representation
    if (one == undefined)
    {
        one = "undefined";
    }

    
    two += four;
    

    foobar(one, two, three);
    alert("called foobar");
    document.location = "#arf";
}
;    // end of our closure
})(window, document)
// a single-line comment that ends in the EOF
;(function($)
{
    // say hi when loaded
    $(function(){alert("hi!")})
})(jQuery)