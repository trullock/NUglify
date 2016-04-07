function raf()
{
    return requestAnimationFrame ||webkitRequestAnimationFrame || mozRequestAnimationFrame
        || oRequestAnimationFrame || msRequestAnimationFrame || function (callback)
        {
            if (typeof callback == "function")
            {
                setTimeout(callback, 16.7);
            }
        };
}
