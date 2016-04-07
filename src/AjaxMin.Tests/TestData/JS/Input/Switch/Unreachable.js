function foo(x)
{
    while (x)
    {
        switch(x)
        {
        case -1: // no statements of its own
        case 0:  // no statements of its own
        case 1:
            ++x;
            return 0;
            // everything after return is unreachable
            x += 2;
            break;

        case 42:
            --x;
            break;
            // everything after break is unreachable
            return 69;
            foo(x + 1);

        case 62:
            x *= -1;
            continue;
            // everything after continue is unreachable
            foo(x+1);
        }

        ++x;
    }
}
