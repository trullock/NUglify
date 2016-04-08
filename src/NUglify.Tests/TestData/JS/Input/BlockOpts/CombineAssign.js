function foo()
{
    // ideally we would want to strip these all out:
    // return"onetwothreefour"
    var a = "one";
    a += "two";
    a += "three";
    a += "four";
    return a;
}

function bar(txt, url)
{
    // ideally we would want to strip all these out and combine them:
    // return'<h1><a href="'+url+'">'+txt+"<\/a><\/h1>"
    var html = "<h1>";
    html += "<a href=\"";
    html += url;
    html += "\">";
    html += txt;
    html += "</a></h1>";
    return html;
}

function bat(ul)
{
    var child, html;
    html = "<h1>";
    for(var ndx = 0; (child = ul.childNodes[ndx]); ++ndx)
    {
        if (child.nodeName == "LI")
        {
            html += "<em>";
            html += child.innerHtml;
            html += "</em>";
            html += ',';
        }
    }
    return html += "[end]" + "</h1>";
}

function ack(bar)
{
    // ideally we'd be able to combine these to:
    // return bar+="foobat"
    bar += "foo";
    return bar = bar += "bat";
}

function gag(bar)
{
    var a = 10;
    return (a = bar) + (a + 42); 
}


