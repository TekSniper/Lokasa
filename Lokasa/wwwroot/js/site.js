// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
/*
// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', () => {

    // Get all "navbar-burger" elements
    const $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);

    // Add a click event on each of them
    $navbarBurgers.forEach(el => {
        el.addEventListener('click', () => {

            // Get the target from the "data-target" attribute
            const target = el.dataset.target;
            const $target = document.getElementById(target);

            // Toggle the "is-active" class on both the "navbar-burger" and the "navbar-menu"
            el.classList.toggle('is-active');
            $target.classList.toggle('is-active');

        });
    });

});*/
document.addEventListener('DOMContentLoaded', () => {
    // Récupère tous les éléments "navbar-burger"
    const navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);

    // Ajoute l'événement click
    if (navbarBurgers.length > 0) {
        navbarBurgers.forEach(burger => {
            burger.addEventListener('click', () => {
                // Récupère la cible à partir de data-target
                const target = burger.dataset.target;
                const menu = document.getElementById(target);

                // Alterne les classes "is-active"
                burger.classList.toggle('is-active');
                menu.classList.toggle('is-active');
            });
        });
    }

    // Assure que la navbar se ferme lorsqu'on clique en dehors
    document.addEventListener('click', (event) => {
        if (!event.target.closest('.navbar') && document.querySelector('.navbar-menu.is-active')) {
            document.querySelector('.navbar-menu.is-active').classList.remove('is-active');
            document.querySelector('.navbar-burger.is-active').classList.remove('is-active');
        }
    });
});