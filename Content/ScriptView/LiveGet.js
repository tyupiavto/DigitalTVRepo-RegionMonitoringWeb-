var response;
$('body').on('click touched', '#clicklive', function () {
    $.post('/LiveGet/GetLiveSensor', {}, function (Response) {
        $('#liveGetSend').html("");
        $('#liveGetSend').html(Response);
    });
});

$('body').on('click touched', '#start_live_get', function () {
    var win = window.open("/LiveGet/LiveGet");
    if (win) {
        //Browser has allowed it to be opened
        win.focus();

        $.post('/LiveGet/GetLiveSensor', {}, function (Response) {
            $('#liveGetSend').html("");
            $('#liveGetSend').html(Response);
        });
    }
   
    event.preventDefault();
    //setTimeout(livegetopen, 2000);
});

function livegetopen() {
    $.post('/LiveGet/GetLiveSensor', {}, function (Response) {
        $('#liveGetSend').html("");
        $('#liveGetSend').html(Response);
        response = Response;
        alert(Response);
    });
    //alert(response);
}