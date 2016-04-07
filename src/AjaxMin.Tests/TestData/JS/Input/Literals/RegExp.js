function test1(text)
{
    // this regular expression do NOT have the global flag (g) on it,
    // so we can treat it like a constant and get rid of the variable
    // middle-man.
    var re = /the/;
    return re.exec(text);
}

function test2(text)
{
    // this regular expression DOES have the global flag (g) on it,
    // so we CANNOT treat it like a constant and get rid of the variable
    // middle-man. If we were to replace the variable in the while condition
    // with the literal, it would cause an infinite loop because the exec()
    // method would restart from the beginning each and every time.
    var re = /the/g;
    var count = 0;
    var match;
    while((match = re.exec(text)))
    {
        ++count;
    }
    return count;
}

var foo = location.href.replace(/[\s?!@#$%^&*()_=+,.<>'":;\[\]/|]/g, '-');

// the regular expression doesn't have a closing semicolon, so
// it should end at the end of the line and not keep parsing.
var re = /\w+/
alert("hi")
