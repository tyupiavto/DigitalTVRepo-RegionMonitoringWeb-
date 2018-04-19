

$(document).ready(function () {
    $('#page_list').on('click', 'a', function (e) {
        e.preventDefault();
        if (this.href != "") {
            $.ajax({
                url: this.href,
                type: 'GET',
                cache: false,
                success: function (Response) {
                    $('#device_settings').html("");
                    $('#device_settings').html(Response);
                }
            }, 'text');
        }
    });


    $('#page_Log').on('click', 'a', function (e) {
        e.preventDefault();
        if (this.href != "") {
            $.ajax({
                url: this.href,
                type: 'GET',
                cache: false,
                success: function (Response) {
                    $('#loginformation').html("");
                    $('#loginformation').html(Response);
                }
            }, 'text');
        }
    });
});