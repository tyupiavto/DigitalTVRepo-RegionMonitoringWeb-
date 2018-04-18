$('body').on('click touchend', '#start_log', function () {
    window.open('/Trap/LogSetting', 'Trap');

    //$.post("/Trap/LogShow", {}, function (Response) {

    //    $('#trap_log_information').html("");
    //    $('#trap_log_information').html(Response);

    //});
});

$("#buttrap").click(function () {
    $.post("/Trap/LogShow", {}, function (Response) {

        $('#trap_log_information').html("");
        $('#trap_log_information').html(Response);

    });
});