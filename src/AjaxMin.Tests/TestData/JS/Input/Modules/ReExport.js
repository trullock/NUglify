
// this statement should export foo and bar from othermodule, but it shouldn't
// affect THIS scope.
export {foo, bar} from 'othermodule';

// so calling foo and passing bar should result in two undefined global values.
foo(bar);
