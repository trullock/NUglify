$(document).ready(function () {
	let test = 123;
	if (!window.a) {
		function idle() {
			console.log("test")
		}
	}
	function cancelTest() {
		console.log("test2")
	}
	function why() {
		console.log("test3")
	}

	window.test(why, cancelTest);
});