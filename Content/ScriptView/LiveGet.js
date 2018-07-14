var response;
var DeviceID, IP;
var device = new Array();
$('body').on('click touched', '#clicklive', function () {
    $.post('/LiveGet/GetLiveSensor', {}, function (Response) {
        $('#liveGetSend').html("");
        $('#liveGetSend').html(Response);
    });
});

$('body').on('click touched', '#start_live_get', function () {
    var win = window.open("/LiveGet/LiveGet");
    if (win) {win.focus();}
});

$('body').on('click touched', '#openChart', function () {

    var wins = window.open("/LiveGet/GetChart");
    DeviceID = $(this).attr("value");
    IP = $(this).attr("name");
    $.post('/LiveGet/DeviceInformation', { DeviceID: DeviceID, IP: IP }, function () {});
});


$('body').on('click touched', '#chartmodalopen', function () {
    //alert(device)
    //DeviceID = 43;
    //IP = "192.168.24.11";
    //$.post('/LiveGet/ChartSensorLive', { DeviceID: DeviceID, IP: IP }, function (Response) {
    //    $('#showChartSensor').html("");
    //    $('#showChartSensor').html(Response);
    //});
});