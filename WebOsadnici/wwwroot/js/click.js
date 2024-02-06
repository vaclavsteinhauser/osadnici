"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/clickHub").build();

connection.start().then(function () {
    console.log("Connection established.");
}).catch(function (err) {
    return console.error(err.toString());
});

// Posluchaè pro metodu SvgIdReceived
connection.on("SvgIdReceived", function (svgId) {
    console.log("Received SVG ID from server:", svgId);

    // Zpracujte svgId podle potøeby, napøíklad aktualizujte UI
    document.getElementById("receivedSvgId").innerText = "Received SVG ID: " + svgId;
});


var polygons = document.querySelectorAll("svg > *");

polygons.forEach(function (polygon) {
    polygon.addEventListener("click", function (event) {
        var clickedId = findClosestSvgId(polygon.closest("svg"));

        connection.invoke("ReceiveSvgId", connection.connectionId, document.getElementById("id-hry").innerText, clickedId)
            .catch(function (err) {
                console.error("Invocation error:", err.toString());
            });

        document.getElementById("text").innerText = "Id odesláno: " + clickedId;
    });
});

function findClosestSvgId(element) {
    if (element.tagName === "svg") {
        return element.id;
    } else if (element.parentNode) {
        return findClosestSvgId(element.parentNode);
    } else {
        return null;
    }
}