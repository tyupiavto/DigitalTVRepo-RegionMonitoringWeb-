
var deviceID, dividedMultiply, DeviceName, intervalNumber, intervalID, SetOID, SetValue, SearchName, IP, Port, Version, chechkID, unChechkID, presetName, IpAddress, second, communityRead, GpsID, towerID, towerName, defineWalk, dataType, setID;
var checkMap = false, checkLog = false;
var resolutionWidht = screen.width;
var resolutionHeight = screen.height;
var handleOne = $("#slider-range-one");
var handleTwo = $("#slider-range-two");
var handleThree = $("#slider-range-three");
var handleFour = $("#slider-range-four");
var handleFive = $("#slider-range-five");

var leftvalue, oidName, description, walkOid,myDescriptionID,myDescription;

$(document).on('click touchend', '.device_settings', function () { // add device setting open
    deviceID = $(this).closest($(".foo")).attr("id");
    DeviceName = $('.device_header' + deviceID).attr("value");

    $('#device_settings_name').text($('.tower_name' + deviceID).attr("title"));
    towerName = $('.tower_name' + deviceID).attr("title");
    towerID = $('.tower_name' + deviceID).parent().parent().parent().parent().attr("id");
    $('#preset_name').val("");
    defineWalk = 1;
    $('#map_checked_add_all').removeClass("checked").addClass("");
    $("#map_checked_all").prop('checked', false);
    $('#log_checked_add_all').removeClass("checked").addClass("");
    $("#log_checked_all").prop('checked', false);
    $.post("/DeviceGroup/LoadMib", { DeviceName: DeviceName, towerName: towerName, deviceID: deviceID, defineWalk: defineWalk }, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
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
    $.post("/DeviceGroup/WalkSend", { IP: IP, Port: Port, Version: Version, communityRead: communityRead, towerName: towerName, DeviceName: DeviceName, deviceID: deviceID, Version: Version }, function (Response) {
        $('#load_walk').css("display", "none");

        $('#walk_checked_add').removeClass("").addClass("checked");
        $("#walk_checked").prop('checked', true);

        $('#device_settings').html("");
        $('#device_settings').html(Response);
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
    $.post("/GetNext/IntervalSearch", { intervalID: intervalID, Interval: Interval, towerName: towerName, deviceID: deviceID, towerID: towerID }, function () { }, 'json');
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
    $.post("/DeviceGroup/PageList", { pageList: pageList }, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
        $('#walk_list').text(pageList);
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

$('body').on('click touchend', '.walk_list_version li', function () { // time interval add
    $('#walk_version').text($(this).attr("value"));
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
            $.post("/DeviceGroup/PresetListName", { DeviceName: DeviceName }, function (Response) {
                $('.preset_list_remove').html("");
                $('.preset_list_remove').html(Response);
            }, 'text');
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend', '.preset_list_remove li', function () { // selected  preset  click add 
    presetSearchName = $(this).children().attr("value");
    $('#preset_name').val($(this).children().attr("value"));
    //$.post("/DeviceGroup/PresetSearch", { presetSearchName: presetSearchName}, function (Response) {
    //    $('#device_settings').html("");
    //    $('#device_settings').html(Response);
    //},'text');
});

$('body').on('click touched', '#preset_send', function () {
    $.post("/DeviceGroup/PresetSearch", { presetSearchName: presetSearchName }, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
    }, 'text');
});

$('body').on('click touchend', '.removePreset', function () {
    presetName = $(this).attr("value");
    $.post("/DeviceGroup/PresetRemove", { presetName: presetName }, function (Response) {
        $('.preset_list_remove').html("");
        $('.preset_list_remove').html(Response);
        $('#preset_name').val("");
    }, 'text');
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
            }, 'text');
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend', '.interval_list_remove li', function () { //interval default search
    intervalNumber = $(this).children().attr("value");
    $('#interval_number').val(intervalNumber);
    $.post("/DeviceGroup/DefaultIntervalSearch", { intervalNumber: intervalNumber }, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
    }, 'text');
});

$('body').on('click touchend', '.removeInterval', function () {

    intervalID = $(this).attr("id");
    $.post("/DeviceGroup/IntervalRemove", { intervalID: intervalID }, function (Response) {
        $('.interval_list_remove').html("");
        $('.interval_list_remove').html(Response);
    }, 'text');
});

$('body').on('click touchend', '#setButtons', function () { // open modal set and send set 
    setID = $(this).attr("value");
    $('#set_description').text($('#description' + setID).text());
    $('#set_oid').text($('#description' + setID).attr("value"));
    $('#set_value').text($('#Value' + setID).text());
    $('#set_datatype').text($('#Value' + setID).attr("value"));
});

$('#SendSet').click(function () { // set send value
    SetOID = $('#set_oid').text();
    SetValue = $('#set_send_value').val();
    communityWrite = $('#write_community').val();
    IP = $('#tower_ip').val();
    dataType = $('#set_datatype').text();
    Version = $('#walk_version').text();
    Port = $('#tower_port').val();
    $.post("/DeviceGroup/SetSend", { IP: IP, Port: Port, SetOID: SetOID, SetValue: SetValue, communityWrite: communityWrite, dataType: dataType, Version: Version }, function (Response) {
        $('#Value' + setID).text(Response);
        $('#Value' + setID).css("color", "#9dff75");
        $('#description' + setID).css("color", "#9dff75");
        $('#oidname' + setID).css("color", "#9dff75");
        $('#set_send_value').val("");
    }, 'json');
});

// $('#walk_search_click').click(function () { // search description value
$('body').on('keyup', '#description_value_search', function () {
    SearchName = $('#description_value_search').val();

    $('#map_checked_add_all').removeClass("checked").addClass("");
    $("#map_checked_all").prop('checked', false);
    $('#log_checked_add_all').removeClass("checked").addClass("");
    $("#log_checked_all").prop('checked', false);

    $.post("/DeviceGroup/WalkSearchList", { SearchName: SearchName }, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
    }, 'text');

});
$('body').on('click touched', '.walk_check div', function () { // walk unchecked , show mib file  
    var mapID = $(this).attr("id");

    if ($('#walk_checked').is(':checked') == false) {
        $('#walk_checked_add').removeClass("").addClass("checked");
        $("#walk_checked").prop('checked', true);
        defineWalk = 1;

        $.post("/DeviceGroup/LoadMib", { DeviceName: DeviceName, towerID: towerID, deviceID: deviceID, defineWalk: defineWalk }, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    } else {
        $('#walk_checked_add').removeClass("checked").addClass("");
        $("#walk_checked").prop('checked', false);
        defineWalk = 0;
        $.post("/DeviceGroup/LoadMib", { DeviceName: DeviceName, towerID: towerID, deviceID: deviceID, defineWalk: defineWalk }, function (Response) {
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
        $.post("/GetNext/CheckMap", { chechkID: chechkID, towerName: towerName, deviceID: deviceID, towerID: towerID }, function () { }, 'json'); // map check 
    } else {
        unChechkID = mapID;
        $.post("/GetNext/UncheckMap", { unChechkID: unChechkID, towerName: towerName, deviceID: deviceID, towerID: towerID }, function () { }, 'json'); // map uncheck 

        $('#map_checked_add' + mapID).removeClass("checked").addClass("");
        $("#map_checked" + mapID).prop('checked', false);
    }
});

$('body').on('click touched', '.log_check div', function () { // log checked preset 
    var logID = $(this).attr("id");
    towerName = $('#device_settings_name').text();
    if ($('#log_checked' + logID).is(':checked') == false) {
        chechkID = logID;
        $.post("/GetNext/CheckLog", { chechkID: chechkID, towerName: towerName, deviceID: deviceID, towerID: towerID }, function () { }, 'json'); // log check 

        $('#log_checked_add' + logID).removeClass("").addClass("checked");
        $("#log_checked" + logID).prop('checked', true);
    } else {
        unChechkID = logID;
        $.post("/GetNext/UncheckLog", { unChechkID: unChechkID, towerName: towerName, deviceID: deviceID, towerID: towerID }, function () { }, 'json'); // log uncheck 
        $('#log_checked_add' + logID).removeClass("checked").addClass("");
        $("#log_checked" + logID).prop('checked', false);
    }
});

$('body').on('click touched', '.map_check_all div', function () { // Map check select all
    var mapID = $(this).attr("id");

    if ($('#map_checked_all').is(':checked') == false) {

        $('#map_checked_add_all').removeClass("").addClass("checked");
        $("#map_checked_all").prop('checked', true);

        defineWalk = 1;
        checkMap = true;
        $.post("/DeviceGroup/SelectAllLogMap", { checkMap: checkMap, checkLog: checkLog }, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    } else {
        checkMap = false;
        $('#map_checked_add_all').removeClass("checked").addClass("");
        $("#map_checked_all").prop('checked', false);
        defineWalk = 0;
        $.post("/DeviceGroup/SelectAllLogMap", { checkMap: checkMap, checkLog: checkLog }, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    }
});

$('body').on('click touched', '.log_check_all div', function () { // Log check select all
    var mapID = $(this).attr("id");

    if ($('#log_checked_all').is(':checked') == false) {

        $('#log_checked_add_all').removeClass("").addClass("checked");
        $("#log_checked_all").prop('checked', true);
        defineWalk = 1;
        checkLog = true;
        $.post("/DeviceGroup/SelectAllLogMap", { checkMap: checkMap, checkLog: checkLog }, function (Response) {
            $('#device_settings').html("");
            $('#device_settings').html(Response);
        }, 'text');
    }
    else {
        checkLog = false;
        $('#log_checked_add_all').removeClass("checked").addClass("");
        $("#log_checked_all").prop('checked', false);
        defineWalk = 0;
        $.post("/DeviceGroup/SelectAllLogMap", { checkMap: checkMap, checkLog: checkLog }, function (Response) {
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

$('body').on('click touched', '.mib_search_time_interval li', function () { // time interval add
    var Interval = $(this).attr("value");
    $('#interval' + intervalID).text(Interval);
    IP = $('#tower_ip').val();
    towerName = $('#device_settings_name').text();
    var OidMib = $('#description' + intervalID).attr("value");
    $.post("/DeviceGroup/IntervalSearchMib", { intervalID: intervalID, Interval: Interval, towerName: towerName, deviceID: deviceID, OidMib: OidMib }, function () { }, 'json');
});

var maxlenght = 100;
var minlenght = 0;
var settingID;
function getPercentageChange(oldNumber, newNumber, precent) {
    var decreaseValue = oldNumber * newNumber;

    return (precent / decreaseValue) * 100;
}
var lenght = maxlenght + Math.abs(minlenght);
var precent = getPercentageChange(maxlenght, minlenght, 20);

$('body').on('click touched', '.logmapsetting', function () {
    $('#value_logmap_min').val("");
    $('#value_logmap_max').val("");

     settingID= $(this).attr("value");
     oidName = $('#oidname' + settingID).text();
     walkOid = $('#description' + settingID).attr("value");
     description = $('#description' + settingID).text();
     if (description == "Is Not Description") {
        description = '';
     }

    $.post("/DeviceGroup/LogMapExistingValue", { towerName: towerName, deviceID: deviceID, oidName: oidName, description: description, walkOid: walkOid, settingID:settingID }, function (Response) {
        if (Response != '') {
            maxlenght = parseFloat(Response.TwoEndError);
            minlenght = parseFloat(Response.OneStartError);
            slideLogMapSetting();

            handleOne.slider('values', 0, Response.OneStartError);
            handleOne.slider('values', 1, Response.OneEndError);
            handleTwo.slider('values', 0, Response.OneEndError);
            handleTwo.slider('values', 1, Response.StartCorrect);
            handleThree.slider('values', 0, Response.StartCorrect);
            handleThree.slider('values', 1, Response.EndCorrect);
            handleFour.slider('values', 0, Response.EndCorrect);
            handleFour.slider('values', 1, Response.TwoEndCrash);
            handleFive.slider('values', 0, Response.TwoStartError);
            handleFive.slider('values', 1, Response.TwoEndError);
            $('#divided_multiply').val(Response.DivideMultiply);
        }
        else {
            slideLogMapSetting();
        }
    });
});

$('body').on('click touched', '#value_logmap_button', function () {
    maxlenght = parseFloat($('#value_logmap_max').val());
    minlenght = parseFloat($('#value_logmap_min').val());
    slideLogMapSetting();
});
function slideLogMapSetting() {
    handleOne.slider({
        range: true,
        min: minlenght,
        max: maxlenght,
        step: 0.1,
        values: [minlenght, maxlenght / 5],
        slide: function (event, ui) {
            handleTwo.slider('values', 0, ui.values[1]);
        },
    }).slider("float", {
        handle: true,
        pips: true,
        labels: false,
        prefix: "",
        suffix: ""
    });

    handleTwo.slider({
        range: true,
        min: minlenght,
        max: maxlenght,
        step: 0.1,
        values: [maxlenght / 5, maxlenght / 2.5],
        slide: function (event, ui) {
            handleThree.slider('values', 0, ui.values[1]);
            handleOne.slider('values', 1, ui.values[0]);
        }
    }).slider("float", {
        handle: true,
        pips: true,
        labels: false,
        prefix: "",
        suffix: ""
    });


    handleThree.slider({
        range: true,
        min: minlenght,
        max: maxlenght,
        step: 0.1,
        values: [maxlenght / 2.5, maxlenght / 1.6],
        slide: function (event, ui) {
            handleFour.slider('values', 0, ui.values[1]);
            handleTwo.slider('values', 1, ui.values[0]);
        }
    }).slider("float", {
        handle: true,
        pips: true,
        labels: false,
        prefix: "",
        suffix: ""
    });

    handleFour.slider({
        range: true,
        min: minlenght,
        max: maxlenght,
        step: 0.1,
        values: [maxlenght / 1.6, maxlenght / 1.25],
        slide: function (event, ui) {
            handleFive.slider('values', 0, ui.values[1]);
            handleThree.slider('values', 1, ui.values[0]);
        }
    }).slider("float", {
        handle: true,
        pips: true,
        labels: false,
        prefix: "",
        suffix: ""
    });

    handleFive.slider({
        range: true,
        min: minlenght,
        max: maxlenght,
        step: 0.1,
        values: [maxlenght / 1.25, maxlenght],
        slide: function (event, ui) {
            handleFour.slider('values', 1, ui.values[0]);
        }
    }).slider("float", {
        handle: true,
        pips: true,
        labels: false,
        prefix: "",
        suffix: ""
    });
    var oneStartError, oneEndError, startCorrect, endCorrect, oneStartCrash, oneEndCrash, twoStartError, twoEndError, twoStartCorrect, twoEndCorrect, twoStartCrash, twoEndCrash;

    $('body').on('click touched', '#sendLogMapSetting', function () {

        oneStartError = $('#slider-range-one span:nth-child(2) .ui-slider-tip').text();
        oneEndError = $('#slider-range-one span:nth-child(3) .ui-slider-tip').text();
        oneStartCrash = $('#slider-range-one span:nth-child(3) .ui-slider-tip').text();
        oneEndCrash = $('#slider-range-two span:nth-child(3) .ui-slider-tip').text();
        startCorrect = $('#slider-range-three span:nth-child(2) .ui-slider-tip').text();
        endCorrect = $('#slider-range-three span:nth-child(3) .ui-slider-tip').text();
        twoStartError = $('#slider-range-five span:nth-child(2) .ui-slider-tip').text();
        twoEndError = $('#slider-range-five span:nth-child(3) .ui-slider-tip').text();
        twoStartCrash = $('#slider-range-three span:nth-child(3) .ui-slider-tip').text();
        twoEndCrash = $('#slider-range-four span:nth-child(3) .ui-slider-tip').text();
        dividedMultiply = $('#divided_multiply').val();
        $.post("/DeviceGroup/LogMapSetting", {
            towerName: towerName, settingID: settingID, oneStartError: oneStartError, oneEndError: oneEndError,
            oneStartCrash: oneStartCrash, oneEndCrash: oneEndCrash,
            startCorrect: startCorrect, endCorrect: endCorrect,
            twoStartError: twoStartError, twoEndError: twoEndError,
            twoStartCrash: twoStartCrash, twoEndCrash: twoEndCrash,
            dividedMultiply: dividedMultiply}, function () { }, 'post');
    });
}

$('#opneIPLink').click(function () {
    $('#opneIPLink a').attr("href", 'http://' + $('#tower_ip').val());
});

$('body').on('focus', '.mydescriptioninput', function () {
    walkID=$(this).attr("name");
});

$('body').on('focusout', '.mydescriptioninput', function () {
    myDescription = $('#my_description_walk' + walkID).val();
    $.post("/DeviceGroup/MyDescriptionAdd", { myDescription, walkID, towerName, deviceID }, function () {},'json');
});

