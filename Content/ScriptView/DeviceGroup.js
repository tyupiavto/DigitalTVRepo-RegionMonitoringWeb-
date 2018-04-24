var GroupName, deviceName, Name, ManuFacture, Model, Purpose, CountrieName, StateName, CityName;
var deviceGroupList, deviceRemoveID,towerID,deviceName,deviceTopMinimizeID=0,diagramPresetName,removeDiagramID=0;
var LInd = 0;
$('#add_group').click(function () {
    GroupName = $('#add_group_name').val();
    $.post("/DeviceGroup/GroupCreate", { 'GroupName': GroupName }, function (Response) {
        $('#group_name').val("");
    }, 'json');
});

$('#DevName').click(function () {
    $.post("/DeviceGroup/GroupShow", {}, function (Response) {
        $('#deviceName').html("");
        $('#deviceName').html(Response);
        $('#deviceName').css("display", "block");
        $("#deviceNameGroup").animate({ height: 'toggle', opacity: 'toggle' }, 300);
        LInd = 0;
    }, 'text');
});

var device_add;
device_add = $('#deviceName');
device_add.on("click", "li span", function (e) {
    var deviceGroupID = $(this).attr("value");
    if (e.target.id == "add_device" + deviceGroupID) {
        deviceName = $('#group_name' + deviceGroupID).text();
        $.post("/DeviceGroup/AddDevice", { deviceName: deviceName, deviceGroupID: deviceGroupID }, function (Response) {
            $('#add_device_partial').html("");
            $('#add_device_partial').html(Response);
        },'text');
    }
});


var file; var DeviceInsert = []; var array = [];
$('#add_file').click(function () {

    file = new FormData();
    file.append("Name", $('#device_name').val());
    file.append("ManuFacture",$('#manufacture_name').val());
    file.append("Model",$('#model_name').val());
    file.append("Purpose",$('#purpose_name').val());
    file.append("mib_file", $('#FileUploadMib')[0].files[0]);
    
    $.ajax({
        type: 'post',
        contentType: false,
        processData: false,
        data: file,
        dataType: 'json',
        url: '/DeviceGroup/DeviceCreate',
        success: function (Response) {
            $('#add_device_partial').html("");
        }
    });
});

$('body').on('click touchend', '#clear_diagram', function () {
    alert("You want to delete ?");
    location.reload();
    $.post("/DeviceGroup/ClearDiagram", function (Response) {
        $("#mainDiv").html("");
    },'json');
});

$('body').on('click touchend', '#open_map', function () {
    var win = window.open('/Map/mapStyle', 'Map');
    //$.post('/Map/openMap', {}, function () { });
});

//device_list = $('#deviceName');
$('#deviceName').on("click touched", "li", function () {
    deviceGroupList = $(this).attr("value");
    if (LInd == 0) {
        $('#deviceGroupName' + deviceGroupList).animate({ height: 'toggle', opacity: 'toggle' }, 300);
        LInd = 1;
    }
    $('#deviceGroupName' + deviceGroupList).animate({ height: 'toggle', opacity: 'toggle' }, 300);
    $.post("/DeviceGroup/DeviceList", { deviceGroupList: deviceGroupList }, function (Response) {
        $('#deviceGroupName' + deviceGroupList).html("");
        $('#deviceGroupName' + deviceGroupList).html(Response);
    }, 'text');
});

$('body').on('contextmenu touched', '.tableBodyTower', function () { // checked gps right click
    deviceRemoveID = $(this).attr("id");
    DeviceName = $(this).attr("name");
    towerID = $(this).attr("title");
    $(document).bind("contextmenu", function (event, ui) {
        event.preventDefault();
        $(this).unbind(event);
        $(".remove-menu").finish().toggle(100).css({
            top: (event.pageY) + "px",
            left: (event.pageX) + "px"
        });
        event.stopPropagation();
    });
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

$(".remove-menu li").click(function (event) {

    switch ($(this).attr("data-action")) {
        case "RemoveDevice":
            deviceTopMinimizeID = 0;
            var towID =$('#' + towerID).parent().parent().attr("id");
            $('.add' + towID + '  table').each(function () {
                if (deviceTopMinimizeID == 1) {
                    $('.tower_name' + $(this).attr("id")).css("top", "" + ($('.tower_name' + $(this).attr("id")).position().top - 35) + "px");
                }
                if (deviceRemoveID == $(this).attr("id")) {
                    $('.tower_name' + deviceRemoveID).remove();
                    var height = $(".add" + $('#' + towerID).parent().parent().attr("id")).height();
                    $('.add' + towID).height(height - 35);
                    deviceTopMinimizeID = 1;
                    $.post("/DeviceGroup/RemoveDevice", { DeviceName: DeviceName, towerID: towerID }, function () { }, 'json');
                }
            });
            //$('.tower_name' + deviceRemoveID).remove();
            //var height = $(".add" + $('#' + towerID).parent().parent().attr("id")).height();
            //$('.add' + $('#' + towerID).parent().parent().attr("id")).height(height - 35);
           
            break;
       
    }
    $(".remove-menu").hide(100);
});