document.addEventListener('DOMContentLoaded', () => {
    const menuToggle = document.getElementById('menu-toggle-2');
    const sidebar = document.getElementById('sidebar');
    const content = document.getElementById('content');

    // Gestion du bouton burger
    menuToggle.addEventListener('click', () => {
        sidebar.classList.toggle('is-active'); // Ouvre/ferme le menu
        sidebar.classList.toggle('is-hidden'); // Cache ou montre
        content.classList.toggle('is-fullwidth'); // Ajuste le contenu
    });

    // Vérifie la taille de l'écran et supprime "is-hidden" en mode PC
    window.addEventListener('resize', () => {
        if (window.innerWidth >= 768) { // Mode PC
            sidebar.classList.remove('is-hidden-mobile'); // Toujours visible
            sidebar.classList.remove('is-active'); // Pas d'animation active
            content.classList.remove('is-fullwidth'); // Marge pour le menu
        }
    });
});