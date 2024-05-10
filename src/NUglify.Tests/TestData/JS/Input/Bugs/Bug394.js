const someFunc = ({ val, op = 'eq' }) => {
    var useOp = op + val;
};

var args = {
    val: 'va',
    op: 'contains'
};

someFunc(args);