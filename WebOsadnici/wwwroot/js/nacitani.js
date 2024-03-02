"use strict";
// Funkce, která se provede po kliknutí na odkaz
function handleClick(event) {
    // Zabránění výchozímu chování odkazu (navigace na novou stránku)
    event.preventDefault();

// Získání elementu pro zobrazení načítání
var loadingMessageElement = document.getElementById('loading-message');

// Zobrazení zprávy o načítání
loadingMessageElement.style.display = "block";

// Získání URL odkazu
window.location.href = event.currentTarget.href;
}

// Získání všech odkazů na stránce
var links = document.querySelectorAll('.hra');

// Přidání event listeneru na kliknutí pro každý odkaz
links.forEach(function (link) {
    link.addEventListener('click', handleClick);
});