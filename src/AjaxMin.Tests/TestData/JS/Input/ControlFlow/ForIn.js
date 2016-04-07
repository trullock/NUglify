function Func(p1)
{
    // Create some variables.
    var a, key, s = "";

    // Initialize object.
    a = {"a" : "Athens" , "b" : "Belgrade", "c" : "Cairo"}

    // Iterate the properties.
    for (key in a)   
    {
      s += a[key] + "&ltBR>";
    }
    
    for(var i in [1, 2, 3])
    {
        alert(i);
    }
    
    for(var j in [1, 2, 3])
    {
        for(var k in [1, 2, 3])
        {
            alert(k);
        }    
    }
    
    for(key in s)
    {
      // empty body
    }

    var obj = {};
    for(obj[key] in [1,2,3])
    {
        alert(obj[key]);
    }
}