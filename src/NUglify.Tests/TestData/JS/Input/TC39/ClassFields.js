class Shape {
    id = 0;

    constructor (id, x, y) {
	    this.id = id;
	    this.move(x, y);
    }

    move (x, y) {
	    this.x = x;
	    this.y = y;
    }
}