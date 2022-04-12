var myPasswd = document.getElementById("psw");
var myConfirmPasswd = document.getElementById("samepasswd");
var letter = document.getElementById("letter");
var capital = document.getElementById("capital");
var number = document.getElementById("number");
var lenght = document.getElementById("length");
var char = document.getElementById("chars");
var check = document.getElementById("validcheck");


myPasswd.onfocus = function () {
    document.getElementById("message").style.display = "block";
}
myPasswd.onblur = function () {
    document.getElementById("message").style.display = "none";
}

myConfirmPasswd.onfocus = function () {
    document.getElementById("message").style.display = "block";
}
myConfirmPasswd.onblur = function () {
    document.getElementById("message").style.display = "none";
}



myPasswd.onkeyup = function () {
    var lowerCaseLetters = /[a-z]/g;
    if (myPasswd.value.match(lowerCaseLetters)) {
        letter.classList.remove("text-danger");
        letter.classList.add("text-success");
    }
    else {
        letter.classList.remove("text-success");
        letter.classList.add("text-danger");
    }
    var upperCaseLetters = /[A-Z]/g;
    if (myPasswd.value.match(upperCaseLetters)) {
        capital.classList.remove("text-danger");
        capital.classList.add("text-success");
    }
    else {
        capital.classList.remove("text-success");
        capital.classList.add("text-danger");
    }

    var numbers = /[0-9]/g;
    if (myPasswd.value.match(numbers)) {
        number.classList.remove("text-danger");
        number.classList.add("text-success");
    }
    else {
        number.classList.remove("text-success");
        number.classList.add("text-danger");
    }

    if (myPasswd.value.length >= 8) {
        lenght.classList.remove("text-danger");
        lenght.classList.add("text-success");
    } else {
        lenght.classList.remove("text-success");
        lenght.classList.add("text-danger");
    }
    if (myPasswd.value == myConfirmPasswd.value) {
        check.classList.remove("text-danger");
        check.classList.add("text-success");
    }
    else {
        check.classList.remove("text-success");
        check.classList.add("text-danger");
    }
    var chars =/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;
    if (myPasswd.value.match(chars)) {
        char.classList.remove("text-danger");
        char.classList.add("text-success");
    }
    else {
        char.classList.remove("text-success");
        char.classList.add("text-danger");
    }


}
myConfirmPasswd.onkeyup = function () {
    if (myPasswd.value == myConfirmPasswd.value) {
        check.classList.remove("text-danger");
        check.classList.add("text-success");
    }
    else {
        check.classList.remove("text-success");
        check.classList.add("text-danger");
    }
}