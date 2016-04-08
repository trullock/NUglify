//@cc_on
var o = 
{
    IsIE: function() 
    {
        // Conditional commenting is enabled only for IE.
        var ie = false;
        /*@cc_on
        ie = true;
        @*/
        return ie; 
    }
};