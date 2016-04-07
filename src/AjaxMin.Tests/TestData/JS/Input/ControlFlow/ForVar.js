// have a couple var-statements before a for-statement statement that has no initializers at all
// the vars should be combined and moved inside the for-statement
var a;
var i = 0;
for (; i < 10; ++i)
{
}


// a var-statement before a for-statement that has a var-initializer
// the outer var should be combined with the inner var
var a = 10, b;
for (var i = 0; i < 10; ++i)
{
}

// same as before, but multiple statements and multiple declarations
// all should be combined within the for-statement
var a = 10;
var b, c, d = "";
for (var i = 0; i < 10; ++i)
{
}

// same as before, but one variable is declared twice
// should remove the uninitialized one
var a = 10;
var b, i, c, d = "";
for (var i = 0; i < 10; ++i)
{
}

// same as before, but the dup is not in the immediately-preceeding var-statement
// should remove the uninitialized one
var i;
var a = 10, b, c, d = "";
for (var i = 0; i < 10; ++i)
{
}

// same as before, but one variable is declared and initialized twice (first is a constant)
// should combine them all and leave all the initializers
var a = 10;
var b, i = 5, c, d = "";
for (var i = 0; i < 10; ++i)
{
}

// the initializer in the for-statement is an assignment, and the var-statement 
// before is for the same variable and has no initializer
// move the var statement inside the for-initializer
var i;
for (i = 0; i < 10; ++i)
{
}

// same as above, but with other declarations in the preceding var-statement, where the target
// initializer is in the adjacent var-statement
// move the var statements inside the for-initializer and remove the dup
var a;  
var b, i, c;
for (i = 0; i < 10; ++i)
{
}

// same as above, but the target is NOT in the adjacent var-statement
// move the var statements inside the for-initializer and remove the dup
var a;
var i;  
var b;
var c; 
var d;
for (i = 0; i < 10; ++i)
{
}

// var-statement and initializer assignment have different values
// combining gains nothing, but might be easier on the code logic
var i = 5;
for (i = 0; i < 10; ++i)
{
}

// var-statement with no initializer before, assignment initializer within, 
// but also some other stuff with a comma-operator
// DO NOT combine; would break
// (and convert the condition to 0)
var i;
for (i = 10, a = 5; false; --i)
{
}

// same as above, but both variables in the comma operator assignments are
// declared in the preceding var-statement.
// we should move the var statement into the for statement in this
// situation, too.
// (and replace the true condition with nothing)
var i, a;
for (i = 10, a = 5; true; --i)
{
}

