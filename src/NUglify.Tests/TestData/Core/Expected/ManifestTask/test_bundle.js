///#source 1 1 TESTRUNPATH\Out\Dll\Input\ManifestTask\file1.js
(function(window, document, undefined)
{
    // start of our closure
    ///#IF DEBUG
    if (window.console)
    {
        console.log("entering closure");
    }
    ///#END

;///#source 1 1 TESTRUNPATH\Out\Dll\Input\ManifestTask\file2.js
var foo;
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
///#source 1 1 TESTRUNPATH\Out\Dll\Input\ManifestTask\file3.js

function arf(one, two, three, four)
{
    // convert an undefined value to the string representation
    if (one == undefined)
    {
        one = "undefined";
    }

    ///#if porkpie < 69
    two += four;
    ///#endif

    foobar(one, two, three);
    alert("called foobar");
    document.location = "#arf";
}
;///#source 1 1 TESTRUNPATH\Out\Dll\Input\ManifestTask\file4.js
    // end of our closure
})(window, document)
// a single-line comment that ends in the EOF
;///#source 1 1 TESTRUNPATH\Out\Dll\Input\ManifestTask\file5.js
(function($)
{
    // say hi when loaded
    $(function(){alert("hi!")})
})(jQuery)