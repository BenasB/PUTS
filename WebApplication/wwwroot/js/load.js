$(document).ready(function () {
    $.ajax({
        url: this.href,
        cache: false,
        success: function () {
            var problemID = $(this).data('id');
            var filePath = $(this).data('filePath');
            var startingUrl = window.location.pathname;
            var url = "/Problem/Result/" + startingUrl.substring(startingUrl.lastIndexOf('/')) + window.location.search;
            window.location.href = url;
        }
    });
});