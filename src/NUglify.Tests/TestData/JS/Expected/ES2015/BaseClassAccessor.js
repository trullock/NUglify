﻿class Shape{toString(){return`Shape(${this.id})`}}class Rectangle extends Shape{constructor(id,x,y){super(id,x,y)}toString(){return"Rectangle > "+super.toString()}}class Circle extends Shape{constructor(id,x,y){super(id,x,y)}toString(){return"Circle > "+super.toString()}}