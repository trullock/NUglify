// import examples that should minify fine
import $ from "jquery-2.0.0.min";
$(() => alert("JQUERY!"));

// import encrypt and decrypt from external module
import { encrypt, decrypt } from "crypto";
encrypt("this");
decrypt("that");

// importing foo, but the local value is bat
import { foo as bat } from "libraries/bat";
bat();

// import library's default object to mylib
import mylib from "external/lib";
alert(mylib);

// import external module; don't execute
import "ack";

// import two methods with the same name from two different libraries, but rename them locally
import { draw as drawShape } from 'shape';
import { draw as drawGun } from 'cowboy';
drawShape();
drawGun();

// import sum and pi; should not be undefined
import { sum, pi } from 'math';
alert(sum(pi, 3));

