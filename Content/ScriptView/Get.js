var towerName;
var deviceID;
var towerID;
var playGet = new Array();
var stopGet = new Array();
var getarray = new Array();
//$('#get_next').click(function () {
//    $.post("/GetNext/Get", {}, function () { });
//});

$('body').on('click touched', '.paly_stop_device', function () {
    deviceID = $(this).closest($(".foo")).attr("id");
    towerName = $('.tower_name' + deviceID).attr("title");
    towerID = $('#' + towerName).parent().parent().attr("id");

    if ($('#paly_stop_device' + deviceID).attr('src') == '/Icons/play.png') {
        $('#paly_stop_device' + deviceID).attr("src", "/Icons/stop.png");
        $('#paly_stop_tower' + towerID).attr("src", "/Icons/stop.png");
    }
    else {
        $('#paly_stop_device' + deviceID).attr("src", "/Icons/play.png");
    }
    $.post("/GetNext/Get", { towerName: towerName, towerID: towerID, deviceID: deviceID }, function (Response) {
        if (Response == "1") {
            $('#paly_stop_tower' + towerID).attr("src", "/Icons/play.png");
            $('#paly_stop_device' + deviceID).attr("src", "/Icons/play.png");
        }
        //else {
        //    $('#paly_stop_device' + deviceID).attr("src", "/Icons/play.png");
        //}
    });
});

$('body').on('click touched', '.paly_stop_tower', function () {
    var towerID = $(this).parent().parent().parent().parent().attr("id");
    var towerName = $(this).parent().parent().attr("id");

    if ($('#paly_stop_tower' + towerID).attr('src') == '/Icons/play.png') {
        $('#paly_stop_tower' + towerID).attr("src", "/Icons/stop.png")
        $('.add' + towerID + '  table').each(function () {
            var deviceID = $(this).attr("id");
            $('#paly_stop_device' + deviceID).attr("src", "/Icons/stop.png");
            playGet.push(deviceID);
        });
        $.post("/GetNext/GetPlay", { towerName: towerName, towerID: towerID, playGet: playGet }, function (Response) {
            $.each(Response,function (e,index) {
                $('#paly_stop_device' + index).attr("src", "/Icons/play.png");
            });
            playGet = []; saveDiagram();
        }, 'json');
    }
    else {
        $('#paly_stop_tower' + towerID).attr("src", "/Icons/play.png");
        $('.add' + towerID + '  table').each(function () {
            var deviceID = $(this).attr("id");
            $('#paly_stop_device' + deviceID).attr("src", "/Icons/play.png");
            stopGet.push(deviceID);
        });
        $.post("/GetNext/GetStop", { towerName: towerName, towerID: towerID, stopGet: stopGet }, function (Response) { stopGet = []; saveDiagram();}, 'json');
    }

    //$('.add' + towerID + '  table').each(function () {
    //    var deviceID = $(this).attr("id");
    //    if ($('#paly_stop_device' + deviceID).attr('src') == '/Icons/play.png') {
    //        $('#paly_stop_device' + deviceID).attr("src", "/Icons/stop.png");
    //        $.post("/GetNext/GetPlay", { towerName: towerName, towerID: towerID, deviceID: deviceID }, function (Response) { }, 'json');
    //    }
    //    else {
    //        $('#paly_stop_device' + deviceID).attr("src", "/Icons/play.png");
    //        $.post("/GetNext/GetStop", { towerName: towerName, towerID: towerID, deviceID: deviceID }, function (Response) { }, 'json');
    //    }
        //$.post("/GetNext/Get", { towerName: towerName, towerID: towerID, deviceID: deviceID }, function (Response) {}, 'json');
    //});
});