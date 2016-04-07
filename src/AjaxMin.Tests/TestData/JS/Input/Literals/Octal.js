// octal escapes in strings
// use different lengths, between 0 and 377

// can be up to three, but is only one - ends in delimiter
var threeOne1 = "\0";       // 0

// can be up to three, but is only one - ends in some other character
var threeOne1 = "\0xxx";    // 0

// can be up to three, but is only two, ends in delimiter
var threeTwo1 = "\02";      // 2

// can be up to three, but is only two, ends in some other character
var threeTwo1 = "\02xxx";   // 2

// three digits, ends in delimiter
var threeThree1 = "\123";    // 83 'S'

// three digits, ends in some other character
var threeThree2 = "\123xxx"; // 83 'S'

// can be up to two, but is only 1, ends in delimiter
var twoOne1 = "\6";         // 6

// can be up to two, but is only 1, ends in delimiter
var twoOne2 = "\6xxx";      // 6

// can be up to two and is two, ends in delimiter
var twoTwo = "\46";         // 39 '&'

// can be up to two and is two, ends in some other character
var twoTwo = "\46xxx";      // 39 '&'

// can be up to two but LOOKS like there's three
var twoTwoPlus = "\773";    // 63 '?' followed by a 3

// make sure they don't get combined
var concat = '\123' + '\77'; // would be "S?"