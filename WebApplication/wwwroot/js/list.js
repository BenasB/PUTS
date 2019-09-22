$(document).ready(function () {
    $('#deletionModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var clickedButtonId = button.data('id');

        document.getElementById("hiddenid").value = clickedButtonId;
    });
});