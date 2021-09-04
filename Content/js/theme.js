"use strict";

document.addEventListener("DOMContentLoaded", function() {
    const sidebarToggler = document.querySelector(".sidebar-toggler");

    if (sidebarToggler) {
        sidebarToggler.addEventListener("click", function(e) {
            e.preventDefault();

            document.querySelector(".sidebar").classList.toggle("shrink");
            document.querySelector(".sidebar").classList.toggle("show");
        });
    }

    const searchFormControl = document.getElementById("searchInput");
    const dropdownMenu = document.getElementById("searchDropdownMenu");

    if (searchFormControl && dropdownMenu) {
        searchFormControl.addEventListener("focus", function(e) {
            var dropdownMenus = [].slice.call(
                document.querySelectorAll(".dropdown-menu.show:not(#searchDropdownMenu)")
            );
            dropdownMenus.map(function(dropdownMenu) {
                dropdownMenu.classList.remove("show");
            });

            dropdownMenu.classList.add("d-block");
        });
        document.addEventListener("click", function(e) {
            if (e.target.id == "searchInput" || e.target.closest("#searchDropdownMenu")) {
                dropdownMenu.classList.add("d-block");
            } else {
                dropdownMenu.classList.remove("d-block");
            }
        });
    }

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function(tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});