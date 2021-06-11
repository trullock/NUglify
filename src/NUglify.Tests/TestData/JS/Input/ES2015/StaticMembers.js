// http://es6-features.org/#StaticMembers
class Rectangle extends Shape {
    static defaultRectangle () {
	    return new Rectangle("default", 0, 0, 100, 100);
    }
    // shouldnt conflict with static one
    defaultRectangle(a, b, c, d) {
	    return new Rectangle("default", a, b, c, d);
    }
}