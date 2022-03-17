let foo, bar;
({ foo, bar } = { foo: 42, bar: true });
// similar but with variable being destructured
({ foo, bar } = fooBar);