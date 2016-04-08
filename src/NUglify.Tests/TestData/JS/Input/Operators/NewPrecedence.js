function test(oType)
{
    // no parentheses around Array
    var arr = new Array(Array,Function,Boolean);
    // no prentheses around arr[0]
    var foo = new arr[0](1, 2, 3);
    
    // KEEP parentheses around Type.getType(oType.sObjectType)
    return new (Type.getType(oType.sObjectType))(
        oType.sTypeName,
        oType.iVersion,
        oType.sRootElementType,
        oType.sRootElementName);
}

function foo()
{
    return new Date(new Date().ToUTCString()); 
}

// don't need any parentheses added
var a = new new foo;

// don't need any parentheses added
var b = new new foo(a);

// need to KEEP these parens to make sure the (a) stays with the outer new
var c = new (new foo)(a);

// don't need any parentheses added
var d = new new foo(a)(b);

