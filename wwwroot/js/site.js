// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#myTablePrint').DataTable({
        "scrollY": "auto",
        "scrollCollapse": true,
        "paging": true,
        "dom": "<'row'<'col-sm-12 col-md-4'l><'col-sm-12 col-md-4'B><'col-sm-12 col-md-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>"

    });
});
$(document).ready(function () {
    $('#myTable').DataTable({
        "scrollY": "auto",
        "scrollCollapse": true,
        "paging": false,
        "dom": "<'row'<'col-sm-12 col-md-4'l><'col-sm-12 col-md-4'><'col-sm-12 col-md-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>"

    });
});
$(document).ready(function () {
    $('#groupTable').DataTable({
        "scrollY": "auto",
        "scrollCollapse": false,
        "paging": false,
        "dom": "tr" 

    });
});
