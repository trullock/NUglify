const object = {
    doSomething: (test) => {
        return "done" + test;
    },
    doSomethingAsync: async (url) => {
        return await fetch(url);
    }
}

object.doSomethingAsync();