function test1()
{
    var arr = [1,2,3,4,5,6,7,8,9,10];
    var a = [ for( x of arr) if (x % 2) x*x ];
    var b = [ for(i of arr) if(i != 2) for(j of arr) if (j != 2) if (i != j) [i, j] ];
    var moz = [ x * x for(x in arr) if (x % 2)];

    return a.concat(b, moz);
}
