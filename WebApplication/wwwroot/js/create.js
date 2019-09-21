$('#add-test').click(function () {
    $.ajax({
        url: this.href,
        cache: false,
        success: function (html) {
            $('#tests').append(html);
        }
    });
return false;
});

$('#add-example').click(function () {
    $.ajax({
        url: this.href,
        cache: false,
        success: function (html) {
            $('#examples').append(html);
        }
    });
    return false;
});