
var dvcID, DeviceName, timeID, SetOID, SetValue, SearchName, IP,Port,Version;

var CheckArray = new Array();
var walkArray = new Array();
var array = new Array();
var ChekedList = new Array();
var TimeChange = new Array();
var tm = new Array();
var UnChecked = new Array();
//$(document).ready(function () { 

$(document).on('click touchend', '.device_settings', function () { // add device setting open
    dvcID = $(this).closest($(".foo")).attr("id");
    DeviceName =$('.device_header' + dvcID).text();
    //if (DeviceName.length > 15) {
    //    DeviceName = DeviceName.substr(0, DeviceName.length - (DeviceName.length - 12)) + '...';
    //} 
    $.post("/DeviceGroup/LoadMib", { DeviceName: DeviceName }, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
    }, 'text');
    
    });

$('#walk_send').click(function () { // device walk ip port version
    IP = $('#tower_ip').val();
    Port = $('#tower_port').val();
    Version = $('#walk_version').text();
    $.post("/DeviceGroup/WalkSend", { IP: IP, Port: Port, Version: Version }, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);

        $('#walk_checked_add').removeClass("").addClass("checked");
        $("#walk_checked").prop('checked', true);
    });
});


var handled = false; var width;                
$('body').on('click touchend', '#select_time', function (e) { // time intervel search
    width = 0;
      timeID = $(this).attr("value");
    if (e.type == "touchend") {
        handled = true;
        $('#select_time_list' + timeID).css({ 'width': width + 0 }).toggle();
        $('.select_time' + timeID + ' li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#select_time_list' + timeID).css({ 'width': width + 0 }).toggle();
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend', '.search_time_interval li', function () { // time interval add
    var value = $(this).attr("value");
    $('#interval' + timeID).text(value);
});

$('body').on('click touchend', '#select_list', function (e) {// search number  list information
      width = 0;
    if (e.type == "touchend") {
        handled = true;
        $('#select_list_information').css({ 'width': width + 0 }).toggle();
        $('.select_list li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#select_list_information').css({ 'width': width + 0 }).toggle();
        }
        else {
            handled = false;
        }
});
$('body').on('click touchend', '.walk_list_search li', function () { // walk list add
    var pageList = $(this).attr("value");
    $('#walk_list').text(pageList);
    $.post("/DeviceGroup/PageList", { pageList: pageList, ChekedList: ChekedList, TimeChange: TimeChange, UnChecked: UnChecked}, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
    });
});


$('body').on('click touchend', '#select_version', function (e) { //version
    width = 0;
    if (e.type == "touchend") {
        handled = true;
        $('#select_list_version').css({ 'width': width + 0 }).toggle();
        $('.select_version li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#select_list_version').css({ 'width': width + 0 }).toggle();
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend','.walk_list_version li', function () { // time interval add
    //var value = $(this).attr("value");
    //var timeID = $(this).attr("id");
    $('#walk_version').text($(this).attr("value"));

    tm.push($(this).attr("id"));
    tm.push($(this).attr("value"));
    TimeChange.push(tm);
    tm = [];

});

    $('body').on('click touchend', '#setButtons', function () { // open modal set and send set 
        var setID = $(this).attr("value");
        $('#set_description').text($('#description' + setID).text());
        $('#set_oid').text($('#description' + setID).attr("value"));
        $('#set_value').text($('#Value' + setID).text());
        $('#set_datatype').text($('#Value' + setID).attr("value"));
    });

    $('#SendSet').click(function () { // set send value
        SetOID = $('#set_oid').text();
        SetValue = $('#set_send_value').val();
        $.post("/DeviceGroup/SetSend", { SetOID: SetOID, SetValue: SetValue }, function () {
        },'json');
    });

    $('#walk_search_click').click(function () { // search description value
        SearchName = $('#description_value_search').val();
        $.post("/DeviceGroup/WalkSearchList", { SearchName: SearchName, ChekedList: ChekedList, TimeChange: TimeChange, UnChecked: UnChecked}, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        },'text');
    //$.ajax({
    //    type: 'POST',
    //    data: {
    //        'SearchName': SearchName, ChekedList: ChekedList, TimeChange: TimeChange, UnChecked: UnChecked
    //    },
    //    dataType: 'text',
    //    url: '/Walk/SearchList',
    //    success: function (Response) {
    //        $('#walkdraw').html("");
    //        $('#walkdraw').html(Response);
    //        window.location.href = "/Walk/Index";

    //    }
    //});
});
$('body').on('click touched', '.walk_check div', function () { // walk unchecked , show mib file  
    var mapID = $(this).attr("id");
    if ($('#walk_checked').is(':checked') == false) {
        //$('#walk_checked_add').removeClass("").addClass("checked");
        //$("#walk_checked").prop('checked', true);
    } else {
        $('#walk_checked_add').removeClass("checked").addClass("");
        $("#walk_checked").prop('checked', false);
        $.post("/DeviceGroup/LoadMib", { DeviceName: DeviceName }, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    }
});

$('body').on('click touched', '.map_check div', function () { // map click checked 
    var mapID = $(this).attr("id");
    if ($('#map_checked' + mapID).is(':checked') == false) {
        $('#map_checked_add' + mapID).removeClass("").addClass("checked");
        $("#map_checked" + mapID).prop('checked', true);
    } else {
        $('#map_checked_add' + mapID).removeClass("checked").addClass("");
        $("#map_checked" + mapID).prop('checked', false);
    }
});
$('body').on('click touched', '.log_check div', function () { // log checked preset 
    var logID = $(this).attr("id");
    if ($('#log_checked' + logID).is(':checked') == false) {
        ChekedList.push(logID);

        $('#log_checked_add' + logID).removeClass("").addClass("checked");
        $("#log_checked" + logID).prop('checked', true);
    } else {
        UnChecked.push(logID);

        $('#log_checked_add' + logID).removeClass("checked").addClass("");
        $("#log_checked" + logID).prop('checked', false);
    }
});