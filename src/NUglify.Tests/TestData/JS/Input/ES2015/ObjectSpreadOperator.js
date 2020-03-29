const obj1 = {
	prop0: "val0",
	prop1: "val1"
};
const obj2 = {
	...obj1,
	prop2: "val2"
};

const obj3 = {
	...obj1, 
	...obj2
};