"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/clickHub").build();

//Disable the send button until connection is established.
//document.getElementById("sendButton").disabled = true;

/*connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});*/

document.getElementsByTagName("svg").forEach(addEventListener("click", function (event) {
    document.getElementById("text").innerText = this.id;
}));