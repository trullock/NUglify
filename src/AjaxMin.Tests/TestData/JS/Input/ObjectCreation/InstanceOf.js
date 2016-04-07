function Func(obj)
{
    var i, t, s = "";   // Create variables.
    t = new Array();   // Create an array.
    t["Date"] = Date;   // Populate the array.
    t["Object"] = Object;
    t["Array"] = Array;
    for (i in t)
    {
     if (obj instanceof t[i])   // Check class of obj.
     {
        s += "obj is an instance of " + i + "\n";
     }
     else 
     {
        s += "obj is not an instance of " + i + "\n";
     }
    }
    return(s);
}

