function Func(p1)
{
    var index;
    var name = "Thomas Jefferson";
    var answer = 42, counter, numpages = 10;
    var 你好 = "hello"; // valid characters for a JS identifier, but should get escaped to \uXXXX when ascii output mode
    
    // the comma operator will still need parens to keep it from appearing like answer is also being var'd
    var x = index * (name,answer), y; 
}