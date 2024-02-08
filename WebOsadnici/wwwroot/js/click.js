"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/clickHub").build();

connection.start().then(function () {
    console.log("Connection established.");

    var cesty = document.getElementsByClassName("cesta");
    Array.from(cesty).forEach(function (cesta) {
        cesta.addEventListener("click", function (event) {
            var clickedId = cesta.id;

            connection.invoke("KliknutiNaCestu", connection.connectionId, clickedId)
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

            connection.invoke("KliknutiNaPolicko", connection.connectionId, clickedId)
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

            connection.invoke("KliknutiNaRozcesti", connection.connectionId, clickedId)
                .catch(function (err) {
                    console.error("Invocation error:", err.toString());
                });
            document.getElementById("text").innerText = clickedId;
        });
    });
}).catch(function (err) {
    console.error(err.toString());
});

connection.on("CestaKlikOdpoved", function (odpoved) {
    document.getElementById("kliknutacesta").innerText = odpoved;
});

connection.on("PolickoKlikOdpoved", function (odpoved) {
    document.getElementById("kliknutepolicko").innerText = odpoved;
});

connection.on("RozcestiKlikOdpoved", function (odpoved) {
    document.getElementById("kliknuterozcesti").innerText = odpoved;
});
connection.on("NastavVesnicku", function (odpoved) {
    var obrazek = document.getElementById(odpoved).nextElementSibling;
    obrazek.setAttribute("xlink:href", "../../vesnicka.svg");
    obrazek.setAttribute("display", "inline");
});
connection.on("NastavMesto", function (odpoved) {
    var obrazek = document.getElementById(odpoved).nextElementSibling;
    obrazek.setAttribute("xlink:href", "../../vesnicka.svg");
    obrazek.setAttribute("display", "inline");
});
connection.on("NastavBarvu", function (id, barva) {
    document.getElementById(id).setAttribute("fill", barva);
});
