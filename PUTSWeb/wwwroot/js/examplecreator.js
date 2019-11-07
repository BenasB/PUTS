$(document).ready(function () {
    bindDeleteExample();
});
$(document).ajaxComplete(function (event, xhr, settings) {
    bindDeleteExample();
});

function bindDeleteExample() {
    $('a.delete-example').on('click', function () {
        $(this).parents('div.example-case:first').remove();
        return false;
    });
}