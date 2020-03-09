class MyClass {
    async getDataFromAjax(url) {
        let data;
        try {
            data = await fetch(url);
            // This will wait 
            // until fetch returns
            fillClientStateWithData(data.json());
        } catch (error) {
            // This will execute if the
            // API returns an error
            handleAjaxError(error);
        }
    }
}

new MyClass().getDataFromAjax();