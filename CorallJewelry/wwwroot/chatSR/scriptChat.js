function closeChat() {
    var mainChat = document.getElementById("mainChat");
    var input = document.getElementById("inmessage");
    var head = document.getElementById('head');
    if (mainChat.style.height !== "0px") {
       
        mainChat.style.transition = "0.7s";
        head.style.transition = "0.7s";
        inmessage.style.transition = "2s";
        mainChat.style.overflow = "hidden";
        mainChat.style.height = "0px";
        mainChat.style.width = "0px";
        mainChat.style.padding = "0px";
        inmessage.style.height = "0px";
        mainChat.style.bottom = "5px";
        inmessage.style.display = "none";
        head.style.bottom = "15px";

        document.getElementById("yourmess").style.transition = "0.6s";
        document.getElementById("mymess").style.transition = "0.6s";

        document.getElementById("yourmess").style.display = "none";
        document.getElementById("mymess").style.display = "none";

        document.getElementById("inputMessage").value = "";
    }
    else {
        mainChat.style.padding = "10px";
        mainChat.style.overflow = "auto";
        mainChat.style.width = "300px";

        mainChat.style.transition = "0.7s";
        head.style.transition = "0.7s";
        inmessage.style.transition = "2s";
        mainChat.style.height = "400px";
        head.style.bottom = '470px';
        inmessage.style.height = "80px";
        mainChat.style.bottom = "90px";
        inmessage.style.display = "";
        document.getElementById("yourmess").style.transition = "0.7s";
        document.getElementById("mymess").style.transition = "0.7s";

        document.getElementById("yourmess").style.display = "";
        document.getElementById("mymess").style.display = "";
    }
}

