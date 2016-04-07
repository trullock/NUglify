var i = 0;
for (i = 0; i <= 100; i++) {
    if (i % 2 == 0) write(i);
}

function write(i) {
    document.write("The number is " + i);
    document.write("<br />");    
}