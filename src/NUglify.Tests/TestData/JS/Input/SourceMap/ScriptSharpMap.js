alertOnTimeout();

function alertOnTimeout() {
    setTimeout("invokeAlert()", 3000);
}

function invokeAlert() {
    alert("Hello");
}