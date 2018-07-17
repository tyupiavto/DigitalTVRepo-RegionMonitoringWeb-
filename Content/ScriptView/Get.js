var towerName;
var deviceID;
var towerID,TowerTextName;
var playGet = new Array();
var stopGet = new Array();
var getarray = new Array();

$('body').on('click touched', '.paly_stop_device', function () {
    deviceID = $(this).closest($(".foo")).attr("id");
    towerName = $('.tower_name' + deviceID).attr("title");
    towerID = $('#' + towerName).parent().parent().attr("id");
    TowerTextName = $('.header' + towerID).text();
    if ($('#paly_stop_device' + deviceID).attr('src') == '/Icons/play.png') {
        $('#paly_stop_device' + deviceID).attr("src", "/Icons/stop.png");
        $('#paly_stop_tower' + towerID).attr("src", "/Icons/stop.png");
    }
    else {
        $('#paly_stop_device' + deviceID).attr("src", "/Icons/play.png");
    }
    $.post("/GetNext/Get", { towerName: towerName, towerID: towerID, deviceID: deviceID,TowerTextName:TowerTextName }, function (Response) {
        if (Response == "false") {
            $('#paly_stop_tower' + towerID).attr("src", "/Icons/play.png");
            $('#paly_stop_device' + deviceID).attr("src", "/Icons/play.png");
        }
        saveDiagram();
        //else {
        //    $('#paly_stop_device' + deviceID).attr("src", "/Icons/play.png");
        //}
    });
});

$('body').on('click touched', '.paly_stop_tower', function () {
    var towerID = $(this).parent().parent().parent().parent().attr("id");
    var towerName = $(this).parent().parent().attr("id");
    TowerTextName = $('.header' + towerID).text();
    if ($('#paly_stop_tower' + towerID).attr('src') == '/Icons/play.png') {
        $('#paly_stop_tower' + towerID).attr("src", "/Icons/stop.png")
        $('.add' + towerID + '  table').each(function () {
            var deviceID = $(this).attr("id");
            $('#paly_stop_device' + deviceID).attr("src", "/Icons/stop.png");
            playGet.push(deviceID);
        });
        $.post("/GetNext/GetPlay", { towerName: towerName, towerID: towerID, playGet: playGet, TowerTextName:TowerTextName}, function (Response) {
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
        $.post("/GetNext/GetStop", { towerName: towerName, towerID: towerID, stopGet: stopGet, TowerTextName: TowerTextName}, function (Response) { stopGet = []; saveDiagram();}, 'json');
    }
});