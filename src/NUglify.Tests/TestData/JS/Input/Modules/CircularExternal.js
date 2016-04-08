// import even, which will also import odd, which does an import on even, but the circular
// nature should be just fine because it should know that even has already been imported.

import even from 'lib/even';

alert(even(Number.MAX_VALUE));
