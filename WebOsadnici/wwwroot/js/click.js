"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/clickHub").build();
var HraId = document.getElementById("id-hry").innerText;
var HracId = document.getElementById("id-hrace").innerText;
connection.start().then(function () {
    console.log("Connection established.");

    document.getElementById("tlacitko_smena").addEventListener("click", function (event) {
        connection.invoke("tlacitko_smena_klik", connection.connectionId, HracId, HraId)
            .catch(function (err) {
                console.error("Invocation error:", err.toString());
            });
    });
    document.getElementById("tlacitko_dalsi").addEventListener("click", function (event) {
        connection.invoke("tlacitko_dalsi_klik", connection.connectionId, HracId, HraId)
            .catch(function (err) {
                console.error("Invocation error:", err.toString());
            });
    });

    var cesty = document.getElementsByClassName("cesta");
    Array.from(cesty).forEach(function (cesta) {
        cesta.addEventListener("click", function (event) {
            var clickedId = cesta.id;

            connection.invoke("KliknutiNaCestu", connection.connectionId, clickedId, HracId,HraId)
                .catch(function (err) {
                    console.error("Invocation error:", err.toString());
                });
            document.getElementById("text").innerText = clickedId;
        });
    });

    var policka = document.getElementsByClassName("policko");
    Array.from(policka).forEach(function (policko) {
        policko.addEventListener("click", function (event) {
            var clickedId = policko.id;

            connection.invoke("KliknutiNaPolicko", connection.connectionId, clickedId, HracId,HraId)
                .catch(function (err) {
                    console.error("Invocation error:", err.toString());
                });
            document.getElementById("text").innerText = clickedId;
        });
    });

    var rozcesti = document.getElementsByClassName("rozcesti");
    Array.from(rozcesti).forEach(function (rozcest) {
        rozcest.addEventListener("click", function (event) {
            var clickedId = rozcest.id;

            connection.invoke("KliknutiNaRozcesti", connection.connectionId, clickedId,HracId,HraId)
                .catch(function (err) {
                    console.error("Invocation error:", err.toString());
                });
            document.getElementById("text").innerText = clickedId;
        });
    });
}).catch(function (err) {
    console.error(err.toString());
});
connection.on("ObnovitStrankuHry", function (hraId) {
    if (hraId==HraId)
        location.reload();
});

connection.on("NastavText", function (odpoved) {
    document.getElementById("instrukce").innerHTML = odpoved;
});

connection.on("NastavStavbu", function (id, stavba) {
    var obrazek = document.getElementById(id).nextElementSibling;
    obrazek.setAttribute("xlink:href", "../../"+stavba);
    obrazek.setAttribute("display", "inline");
});
connection.on("NastavBarvu", function (id, barva) {
    document.getElementById(id).setAttribute("fill", barva);
});
