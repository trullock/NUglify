// foo should be defined and bar should not
///#define foo
///#undef bar

// this should only output the "not bar" alert 
///#ifdef bar
    alert("bar1");
    ///#ifdef foo
    alert("foo");
    ///#endif
    alert("bar2");
///#else
    alert("not bar");
///#endif

// and this should only output the "foo" alert 
///#ifdef foo
    alert("foo");
///#else
    alert("not foo1");
    ///#ifdef bar
    alert("bar");
    ///#else
    alert("not bar");
    ///#endif
    alert("not foo2");
///#endif


///#ifndef bar
    alert("not bar1");
    ///#ifdef foo
    alert("foo");
    ///#else
    alert("not foo");
    ///#endif
    alert("not bar2");
///#else
    alert("bar");
///#endif
