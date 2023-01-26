'use strict';

function test1(
    {
        verylongname_x = 1,
        verylongname_y = 2
    } = {}) {
    console.log(verylongname_x + verylongname_y);
}

function test2(verylongname_x = 1, verylongname_y = 2) {
    console.log(verylongname_x + verylongname_y);
}