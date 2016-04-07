// a mix of opening braces on their own new lines and at the
// end of the previous line.

// function parses the brace itself, even through the body is a block
function foo(bar){
    // if-statements use a block
    if (bar)
    {
        var ext = bar;
        
        // for-statements use a block
        for (var ndx = 0; ndx < 10; ++ndx) {
            var suffix;

            // switch-statements don't use a block at all
            switch (ndx)
            {
                case 2: suffix = "x"; break;
                case 4: suffix = "4"; break;
                case 6: suffix = "6"; break;
                default: suffix = "-"; break;
            }

            ext += suffix;
        }

        return ext;
    }
}

