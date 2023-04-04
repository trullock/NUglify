class HttpClient {
    delete(url, options) {
        return this.send({
            ...options,
            method: "DELETE",
            url,
        });
    }
}