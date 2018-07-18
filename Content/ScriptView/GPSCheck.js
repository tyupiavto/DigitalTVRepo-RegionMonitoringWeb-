var ChekedGps = new Array();
var devicetype = new Array();
var res = new Array();
var liLattitube = new Array();
var liLongitube = new Array();
var liAltitube = new Array();
var GpsCoordinate = new Array();
var gpsInd = 0, GpsID, TowerGpsID,towerGpsName;
var deviceGpsName, lattitube, longitube, altitube, textadd,gpscheckInd=0,towerName,IP;
$(document).on('click touchend', '#GpsSetting', function () { // add device setting open
    TowerGpsID = $(this).closest($(".foo")).attr("id");
    towerName = $('.header' + TowerGpsID).text();

    towerGpsName = $(this).parent().parent().attr("id");
    var dv = $('.device_list_name' + TowerGpsID);
    gpscheckInd = 0;
    dv.find('table').each(function () {
        devicetype.push($(this).attr("name"));
    });

    $.post("/DeviceGroup/TowerGpsSetting", { devicetype: devicetype, towerName:towerName }, function (Response) {
        $('#settingsDiv').html("");
        $('#settingsDiv').html(Response);
        devicetype = [];
    }, 'text');
});

$('body').on('click', '#modal_hiden', function () {
    gpsInd = 0;
    $('#tower_ip').val("");
    $('#description_value_search').val("");
});

$('body').on('contextmenu touched', '.map_check div', function () { // checked gps right click
    GpsID = $(this).attr("id");
    
    $(document).bind("contextmenu", function (event, ui) {
        event.preventDefault();
        $(this).unbind(event);
        $(".custom-menu").finish().toggle(100).css({
            top: (event.pageY-20) + "px",
            left: (event.pageX-110) + "px"
        });
        event.stopPropagation();
    });
});


$(".custom-menu li").click(function (event) {

    switch ($(this).attr("data-action")) {
        case "Yes":
            if (gpsInd < 3 && !$('#map_checked_add' + GpsID).hasClass("checkedGps")) {
                gpsInd++;
                $('#map_checked_add' + GpsID).removeClass("").addClass("checkedGps");
                towerName = $('#device_settings_name').text();
                $.post("/DeviceGroup/GpsSelect", { GpsID: GpsID, towerName: towerName, deviceID: deviceID }, function () { });
            }
            break;
        case "No":
            if ($('#map_checked_add' + GpsID).hasClass("checkedGps")) {
                $('#map_checked_add' + GpsID).removeClass("checkedGps").addClass("");
                gpsInd--;
                towerName = $('#device_settings_name').text();
                $.post("/DeviceGroup/GpsUnSelect", { GpsID: GpsID, towerName: towerName, deviceID: deviceID }, function () {});
            }
            break;
    }
    $(".custom-menu").hide(100);
});

$('body').on('click touched', '#gps_check div', function () { // map click checked 
    var gpscheckID = $(this).attr("id");

    if ($('#gps_checked' + gpscheckID).is(':checked') == false ) {
        
        if (gpscheckInd != 1) {
            gpscheckInd = 1;
            $('#gps_checked_add' + gpscheckID).removeClass("").addClass("checked");
            $("#gps_checked" + gpscheckID).prop('checked', true);
          var  deviceName = $('#gpsDevice_name' + gpscheckID).attr("value");

            $.post("/DeviceGroup/CheckGps", { towerGpsName: towerGpsName, deviceName: deviceName }, function (Response) {
                GpsCoordinate.push("<li id='lattibute'>" + Response[0].Type + "</li>");
                GpsCoordinate.push("<li id='lattibute'>" + Response[1].Type + "</li>");
                GpsCoordinate.push("<li id='lattibute'>" + Response[2].Type+ "</li>");
                IP = Response[0].IP;

                $('#lattitube_list').html(GpsCoordinate);
                $('#longitube_list').html(GpsCoordinate);
                $('#altitube_list').html(GpsCoordinate);
                GpsCoordinate = [];
            }, 'json');
        }
    } else {
        $('#lattitube_list').html("");
        $('#longitube_list').html("");
        $('#altitube_list').html("");

        $('#gps_checked_add' + gpscheckID).removeClass("checked").addClass("");
        $("#gps_checked" + gpscheckID).prop('checked', false);
        gpscheckInd = 0;
        $('#lattitube_name').val("");
        $('#longitube_name').val("");
        $('#altitube_name').val("");
      
    }
});
var handled = false;
$('body').on('click touchend', '#lattitube_select', function (e) { // Lattitube list gps
    width = $(this).width() - 20;
    textadd = false;
    $('#lattitube_cordinat').click(function () {
        textadd = true;
    });
    if (e.type == "touchend") {
        handled = true;
        $('#gps_list_lattitube').css({ 'width': width }).toggle();
    }
    else
        if (e.type == "click" && !handled && !textadd) {
            $('#gps_list_lattitube').css({ 'width': width }).toggle();
        }
        else {
            handled = false;
        }
});
$('body').on('click touchend', '#gps_list_lattitube li', function () { // walk list add
    $('#lattitube_name').val($(this).text());
});

$('body').on('click touchend', '#longitube_select', function (e) { // Longitube list gps
    width = $(this).width()-20;
    if (e.type == "touchend") {
        handled = true;
        $('#gps_list_longitube').css({ 'width': width + 0 }).toggle();
        $('#longitube_select li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#gps_list_longitube').css({ 'width': width + 0 }).toggle();
        }
        else {
            handled = false;
        }
});
$('body').on('click touchend', '#gps_list_longitube li', function () { // walk list add
    $('#longitube_name').val($(this).text());
});

$('body').on('click touchend', '#altitube_select', function (e) { // Altitube list gps
    width = $(this).width()-20;
    if (e.type == "touchend") {
        handled = true;
        $('#gps_list_altitude').css({ 'width': width + 0 }).toggle();
        $('#altitube_select li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#gps_list_altitude').css({ 'width': width + 0 }).toggle();
        }
        else {
            handled = false;
        }
});
$('body').on('click touchend', '#gps_list_altitude li', function () { // walk list add
    $('#altitube_name').val($(this).text());
});

$('body').on('click touchend', '#gps_cor_sub', function () {
    gpscheckInd = 1;
    lattitube = $('#lattitube_name').val();
    longitube = $('#longitube_name').val();
    altitube = $('#altitube_name').val();
    towerName = $('.header' + TowerGpsID).text();
    $.post("/DeviceGroup/TowerGpsSubmit", { deviceGpsName: deviceGpsName, lattitube: lattitube, longitube: longitube, altitube: altitube, towerName: towerName, gpscheckInd: gpscheckInd, IP: IP, TowerGpsID:TowerGpsID }, function () {
        $('#settingsDiv').html("");
    }, 'json');
});
