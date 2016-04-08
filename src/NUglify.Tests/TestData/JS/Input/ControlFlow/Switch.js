function Func(p1)
{
    var ret = 0;
    switch(p1)
    {
        case 1:
            ret = 1;
            break;
        case 2:
            ret = -1;
            // fall through....
        case 3:
        case 4:
            break;
        case 5:
        {
            for(var ndx = 0; ndx < 10; ++ndx)
            {
              ret *= p1;
            }
            break;
        }
        case ret.maxValue:
            ret = -2;
            break;

        default:
            break;
    }

    return ret;
}