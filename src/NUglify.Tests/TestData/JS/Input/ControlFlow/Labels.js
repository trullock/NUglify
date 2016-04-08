function foo()
{
    var line, t = 0;

    lab:
    for(var lab=0; lab < 10; ++lab)
    {
        line = "";
        line : for(var ndx = 0; ndx < lab; ++ndx)
        {
            line = line + ndx + " ";
            continue line;
        }
        if ( lab == 8 )
        {
            break lab;
        }
        else if ( lab == 4 )
        {
            continue lab;
        }
        line : for(var n = 0; n < lab; ++n)
        {
            if ( n == 10 ) { break lab; }
            else if ( n == 7 ) {continue line}
            xline : foogas:while( t < n ) { t++; continue xline; }
            
            if ( n == lab ) continue arf; else break woof;
        }
        document.write("<div>" + line + lab + "</div>");
    }
}
// label at the end of the file
eof:
