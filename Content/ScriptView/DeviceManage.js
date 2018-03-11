
var dvcID, DeviceName,timeID;

//$(document).ready(function () { 

$(document).on('click touchend', '.device_settings', function () { // add device setting open
    dvcID = $(this).closest($(".foo")).attr("id");
    DeviceName = $('.header' + dvcID).text();
    if (DeviceName.length > 15) {
        DeviceName = DeviceName.substr(0, DeviceName.length - (DeviceName.length - 12)) + '...';
    }

    //$.post("/DeviceGroup/DeviceManegeSetting", { dvcID: dvcID, DeviceName: DeviceName}, function (Response) {
    //    $('#search_time_interval').html("");
    //    $('#search_time_interval').html(Response);
    //}, 'text');

    //$('#myModal').modal('show');  

    $.post("/DeviceGroup/WalkMib", {}, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
        
    }, 'text');
    
    });

//    $('#myModal').on("click", function () {
//        $('#myModal').css("display", "none");
//});


var handled = false; var width;                
$('body').on('click touchend', '#select_time', function (e) { // time intervel search
    width = $(this).width();
      timeID = $(this).attr("value");
    if (e.type == "touchend") {
        handled = true;
        $('#select_time_list' + timeID).css({ 'width': width + 0 }).toggle();
        $('.select_time' + timeID + ' li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#select_time_list' + timeID).css({ 'width': width + 0 }).toggle();
            //$.post("/DeviceGroup/ScanIntervalDvc", {}, function (Response) {
            //    $('#interva_partial' + timeID).html("");
            //    $('#interva_partial' + timeID).html(Response);
            //}, 'text');
          
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend', '.search_time_interval li', function () { // time interval add
    var value = $(this).attr("value");
    $('#interval' + timeID).text(value);
});

    $('body').on('click touchend', '#select_list', function (e) {  // search number  list information 
        width = $(this).width();
        alert("width");
    if (e.type == "touchend") {
        handled = true;
        $('#select_list_information').css({ 'width': width + 0 }).toggle();
        $('.select_list li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#select_list_information').css({ 'width': width + 0 }).toggle();
            //$.post("/DeviceGroup/ScanIntervalDvc", {}, function (Response) {
            //    $('#interva_partial' + timeID).html("");
            //    $('#interva_partial' + timeID).html(Response);
            //}, 'text');

        }
        else {
            handled = false;
        }
});

    //$('body').on('click touchend', '#setButtons li', function () {
    //    var setID = $(this).attr("value");
    //    alert("set");

    //});
//});

$('body').on('click touched', '.map_check div', function () {
    var mapID = $(this).attr("id");
    if ($('#map_checked' + mapID).is(':checked') == false) {
        $('#map_checked_add' + mapID).removeClass("").addClass("checked");
        $("#map_checked" + mapID).prop('checked', true);
    } else {
        $('#map_checked_add' + mapID).removeClass("checked").addClass("");
        $("#map_checked" + mapID).prop('checked', false);
    }
});
$('body').on('click touched', '.log_check div', function () {
    var logID = $(this).attr("id");
    if ($('#log_checked' + logID).is(':checked') == false) {
        $('#log_checked_add' + logID).removeClass("").addClass("checked");
        $("#log_checked" + logID).prop('checked', true);
    } else {
        $('#log_checked_add' + logID).removeClass("checked").addClass("");
        $("#log_checked" + logID).prop('checked', false);
    }
});