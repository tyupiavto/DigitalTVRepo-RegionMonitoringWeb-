var GroupName, deviceName, Name, ManuFacture, Model, Purpose, CountrieName, StateName, CityName;
var deviceGroupList, deviceRemoveID, towerID, deviceName, deviceTopMinimizeID = 0, diagramPresetName, removeDiagramID = 0;

$('#countrie').click(function () {
    $("#countries").animate({ height: 'toggle', opacity: 'toggle' }, 300);
    $('#settingsDiv').html("");
});
$('#tower_menu').click(function () {
    $("#tower_drag").animate({ height: 'toggle', opacity: 'toggle' }, 300);
    $('#settingsDiv').html("");
});
$('#deviceName').on("click touched", "li", function (e) {
    deviceGroupList = $(this).attr("value");
    if (e.target.id == "group_name" + deviceGroupList) {
  
    $('#deviceGroupName' + deviceGroupList).animate({ height: 'toggle', opacity: 'toggle' }, 300);
    $('#settingsDiv').html("");
    $.post("/DeviceGroup/DeviceList", { deviceGroupList: deviceGroupList }, function (Response) {
        $('#deviceGroupName' + deviceGroupList).html("");
        $('#deviceGroupName' + deviceGroupList).html(Response);
        }, 'text');
    }
});
var device_add;
device_add = $('#deviceName');
device_add.on("click", "li span", function (e) {
    var deviceGroupID = $(this).attr("value");
    if (e.target.id == "add_device" + deviceGroupID) {
        deviceName = $('#group_name' + deviceGroupID).text();
        $.post("/DeviceGroup/AddDevice", { deviceName: deviceName, deviceGroupID: deviceGroupID }, function (Response) {
            $('#settingsDiv').html("");
            $('#settingsDiv').html(Response);
        }, 'text');
    }
});

$('#add_group').click(function () {
    GroupName = $('#add_group_name').val();
    $.post("/DeviceGroup/GroupCreate", { 'GroupName': GroupName }, function (Response) {
        $('#group_name').val("");
    }, 'json');
});

$('#DevName').click(function () {
    $('#settingsDiv').html("");
    $("#deviceNameGroup").animate({ height: 'toggle', opacity: 'toggle' }, 300);
    $.post("/DeviceGroup/GroupShow", {}, function (Response) {
        $('#deviceName').html("");
        $('#deviceName').html(Response);
        $('#deviceName').css("display", "block");
        //LInd = 0;
    }, 'text');
});


$('#_preset_diagram').click(function () {
    $('#preset_group_name').css("display", "block");
});

$('#preset_save_diagram').click(function () {
    $(".tableBody").removeClass("jtk-endpoint-anchor");
    diagramPresetName = $('#preset_name_txt').val();
    filedata = new FormData();
    filedata.append("PresetName", diagramPresetName);
    filedata.append("Html", $('#mainDiv').html());
    $.ajax({
        type: 'post',
        contentType: false,
        processData: false,
        data: filedata,
        dataType: 'json',
        url: '/DeviceGroup/PresetDiagramSave',
        success: function (Response) {
            $(".tableBody").addClass("jtk-endpoint-anchor");
            $('#preset_group_name').css("display", "none");
            $('#preset_name_txt').val("");
        }
    });
});

$('body').on('click touchend', '#diagram_list_view', function (e) { //preset add remove list
    width = $(this).width;
    if (e.type == "touchend") {
        handled = true;
        $('#diagram_list_view').css({ 'width': width + 0 }).toggle();
        $('#preset_add_remove li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#preset_group_list_remove').css({ 'width': width + 0 }).toggle();
            $.post("/DeviceGroup/DiagramPresetList", {}, function (Response) {
                $('.preset_diagram_list_remove').html();
                $('.preset_diagram_list_remove').html(Response);
            });
        }
        else {
            handled = false;
        }
});



$('body').on('click touchend', '.removePresetDiagram', function () {
    presetName = $(this).attr("value");
    removeDiagramID = 1;
    $.post("/DeviceGroup/RemoveDiagramPreset", { presetName: presetName }, function (Response) {
        $('.preset_diagram_list_remove').html("");
        $('.preset_diagram_list_remove').html(Response);
    }, 'text');
});

$('body').on('click touchend', '.preset_diagram_list_remove li', function () { // selected  preset  click add 
    if (removeDiagramID == 0) {
        presetSearchName = $(this).children().attr("value");
        $('#preset_name_txt').val($(this).children().attr("value"));
        $('#preset_group_list_remove').css({ 'width': width + 0 }).toggle();
        connectaddremove = 1;
        $("#mainDiv").html("");
        location.reload();
        $.post("/DeviceGroup/LoadPresetDiagram", { presetSearchName: presetSearchName }, function (Response) {
            removePointsAndConnections();
            addPoints();

            $.each(Response.pointData, function (i, item) {
                var pointright = item.PointRight;
                var pointLeft = item.PointLeft;
                jsPlumb.connect({
                    uuids: [pointright.toString(), pointLeft.toString()]
                });
            });

            $("#mainDiv").html(Response.htmlData);
            $("#mainDiv .foo").draggable();
            saveDiagram();
        });
    }
    removeDiagramID = 0;
});