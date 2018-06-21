
$('body').on('click touched', '#clicklive', function () {
    $.post('/LiveGet/GetLiveSensor', {}, function (Response) {
        $('#liveGetSend').html("");
        $('#liveGetSend').html(Response);
    });
});