"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/clickHub").build();
var HraId = document.getElementById("id-hry").innerText;
var HracId = document.getElementById("id-hrace").innerText;
connection.start().then(function () {
    console.log("Connection established.");

    
}).catch(function (err) {
    console.error(err.toString());
});
connection.onclose(function (error) {
    console.log('SignalR connection closed.', error);
    document.getElementById("instrukce").innerHTML = "Odpojeno! Obnov stránku";
});
function vytvorit_smenu_hraci(event) {
    event.preventDefault();
    const formSmenaSHraci = document.getElementById("smena-s-hraci-form");
    const formData = new FormData(formSmenaSHraci);
    let data = {};
    formData.forEach((value, key) => {
        data[key] = value;
    });
    connection.invoke("VytvorSmenuHraci", connection.connectionId, HracId, HraId, data)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
    document.getElementById("smena-container").style.display = "none";
}
function vytvorit_smenu_hra(event) {
    event.preventDefault();
    const formSmenaSHraci = document.getElementById("smena-s-hrou-form");
    const formData = new FormData(formSmenaSHraci);
    let data = {};
    formData.forEach((value, key) => {
        data[key] = value;
    });
    connection.invoke("VytvorSmenuHra", connection.connectionId, HracId, HraId, data)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
    document.getElementById("smena-container").style.display = "none";
}

function dalsi_tah(event) {
    connection.invoke("tlacitko_dalsi_klik", connection.connectionId, HracId, HraId)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
}

function klik_cesta(event) {
    var clickedId = event.target.id;
    connection.invoke("KliknutiNaCestu", connection.connectionId, clickedId, HracId, HraId)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
}

function klik_policko(event) {
    var clickedId = event.target.id;

    connection.invoke("KliknutiNaPolicko", connection.connectionId, clickedId, HracId, HraId)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
}

function klik_rozcesti(event) {
    var clickedId = event.target.id;

    connection.invoke("KliknutiNaRozcesti", connection.connectionId, clickedId, HracId, HraId)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
}
function klik_nakup(event) {
    var clickedId = event.target.id;

    connection.invoke("KliknutiNaNakup", connection.connectionId, clickedId, HracId, HraId)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
}
function klik_akcni_karta(event) {
var clickedId = event.target.id;

    connection.invoke("KliknutiNaAkcniKartu", connection.connectionId, clickedId, HracId, HraId)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
}
function klik_surovina(event) {
var clickedId = event.target.id;

    connection.invoke("KliknutiNaSurovinu", connection.connectionId, clickedId, HracId, HraId)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
}
function klik_hrac(event) {
var clickedId = event.target.id;

    connection.invoke("KliknutiNaHrace", connection.connectionId, clickedId, HracId, HraId)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
}
function tlacitko_smena(event) {
    const smenaContainer = document.getElementById("smena-container");
    const isVisible = smenaContainer.style.display === "flex";
    smenaContainer.style.display = isVisible ? "none" : "flex";
}
function provedeni_smeny(clickedId) {

    connection.invoke("KliknutiNaProvedeniSmeny", connection.connectionId, clickedId, HracId, HraId)
        .catch(function (err) {
            console.error("Invocation error:", err.toString());
        });
}

connection.on("ObnovitStrankuHry", function (hraId) {
    if (hraId == HraId)
        location.reload();
});
connection.on("ZmenZlodeje", function (HraId, Id) {
    if (hraId == HraId) {
        const e = document.getElementById(Id + "-zlodej");
        const isVisible = e.style.display === "block";
        e.style.display = isVisible ? "none" : "block";
    }
    
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
connection.on("ObnovSekci", function (hraId,sekce,data) {
    if (hraId == HraId)
        document.getElementById(sekce).innerHTML = data;
});

