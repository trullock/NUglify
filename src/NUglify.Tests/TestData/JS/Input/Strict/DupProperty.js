"use strict"

function test1()
{
    // can't have same name
    return {
        one: 1,
        two: 2,
        "one": 1.0
    };
}

function test2()
{
    // can't have two gets:
    return {
        one: 1,
        get myget() {return true;},
        two: 2,
        get myget() {return false;}
    };
}

function test3()
{
    var bar = 42

    // can't have two sets:
    return {
        one: 1,
        set myset(v) {bar = v;},
        two: 2,
        set myset(v) {bar = 2* v;}
    };
}

function test4()
{
    var ack = 42;

    // can't have a data and a get/set:
    return {
        it: 1,
        get it() {return ack;},
        set it(v) {ack = v;}
    };
}

function test5()
{
    // can't have a data and a get/set:
    return {
        get it() {return true;},
        it: 1,
    };
}

function test6()
{
    var k;
    // can't have a data and a get/set:
    return {
        set it(v) {k=v},
        it: 1,
    };
}

function testOK()
{
    var foo = 0;

    // CAN have a get AND a set (but NO data)
    return {
        one: 1,
        two: 2,
        get my() {return foo;},
        set my(v) {foo = v;}
    };
}