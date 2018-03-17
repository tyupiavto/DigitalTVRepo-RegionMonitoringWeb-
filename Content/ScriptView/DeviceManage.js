
var dvcID, DeviceName,intervalNumber, intervalID, SetOID, SetValue, SearchName, IP, Port, Version, chechkID, unChechkID, presetName, IpAddress, second;

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
    $('#myModal').modal();
    $.post("/DeviceGroup/LoadMib", { DeviceName: DeviceName }, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
    }, 'text');
    });

$('#walk_send').click(function () { // device walk ip port version
    IP = $('#tower_ip').val();
    Port = $('#tower_port').val();
    Version = $('#walk_version').text();
    $('#load_walk').css("display", "block");

    $.post("/DeviceGroup/WalkSend", { IP: IP, Port: Port, Version: Version }, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
        $('#load_walk').css("display", "none");
        $('#walk_checked_add').removeClass("").addClass("checked");
        $("#walk_checked").prop('checked', true);
    });
});


var handled = false; var width;                
$('body').on('click touchend', '#select_time', function (e) { // time intervel search
    width = 0;
    intervalID = $(this).attr("value");
    if (e.type == "touchend") {
        handled = true;
        $('#select_time_list' + intervalID).css({ 'width': width + 0 }).toggle();
        $('.select_time' + intervalID + ' li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $.post("/DeviceGroup/ViewAddInterval", {}, function (Response) {
                $('.search_time_interval').html("");
                $('.search_time_interval').html(Response);
                $('#select_time_list' + intervalID).css({ 'width': width + 0 }).toggle();
            }, 'text');           
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend', '.search_time_interval li', function () { // time interval add
    var Interval = $(this).attr("value");
    $('#interval' + intervalID).text(Interval);
    $.post("/DeviceGroup/IntervalSearch", { intervalID: intervalID, Interval: Interval }, function () { }, 'json');
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
    $.post("/DeviceGroup/PageList", { pageList: pageList}, function (Response) {
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

$('body').on('click touchend', '#preset_add_remove', function (e) { //preset add remove list
    width = 145;
    if (e.type == "touchend") {
        handled = true;
        $('#preset_list_remove').css({ 'width': width + 0 }).toggle();
        $('#preset_add_remove li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#preset_list_remove').css({ 'width': width + 0 }).toggle();
            $.post("/DeviceGroup/PresetListName", {}, function (Response) {
                $('.preset_list_remove').html("");
                $('.preset_list_remove').html(Response);
            },'text');
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend', '.preset_list_remove li', function () { // selected  preset  click add 
    presetSearchName = $(this).children().attr("value");
    $('#preset_name').val($(this).children().attr("value"));
    $.post("/DeviceGroup/PresetSearch", { presetSearchName: presetSearchName}, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
    },'text');
});

$('body').on('click touchend','.removePreset' , function () {
    presetName = $(this).attr("value");
    $.post("/DeviceGroup/PresetRemove", { presetName: presetName}, function (Response) {
        $('.preset_list_remove').html("");
        $('.preset_list_remove').html(Response);
    },'text');
});

$('body').on('click touchend', '#inerval_add_remove', function (e) { //interval add removel list
    width = 145;
    if (e.type == "touchend") {
        handled = true;
        $('#interval_list_remove').css({ 'width': width + 0 }).toggle();
        $('#inerval_add_remove li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#interval_list_remove').css({ 'width': width + 0 }).toggle();
            $.post("/DeviceGroup/intervalListView", {}, function (Response) {
                $('.interval_list_remove').html("");
                $('.interval_list_remove').html(Response);
            },'text');
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend', '.interval_list_remove li', function () { //interval default search
    intervalNumber = $(this).children().attr("value");
    $('#interval_number').val(intervalNumber);
    $.post("/DeviceGroup/DefaultIntervalSearch", { intervalNumber: intervalNumber}, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
    }, 'text');
});

$('body').on('click touchend', '.removeInterval', function () {

    intervalID = $(this).attr("id");
    $.post("/DeviceGroup/IntervalRemove", { intervalID: intervalID }, function (Response) {
        $('.interval_list_remove').html("");
        $('.interval_list_remove').html(Response);
    },'text');
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
        $.post("/DeviceGroup/WalkSearchList", { SearchName: SearchName}, function (Response) {
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

        chechkID = mapID;
        $.post("/DeviceGroup/CheckMap", { chechkID: chechkID }, function () { }, 'json'); // map check 
    } else {
        unChechkID = mapID;
        $.post("/DeviceGroup/UncheckMap", { unChechkID: unChechkID }, function () { }, 'json'); // map uncheck 

        $('#map_checked_add' + mapID).removeClass("checked").addClass("");
        $("#map_checked" + mapID).prop('checked', false);
    }
});
$('body').on('click touched', '.log_check div', function () { // log checked preset 
    var logID = $(this).attr("id");
    if ($('#log_checked' + logID).is(':checked') == false) {
        chechkID = logID;
        $.post("/DeviceGroup/CheckLog", { chechkID: chechkID }, function () { }, 'json'); // log check 

        $('#log_checked_add' + logID).removeClass("").addClass("checked");
        $("#log_checked" + logID).prop('checked', true);
    } else {
        unChechkID = logID;
        $.post("/DeviceGroup/UncheckLog", { unChechkID: unChechkID }, function () { }, 'json'); // log uncheck 
        $('#log_checked_add' + logID).removeClass("checked").addClass("");
        $("#log_checked" + logID).prop('checked', false);
    }
});

$('#preset_save').click(function () {
    presetName = $('#preset_name').val();
    IpAddress = $('#tower_ip').val();

    $.post("/DeviceGroup/PresetSave", { presetName: presetName, IpAddress: IpAddress}, function () {
        $('#preset_name').val("");
    },'json');
});

$('#interval_add').click(function () { /// interval save 
    second = $('#interval_number').val();
    $.post("/DeviceGroup/IntervalAdd", { second: second}, function (Response) {
        $('.interval_list_remove').html("");
        $('.interval_list_remove').html(Response);
        $('#interval_number').val("");
    },'text');
});
