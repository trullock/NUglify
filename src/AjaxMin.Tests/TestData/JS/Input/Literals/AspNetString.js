// the string literal contains an asp.net replacement,
// which itself contains quotes. Because the asp.net replacement will
// get replaced at run-time, we want to skip over everything inside the
// <% and %> delimiters and NOT consider the double-quotes to be the delimiters
// of the JS string. 
alert("<%= Request("FooBar") %>");

// AND because we don't know whether the developer has specific plans for
// escaping quote characters inside their ASP.NET values, let's KEEP the
// delimiters that they use in their sources. Don't replace them with
// a better choice, because that "better" might not be right.
// keep the single-quotes -- don't change to double-quotes.
alert('<%= Request('foo') %>');

// and don't combine multiple adjacent ASP.NET strings if their delimiters are different.
// the developer might be specifically using different delimiters based on the
// content that can be inserted by ASP.NET. For example, they might have strings
// that will include unescaped double-quote and want to specifically use single-quotes
// so the resulting JS doesn't break.
var foo = "<%= Request("foo1") %>" + '<%= Request("bar1") %>';

// if we wanted to get fancy, it'd okay if they ARE the same delimiter - but 
// let's just keep things simple and NOT combine them at all
var bar = "<%= Request("foo2") %>" + "<%= Request("bar2") %>";
