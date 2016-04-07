//@cc_on

//@ var singleLine = 42; // space after the @ sign means preprocessor treats as normal line

/*@ //space after the @ sign means preprocessor treats as normal code content
 var multiLine = 16;
 @*/

//@set @fourteen = 14
var fourteen = /*@fourteen @*/;

//@set @foobar = (!@fourteen)
var a = /*@foobar @*/;

//@set @foobar = (~@fourteen)
var b = /*@foobar @*/;

//@set @foobar = (@fourteen * 10)
var c = /*@foobar @*/;

//@set @foobar = (@fourteen / 10)
var d = /*@foobar @*/;

//@set @foobar = (@fourteen % 10)
var e = /*@foobar @*/;

//@set @foobar = (@fourteen + 10)
var f = /*@foobar @*/;

//@set @foobar = (@fourteen - 10)
var g = /*@foobar @*/;

//@set @foobar = (@fourteen << 2)
var h = /*@foobar @*/;

//@set @foobar = (@fourteen >> 0x0002)
var i = /*@foobar @*/;

//@set @foobar = (@fourteen >>> 2)
var j = /*@foobar @*/;

//@set @foobar = (@fourteen < 14)
var k = /*@foobar @*/;

//@set @foobar = (@fourteen <= 14)
var l = /*@foobar @*/;

//@set @foobar = (@fourteen > 14)
var m = /*@foobar @*/;

//@set @foobar = (@fourteen >= 14)
var n = /*@foobar @*/;

//@set @foobar = (@fourteen == 14)
var o = /*@foobar @*/;

//@set @foobar = (@fourteen != 14)
var p = /*@foobar @*/;

//@set @foobar = (@fourteen === 14)
var q = /*@foobar @*/;

//@set @foobar = (@fourteen !== 14)
var r = /*@foobar @*/;

//@set @foobar = (@fourteen && 10)
var s = /*@foobar @*/;

//@set @foobar = (@fourteen || 10)
var t = /*@foobar @*/;

//@set @foobar = (@fourteen & 0xf)
var u = /*@foobar @*/;

//@set @foobar = (@fourteen ^ 0xf)
var v = /*@foobar @*/;

//@set @foobar = (@fourteen | 0xF)
var w = /*@foobar @*/;

//@set @foobar = (14)
var x = /*@foobar @*/;

//@set @foobar = (+foo)
var y = /*@foobar @*/;

//@set @foobar = (-14)
var z = /*@foobar @*/;

//@set @foobar = (true)
var tr = /*@foobar @*/;

//@set @foobar = (false)
var fa = /*@foobar @*/;

// shortcuts
//@set @foobar = (@fourteen || 0)
var s0 = /*@foobar @*/;

//@set @foobar = (@fourteen || 1)
var s1 = /*@foobar @*/;

//@set @foobar = (@fourteen && 1)
var s2 = /*@foobar @*/;

//@set @foobar = (@fourteen && 0)
var s3 = /*@foobar @*/;
