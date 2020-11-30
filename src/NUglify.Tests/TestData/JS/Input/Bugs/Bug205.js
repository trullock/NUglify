
function createIterableMethod() {
	return {
		amethod() {
			return this;
		},
		[a](b) {
			return this;
		},
		async [c](d) {
			return this;
		}
	};
}