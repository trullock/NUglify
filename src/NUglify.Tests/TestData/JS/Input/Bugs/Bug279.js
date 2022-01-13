"use strict";
const config = {
	...Manipulator.getDataAttributes(target),
	...Manipulator.getDataAttributes(this)
};