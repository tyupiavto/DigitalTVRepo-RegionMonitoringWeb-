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
    if (win) {
        //Browser has allowed it to be opened
        win.focus();

        //$.post('/LiveGet/GetLiveSensor', {}, function (Response) {
        //    $('#liveGetSend').html("");
        //    $('#liveGetSend').html(Response);
        //});
    }

  //  event.preventDefault();
    //setTimeout(livegetopen, 2000);
});

$('body').on('click touched', '#liveDeviceName', function () {

    var wins = window.open("/LiveGet/GetChart");
    DeviceID = $(this).attr("value");
    IP = $(this).attr("name");
    $.post('/LiveGet/DeviceInformation', { DeviceID: DeviceID, IP: IP }, function () {});
});
$('body').on('contextmenu touched', '#liveDeviceName a', function () { // checked gps right click
  
    $(document).bind("contextmenu", function (event, ui) {
        event.preventDefault();
        $(this).unbind(event);
        $(".chart-menu").finish().toggle(100).css({
            top: 10 + "px",
            left:10+ "px"
        });
        event.stopPropagation();
    });
   // var wins = window.open("/LiveGet/GetChart");
});

$(".chart-menu li").click(function (event) {

    switch ($(this).attr("data-action")) {
        case "ChartSensor":
            var win = window.open("/LiveGet/GetChart");
            if (win) {
                win.focus();
            }
            break;
    }
    $(".chart-menu").hide(100);
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