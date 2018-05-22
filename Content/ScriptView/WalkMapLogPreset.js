﻿
var deviceID, DeviceName, intervalNumber,intervalID, SetOID, SetValue, SearchName, IP, Port, Version, chechkID, unChechkID, presetName, IpAddress, second, communityRead, GpsID, towerID, towerName,defineWalk,dataType;
var resolutionWidht = screen.width;
var resolutionHeight = screen.height;
$(document).on('click touchend', '.device_settings', function () { // add device setting open
    deviceID = $(this).closest($(".foo")).attr("id");
    DeviceName = $('.device_header' + deviceID).attr("value");

    $('#device_settings_name').text($('.tower_name' + deviceID).attr("title"));
    towerID = $('.tower_name' + deviceID).attr("title");
    $('#preset_name').val("");
    defineWalk = 1;
    $.post("/DeviceGroup/LoadMib", { DeviceName: DeviceName, towerID: towerID, deviceID: deviceID, defineWalk: defineWalk }, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);

        $('.device_setting_style').css('min-width', resolutionWidht - 600 + 'px');
        $('.device_setting_style').css('height', resolutionHeight - 240 + 'px');
        $('.scroll_walk').css('width', resolutionWidht - 607 + 'px');
        $('.scroll_walk').css('height', resolutionHeight - 340 + 'px');
        $('.scroll_walk').css('max-height', resolutionHeight -340 + 'px');

    }, 'text');
    });

$('#walk_send').click(function () { // device walk ip port version
    IP = $('#tower_ip').val();
    Port = $('#tower_port').val();
    Version = $('#walk_version').text();
    defineWalk = 1;
    DeviceName = $('.device_header' + deviceID).attr("value");
    var towerName = $('#device_settings_name').text();
    $('#load_walk').css("display", "block");
    communityRead = $('#read_community').val();
    $.post("/DeviceGroup/WalkSend", { IP: IP, Port: Port, Version: Version, communityRead: communityRead, towerName: towerName, DeviceName: DeviceName, deviceID: deviceID, Version:Version}, function (Response) {
        $('#load_walk').css("display", "none");

        $('#walk_checked_add').removeClass("").addClass("checked");
        $("#walk_checked").prop('checked', true);

        $('#device_settings').html("");
        $('#device_settings').html(Response);

        $('.device_setting_style').css('min-width', resolutionWidht - 600 + 'px');
        $('.device_setting_style').css('height', resolutionHeight - 240 + 'px');
        $('.scroll_walk').css('width', resolutionWidht - 607 + 'px');
        $('.scroll_walk').css('height', resolutionHeight - 340 + 'px');
        $('.scroll_walk').css('max-height', resolutionHeight - 340 + 'px');
    });
});


