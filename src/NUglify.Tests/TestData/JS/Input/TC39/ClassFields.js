function x() {
	let longNameToBeMinimised = 1;

	class Shape {
		static id = 1;

		id = 0;

		['computed' + longNameToBeMinimised] = longNameToBeMinimised;

		constructor (id, x, y) {
			this.id = id;
			this.move(x, y);
		}

		async *['computed2' + longNameToBeMinimised] (x, y) {
			this.x = x;
			this.y = y;
			yield 0;
		}

		*[Symbol.iterator]() {
			yield 1;
			yield 2;
			yield 3;
		}
	}
}