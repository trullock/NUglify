
// this is a common piece of code that returns true if the object has
// any enumerable properties, and false if it doesn't. But the big deal here
// is that the variable defined within the for-in statement is not referenced,
// but we need to make sure we don't remove it, otherwise we'll generate invalid
// code.
function AnyProps(obj)
{
    for(var p in obj)
    {
        return true;
    }

    return false;
}