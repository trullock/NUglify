if (location.hash) //! important, but ignore in this position
{
    throw "hash exists";
}
else //! ignore this one, too!
{
    alert("no hash");
}

for(var p in window) /*! ignore this one, too! */
{
    alert(p /*! inside an expression -- ignore! */ + " = " + window[p]);
}

while(true) //! ignore!
{
    if (window.timer)
    {
        break;
    }
}

do //! don't stick around
{
    //! keep one
    if (/*! start of an expression - ignore */+new Date - 1000000)
    {
        //! keep two
        break;
    }
    /*! keep three */
} //! don't keep this one
while(true) /*! ignore? */;

foo: /*! ignore this one too */
for(var i = 0; i < 100; ++i)
{
    if (i % 3) break foo;
}