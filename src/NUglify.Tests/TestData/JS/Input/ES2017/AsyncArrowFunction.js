const object = {
    regularFunction: (test) => {
        return "done" + test;
    },
    asyncArrowFunctionWithoutParameters: async () => {
        return await fetch("someurl");
    },
    asyncArrowFunctionWithParameters: async (url) => {
        return await fetch(url);
    }
}

object.doSomethingAsync();