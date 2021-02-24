function t() {
	// http://es6-features.org/#SpreadOperator
	var params = ["hello", true, 7]
	var other = [1, 2, ...params] // [ 1, 2, "hello", true, 7 ]

	function f(x, y, ...a) {
		return (x + y) * a.length
	}

	f(1, 2, ...params) === 9

	var str = "foo"
	var chars = [...str] // [ "f", "o", "o" ]
}