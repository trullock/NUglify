// in this case we parens shouldn't be removed
hello(([ t ]) => t);
hello(({ t }) => t);

// only in this case it's allowed to remove parens
hello((t) => t);