var handled = false; var width;                
$('body').on('click touchend', '#select_time', function (e) { // time intervel search
    //width = 0;
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
                $('#select_time_list' + intervalID).css('width', '100%').toggle();
            }, 'text');           
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend', '.search_time_interval li', function () { // time interval add
    var Interval = $(this).attr("value");
    $('#interval' + intervalID).text(Interval);
    IP = $('#tower_ip').val();
    towerName = $('#device_settings_name').text();
    $.post("/DeviceGroup/IntervalSearch", { intervalID: intervalID, Interval: Interval, towerName: towerName, deviceID: deviceID }, function () { }, 'json');
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
   // $('#walk_list').text(pageList);
    $.post("/DeviceGroup/PageList", { pageList: pageList}, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
        $('#walk_list').text(pageList);

        $('.device_setting_style').css('min-width', resolutionWidht - 600 + 'px');
        $('.device_setting_style').css('height', resolutionHeight - 240 + 'px');
        $('.scroll_walk').css('width', resolutionWidht - 607 + 'px');
        $('.scroll_walk').css('height', resolutionHeight - 340 + 'px');
        $('.scroll_walk').css('max-height', resolutionHeight - 340 + 'px');
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
    width = 101;
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

        $('.device_setting_style').css('min-width', resolutionWidht - 600 + 'px');
        $('.device_setting_style').css('height', resolutionHeight - 240 + 'px');
        $('.scroll_walk').css('width', resolutionWidht - 607 + 'px');
        $('.scroll_walk').css('height', resolutionHeight - 340 + 'px');
        $('.scroll_walk').css('max-height', resolutionHeight - 340 + 'px');
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
    width = 101;
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
    var setID = $(this).attr("value");
        SetOID = $('#set_oid').text();
        SetValue = $('#set_send_value').val();
        communityWrite = $('#write_community').val();
        IP = $('#tower_ip').val();
        dataType = $('#set_datatype').text();
        Version = $('#walk_version').text();
        Port = $('#tower_port').val();
        $.post("/DeviceGroup/SetSend", { IP: IP, Port: Port, SetOID: SetOID, SetValue: SetValue, communityWrite: communityWrite, dataType: dataType, Version: Version }, function (Response) {
            $('#set_send_value').val("");
            $('#Value' + setID).text(Response);
            $('#Value' + setID).css("color", "#9dff75");
            $('#description' + setID).css("color", "#9dff75");
            $('#oidname' + setID).css("color", "#9dff75");
        },, 'json');
    });

    $('#walk_search_click').click(function () { // search description value
        SearchName = $('#description_value_search').val();
        $.post("/DeviceGroup/WalkSearchList", { SearchName: SearchName}, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        },'text');

});
$('body').on('click touched', '.walk_check div', function () { // walk unchecked , show mib file  
    var mapID = $(this).attr("id");
    
    if ($('#walk_checked').is(':checked') == false) {
        $('#walk_checked_add').removeClass("").addClass("checked");
        $("#walk_checked").prop('checked', true);
         defineWalk = 1;

        $.post("/DeviceGroup/LoadMib", { DeviceName: DeviceName, towerID: towerID, deviceID: deviceID, defineWalk: defineWalk  }, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    } else {
        $('#walk_checked_add').removeClass("checked").addClass("");
        $("#walk_checked").prop('checked', false);
        defineWalk = 0;
        $.post("/DeviceGroup/LoadMib", { DeviceName: DeviceName, towerID: towerID, deviceID: deviceID, defineWalk: defineWalk  }, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    }
});

$('body').on('click touched', '.map_check div', function () { // map click checked 
    var mapID = $(this).attr("id");
    towerName = $('#device_settings_name').text();
    IP = $('#tower_ip').val();
    if ($('#map_checked' + mapID).is(':checked') == false) {
        $('#map_checked_add' + mapID).removeClass("").addClass("checked");
        $("#map_checked" + mapID).prop('checked', true);

        chechkID = mapID;
        $.post("/DeviceGroup/CheckMap", { chechkID: chechkID, towerName: towerName, deviceID: deviceID }, function () { }, 'json'); // map check 
    } else {
        unChechkID = mapID;
        $.post("/DeviceGroup/UncheckMap", { unChechkID: unChechkID, towerName: towerName, deviceID: deviceID }, function () { }, 'json'); // map uncheck 

        $('#map_checked_add' + mapID).removeClass("checked").addClass("");
        $("#map_checked" + mapID).prop('checked', false);
    }
});

$('body').on('click touched', '.log_check div', function () { // log checked preset 
    var logID = $(this).attr("id");
    towerName = $('#device_settings_name').text();
    if ($('#log_checked' + logID).is(':checked') == false) {
        chechkID = logID;
        $.post("/DeviceGroup/CheckLog", { chechkID: chechkID, towerName: towerName, deviceID: deviceID }, function () { }, 'json'); // log check 

        $('#log_checked_add' + logID).removeClass("").addClass("checked");
        $("#log_checked" + logID).prop('checked', true);
    } else {
        unChechkID = logID;
        $.post("/DeviceGroup/UncheckLog", { unChechkID: unChechkID, towerName: towerName, deviceID: deviceID }, function () { }, 'json'); // log uncheck 
        $('#log_checked_add' + logID).removeClass("checked").addClass("");
        $("#log_checked" + logID).prop('checked', false);
    }
});

$('body').on('click touched', '.map_check_all div', function () { // Map check select all
    var mapID = $(this).attr("id");

    if ($('#map_checked_all').is(':checked') == false) {

        if ($('#log_checked_all').is(':checked') == true) { // check log remove
            $('#log_checked_add_all').removeClass("checked").addClass("");
            $("#log_checked_all").prop('checked', false);
        }

        $('#map_checked_add_all').removeClass("").addClass("checked");
        $("#map_checked_all").prop('checked', true);
        defineWalk = 1;
        var check = true;
        $.post("/DeviceGroup/SelectAllMap", { check:check }, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    } else {
        var check = false;
        $('#map_checked_add_all').removeClass("checked").addClass("");
        $("#map_checked_all").prop('checked', false);
        defineWalk = 0;
        $.post("/DeviceGroup/SelectAllMap", { check: check}, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    }
});

$('body').on('click touched', '.log_check_all div', function () { // Log check select all
    var mapID = $(this).attr("id");

    if ($('#log_checked_all').is(':checked') == false) {

        if ($('#map_checked_all').is(':checked') == true) { // check map remove
            $('#map_checked_add_all').removeClass("checked").addClass("");
            $("#map_checked_all").prop('checked', false);
        }

        $('#log_checked_add_all').removeClass("").addClass("checked");
        $("#log_checked_all").prop('checked', true);
        defineWalk = 1;
        var check = true;
        $.post("/DeviceGroup/SelectAllLog", { check: check }, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    } else {
        var check = false;
        $('#log_checked_add_all').removeClass("checked").addClass("");
        $("#log_checked_all").prop('checked', false);
        defineWalk = 0;
        $.post("/DeviceGroup/SelectAllLog", { check: check }, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    }
});


$('#preset_save').click(function () {
    presetName = $('#preset_name').val();
    IpAddress = $('#tower_ip').val();
    TowerNameID = $('#device_settings_name').text();
    IP = $('#tower_ip').val();
    $.post("/DeviceGroup/PresetSave", { presetName: presetName, IpAddress: IpAddress, TowerNameID: TowerNameID, deviceID: deviceID }, function () {
        $('#preset_name').val("");
    }, 'json');
});

$('#interval_add').click(function () { /// interval save 
    second = $('#interval_number').val();
    $.post("/DeviceGroup/IntervalAdd", { second: second }, function (Response) {
        $('.interval_list_remove').html("");
        $('.interval_list_remove').html(Response);
        $('#interval_number').val("");
    }, 'text');
});

$('body').on('click touchend', '#getButtons', function () { // open modal set and send set 
    var getID = $(this).attr("value");
    var getOid = $('#description' + getID).attr("value");
    IP = $('#tower_ip').val();
    if (IP != "") {
        Port = $('#tower_port').val();
        Version = $('#walk_version').text();
        $('#load_walk').css("display", "block");
        communityRead = $('#read_community').val();
        $.post("/DeviceGroup/Get", { getOid: getOid, Version: Version, communityRead: communityRead, IP: IP, Port: Port }, function (Response) {
            $('#load_walk').css("display", "none");
            $('#Value' + getID).text(Response);
            $('#Value' + getID).css("color", "#9dff75");
            $('#description' + getID).css("color", "#9dff75");
            $('#oidname' + getID).css("color", "#9dff75");
        });
    }
    else {
        alert("Please enter IP");
    }
});

//////////////////////////////////////////////////////////////////////////////////////////// check log minb and search interval
$('body').on('click touched', '.mib_map_check div', function () { // map click checked 
    var mapID = $(this).attr("id");
    towerName = $('#device_settings_name').text();
    var OidMib = $('#description' + mapID).attr("value");
    IP = $('#tower_ip').val();
    if ($('#mib_map_checked' + mapID).is(':checked') == false) {
        $('#mib_map_checked_add' + mapID).removeClass("").addClass("checked");
        $("#mib_map_checked" + mapID).prop('checked', true);

        chechkID = mapID;
        $.post("/DeviceGroup/CheckMapMib", { chechkID: chechkID, towerName: towerName, deviceID: deviceID, OidMib: OidMib }, function () { }, 'json'); // map check 
    } else {
        unChechkID = mapID;
        $.post("/DeviceGroup/UncheckMapMib", { unChechkID: unChechkID, towerName: towerName, deviceID: deviceID, OidMib: OidMib}, function () { }, 'json'); // map uncheck 

        $('#mib_map_checked_add' + mapID).removeClass("checked").addClass("");
        $("#mib_map_checked" + mapID).prop('checked', false);
    }
});

$('body').on('click touched', '.mib_log_check div', function () { // log checked preset 
    var logID = $(this).attr("id");
    towerName = $('#device_settings_name').text();
    var OidMib = $('#description' + logID).attr("value");
    if ($('#mib_log_checked' + logID).is(':checked') == false) {
        chechkID = logID;
        $.post("/DeviceGroup/CheckLogMib", { chechkID: chechkID, towerName: towerName, deviceID: deviceID, OidMib: OidMib}, function () { }, 'json'); // log check 

        $('#mib_log_checked_add' + logID).removeClass("").addClass("checked");
        $("#mib_log_checked" + logID).prop('checked', true);
    } else {
        unChechkID = logID;
        $.post("/DeviceGroup/UncheckLogMib", { unChechkID: unChechkID, towerName: towerName, deviceID: deviceID, OidMib: OidMib}, function () { }, 'json'); // log uncheck 
        $('#mib_log_checked_add' + logID).removeClass("checked").addClass("");
        $("#mib_log_checked" + logID).prop('checked', false);
    }
});

$('body').on('click touchend', '.mib_search_time_interval li', function () { // time interval add
    var Interval = $(this).attr("value");
    $('#interval' + intervalID).text(Interval);
    IP = $('#tower_ip').val();
    towerName = $('#device_settings_name').text();
    var OidMib = $('#description' + intervalID).attr("value");
    $.post("/DeviceGroup/IntervalSearchMib", { intervalID: intervalID, Interval: Interval, towerName: towerName, deviceID: deviceID, OidMib: OidMib }, function () { }, 'json');
});
