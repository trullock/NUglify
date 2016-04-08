// here are a couple escaped string that should NOT throw an error when
// the inline-safe-error flag is set
var one = "<" + "/script>";
var two = "]" + "]" + ">";

// here are two that should
var three = "</" + "script>";
var four = "]]>";
