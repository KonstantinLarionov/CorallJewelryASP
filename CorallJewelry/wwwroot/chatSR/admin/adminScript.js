var dialogs = document.getElementById('listDialog');
var buttonHide = document.getElementById('hiddeDialog');
var header = document.getElementsByClassName('header-dialogs')[0];
var mainChat = document.getElementsByClassName('main-chat')[0];

buttonHide.addEventListener("click", function () {
    if (buttonHide.innerText === "<<") {
        dialogs.style.width = "0"
        buttonHide.innerHTML = ">>"
        header.setAttribute("hidden", "1");
        mainChat.style.width = "100%";
    }
    else {
        dialogs.style.width = "25%"
        mainChat.style.width = "75%";
        buttonHide.innerHTML = "<<"
        header.removeAttribute("hidden");
    }
});
