function foo() {
    var x;
    // Block is explicitly terminated by semi-colon. The semi-colon should be output
    // when compressing.
    <%= test %>; 
    return bar(x);
}

function bar() {
    var x;
    // Block is implicitly terminated by output. No semi-colon should be output when compressing.
    <%= test %>
    return x;
}