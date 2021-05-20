"use strict";

var signalR = new signalR.HubConnectionBuilder();
var connection = signalR.withUrl("/chatHub").build();
connection.serverTimeoutInMilliseconds = 1000 * 60 * 60 * 3;
let recipient = document.getElementById("InstrumentItemId").value;;
connection.start();

connection.on("Notify", function (message) {
    var li = document.createElement("li");
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveMessage", function (message, id) {

    if (id != document.getElementById("InstrumentItemId").value)
        return;
    
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    // li.onclick("labelBlocked") = selectReceiver(receiver);
    li.onclick = function () { selectReceiver(message); };
    
    li.textContent = msg;

    document.getElementById("messagesList").appendChild(li);
});

function selectReceiver(message) {
    let m = document.getElementById("labelBlocked");
    var name = message.slice(0, message.indexOf(":"));
    if (m != null) {
        m.innerHTML = 'Заблокировать: ' + name;
        var serch = document.getElementById("blocked");
        serch.setAttribute("value", name);

        
    }
    
   
    
};



document.getElementById("sendButton").addEventListener("click", function (event) {
    const blocked = document.getElementById("IsBlocked");
    if (blocked.checked) {
        var time = document.getElementById("UpTo").value;
        var now = new Date();
        if (now >= time) {
            blocked.value = false;
        }
        else {
            setLabelBlocked();
            return;
        }
    }

    document.getElementById("labelBlocked").context = "";
    var message = document.getElementById("messageInput").value;
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    connection.invoke("SendMessage", msg, null).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendPrivateButton").addEventListener("click", function (event) {
    recipient = document.getElementById("InstrumentItemId").value;
    var message = document.getElementById("messageInput").value;
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    connection.invoke("SendMessage", msg, recipient).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("ReceiveBlocked", function (upto) {
    if (upto <= new Date()) {
        document.location.reload();
        return;
    }
    document.getElementById("IsBlocked").value = true;
    document.getElementById("UpTo").value = upto;
    setLabelBlocked();
});

document.getElementById("blockButton").addEventListener("click", function (event) {
    var ticks = document.getElementById("ticks");
    var tick = ticks.options[ticks.selectedIndex].value;
    var recipient = document.getElementById("selectedUser").value;
    connection.invoke("SendBlock", recipient, tick.toString()).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function setLabelBlocked() {
    var label = document.getElementById("labelBlocked");
    var upto = document.getElementById("UpTo").value;
    if (upto === null || upto <= new Date()) {
        label.innerText = "";
    } else {
        label.innerText = "Вы заблокированы модератором и не можете посылать сообщений до " + upto + "!";
    }
}