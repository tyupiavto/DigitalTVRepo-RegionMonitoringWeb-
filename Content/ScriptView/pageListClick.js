

//$(document).ready(function () {
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
                    TrapPageCheck();
                }
            }, 'text');
        }
    });


    function TrapPageCheck() {
        $('#dropdown li').each(function () {
            if ($(this).children().children().is(':checked') == false) {
                $('td:nth-child(' + $(this).children().children().attr("value") + '),th:nth-child(' + $(this).children().children().attr("value") + ')').hide();
                var column = "table #" + $(this).attr("id");
                $(column).hide();
            }
            else {
                var column = "table #" + $(this).attr("id");
                $(column).show();
                $('td:nth-child(' + $(this).children().children().attr("value") + '),th:nth-child(' + $(this).children().children().attr("value") + ')').show();
            }
        });
    }