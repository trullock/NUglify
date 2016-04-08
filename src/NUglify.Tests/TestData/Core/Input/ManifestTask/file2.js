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