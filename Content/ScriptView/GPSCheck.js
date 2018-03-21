var ChekedGps = new Array();
var devicetype = new Array();
var res = new Array();
var liLattitube = new Array();
var liLongitube = new Array();
var liAltitube = new Array();
var gpsInd = 0;
var deviceGpsName, textadd;
$(document).on('click touchend', '#GpsSetting', function () { // add device setting open
    var GpsID = $(this).closest($(".foo")).attr("id");
    var dv = $('.device_list_name' + GpsID);
    dv.find('table').each(function () {
        devicetype.push($(this).attr("name"));
    });

    $.post("/DeviceGroup/TowerGpsSetting", { devicetype: devicetype }, function (Response) {
        $('#settingsDiv').html("");
        $('#settingsDiv').html(Response);
        devicetype = [];
    }, 'text');
});

$('body').on('contextmenu touched', '.map_check div', function () { // checked gps right click
    GpsID = $(this).attr("id");
    
    $(document).bind("contextmenu", function (event, ui) {
        event.preventDefault();
        $(this).unbind(event);
        $(".custom-menu").finish().toggle(100).css({
            top: (event.pageY - 2) + "px",
            left: (event.pageX - 275) + "px"
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

                $.post("/DeviceGroup/GpsSelect", { GpsID: GpsID }, function () {

                });
            }
            break;
        case "No":
            if ($('#map_checked_add' + GpsID).hasClass("checkedGps")) {
                $('#map_checked_add' + GpsID).removeClass("checkedGps").addClass("");
                gpsInd--;
                $.post("/DeviceGroup/GpsUnSelect", { GpsID: GpsID }, function () {
                });
            }
            break;
    }
    $(".custom-menu").hide(100);
});

$('body').on('click touched', '#gps_check div', function () { // map click checked 
    var gpsID = $(this).attr("id");
    if ($('#gps_checked' + gpsID).is(':checked') == false) {
        $('#gps_checked_add' + gpsID).removeClass("").addClass("checked");
        $("#gps_checked" + gpsID).prop('checked', true);
        deviceGpsName = $('#gpsDevice_name' + gpsID).attr("value");
        $.post("/DeviceGroup/CheckGps", { deviceGpsName: deviceGpsName }, function (Response) {
            $.each(Response, function (i, value) {
                liLongitube.push("<li id='lattibute'>" + value.Longitude + "</li>");
                liLattitube.push("<li id='lattibute'>" + value.Lattitube + "</li>");
                liAltitube.push("<li id='lattibute'>" + value.Altitude + "</li>");
            });
            $('#lattitube_list').html(liLattitube);
            $('#longitube_list').html(liLongitube);
            $('#altitube_list').html(liAltitube);
            liLongitube = [];
            liLattitube = [];
            liAltitube = [];
        }, 'json'); // map check 
    } else {
        $('#lattitube_list').html("");
        $('#longitube_list').html("");
        $('#altitube_list').html("");
        $('#gps_checked_add' + gpsID).removeClass("checked").addClass("");
        $("#gps_checked" + gpsID).prop('checked', false);

        $('#lattitube_name').val("Lattitube");
        $('#longitube_name').val("Longitube");
        $('#altitube_name').val("Altitube");
      
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
        //$('#lattitube_select li').css('widht', '93%');
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

