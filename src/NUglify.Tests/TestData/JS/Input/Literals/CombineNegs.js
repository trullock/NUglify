// make sure the negated constants stay negated!
(function()
{
    function Rect(top, right, bottom, left)
    {
        this.top = top;
        this.right = right;
        this.bottom = bottom;
        this.left = left;
    }

    // the negative is NOT the first instance of this literal processed
    var rect1 = new Rect(12345, 12345, -12345, -12345);
})();

// do it again, but with a different combination of negatives
(function()
{
    function Rect(top, right, bottom, left)
    {
        this.top = top;
        this.right = right;
        this.bottom = bottom;
        this.left = left;
    }

    // the negative IS the first instance of this literal processed
    var rect1 = new Rect(-12345, -12345, 12345, 12345);
})();


// do it again, but with a different combination of negatives
(function()
{
    function Rect(top, right, bottom, left)
    {
        this.top = top;
        this.right = right;
        this.bottom = bottom;
        this.left = left;
    }

    // the negative is NOT the first instance of this literal processed,
    // BUT there are more negatives than positives, so the combined literal
    // should be a negative.
    var rect1 = new Rect(12345, -12345, -12345, -12345);
})();
