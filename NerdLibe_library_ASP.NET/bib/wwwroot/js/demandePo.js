$(function () {
    $('button[data-toggle="modal"]').click(function (event) {
        $('#addDemande').modal('show');
    });
});
$(function () {
    $('button[data-dismiss="modalDimiss"]').click(function () {
        // Close the modal without sending any data
        $('#addDemande').modal('hide');

        // Rediriger vers la page books
        window.location.href = '/Client/books';
    });
});