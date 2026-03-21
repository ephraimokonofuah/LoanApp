// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Sidebar toggle
$(document).ready(function () {
    var sidebar = $('#sidebar');
    var overlay = $('#sidebarOverlay');

    // Desktop toggle
    $('#sidebarToggle').on('click', function () {
        $('body').toggleClass('sidebar-collapsed');
    });

    // Mobile: navbar toggler also opens sidebar
    $('.navbar-toggler').on('click', function () {
        if ($(window).width() < 768 && sidebar.length) {
            sidebar.toggleClass('show');
            overlay.toggleClass('show');
        }
    });

    // Close sidebar on overlay click (mobile)
    overlay.on('click', function () {
        sidebar.removeClass('show');
        overlay.removeClass('show');
    });

    // Close mobile sidebar on link click
    sidebar.on('click', '.sidebar-link', function () {
        if ($(window).width() < 768) {
            sidebar.removeClass('show');
            overlay.removeClass('show');
        }
    });
});
