$(document).ready(function () {
    bindDeleteTest();
});
$(document).ajaxComplete(function (event, xhr, settings) {
    bindDeleteTest();
});

function bindDeleteTest() {
    $('a.delete-test').on('click', function () {
        $(this).parents('div.test-case:first').remove();
        return false;
    });
}