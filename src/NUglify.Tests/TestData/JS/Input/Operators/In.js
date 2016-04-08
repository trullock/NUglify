// this instance requires spaces on both sides
var t = function(x, y, z)
{
  return ( x in y );
};

// require no space on the left
t["do"] in t;

// require no space on the right
t in (Array||Object)

// require no spaces
t["do"] in (Array || Object)


