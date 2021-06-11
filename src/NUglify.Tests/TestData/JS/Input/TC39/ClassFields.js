class Shape {
    static id = 1;
    id = 0;

    static name;
    name;
    age = 21;

    constructor (id, x, y) {
	    this.id = id;
	    this.move(x, y);
    }

    move (x, y) {
	    this.x = x;
	    this.y = y;
    }
}