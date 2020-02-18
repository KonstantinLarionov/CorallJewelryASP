var id_dialog = "";
var user = "admin";
var dialog = "";

function changeFunc() {
    
    var selectBox = document.getElementById("selectBox");
    var selectedValue = selectBox.options[selectBox.selectedIndex].value;
    id_dialog = selectedValue;
  
}


$(document).ready(function () {
    //update_cookie();
    GetMessages();
});
//function update_cookie() {
//    if ($.cookie('chat_user') === undefined) {
//        getUser();
//        $.cookie('chat_user', user, { expires: 365 });
//    } else {
//        user = $.cookie('chat_user');
//        $.cookie('chat_user', $.cookie('chat_user'), { expires: 365 });
//    }
//    if ($.cookie('dialog') !== undefined) {
//        id_dialog = $.cookie('dialog');
//    }
//}
function getWelcome() {
    $.ajax({
        type: "GET",
        url: "/Admin/welcome_message",
        success: function (msg) {
            return msg;
        }
    });
}

function getDialog() {
    $.ajax({
        async: false,
        type: "POST",
        url: "/Admin/GetDialog",
        data: "user=" + user,
        success: function (msg) {
            id_dialog = msg;
            return "1";
        }
    });
}

function getUser() {
    var dt = new Date();
    var cd = dt.getFullYear() + dt.getMonth() + dt.getDate() + (dt.getHours() + "").padStart(2, "0") + (dt.getMinutes() + "").padStart(2, "0") + (dt.getSeconds() + "").padStart(2, "0");
    user = "user_" + cd;
    }

function SendMessage() {

    var message = $('#chat-message').val();
    if (message === "") {
        //swal("Пустое сообщение!");
        return;
    }
    if (id_dialog === "") {
        getDialog();
        $.cookie('dialog', id_dialog, { expires: 365 });
    }
    $.ajax({
        type: "POST",
        url: "/Admin/SendMessage",
        data: "text=" + message + "&dialog=" + id_dialog + "&user=" + user,
        success: function () {
            $('#chat-message').val('');
        }
    });
}
function GetMessages() {
        //if (id_dialog === "") {
        //    continue;
        //}
    //console.log("request messages");
    setInterval(function () {
        $.ajax({
            type: "POST",
            url: "/Admin/GetMessages",
            data: "dialog=" + id_dialog,
            success: function (msg) {
                if (msg != null) {
                    dialog = JSON.parse(msg);
                    $('#chat-list').empty();
                    dialog.forEach(function (item, i, arr) {
                        if (item.User === user) {
                            add_clientmessage(item.Text);
                        } else {
                            add_adminmessage(item.Text);
                        }
                    });
                }
            }
        });
    }, 100);
}

function goto(href){
    window.open(href, '_blank');
}

function chat() {
    $("#chat-icon").toggleClass('chat-hidden');
    $('#chat-wrap').toggleClass('chat-hidden');
}

function add_clientmessage(message) {
    //var dt = new Date();
    //var time = (dt.getHours() + "").padStart(2, "0") + ":" + (dt.getMinutes() + "").padStart(2, "0");
    //$('#chat-list').append('<div class="chat-msg-client">' + $('#chat-message').val() + '<br><span style="font-style: italic; color: #858585; text-align: right;">' + time + '</span></div>');
    $('#chat-list').append('<div class="chat-msg-client">' + message + '</div>');
    $('#chat-list').animate({ scrollTop: $('#chat-list').prop('scrollHeight') }, 500);
    
}

function add_adminmessage(message) {
    //var dt = new Date();
    //var time = (dt.getHours() + "").padStart(2, "0") + ":" + (dt.getMinutes() + "").padStart(2, "0");
    //$('#chat-list').append('<div class="chat-msg-admin">' + message + '<br><span style="font-style: italic; color: #858585; text-align: right;">' + time + '</span></div>');
    $('#chat-list').append('<div class="chat-msg-admin">' + message + '</div>');
    $('#chat-list').animate({ scrollTop: $('#chat-list').prop('scrollHeight') }, 500); 
}
