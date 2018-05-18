var GroupName, deviceName, Name, ManuFacture, Model, Purpose, CountrieName, StateName, CityName;
var deviceGroupList, deviceRemoveID,towerID,deviceName,deviceTopMinimizeID=0,diagramPresetName,removeDiagramID=0;


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
   // alert("You want to delete ?");
    var conf = confirm("You want to delete ?");
    if (conf == true) {
        location.reload();
        $.post("/DeviceGroup/ClearDiagram", function (Response) {
            $("#mainDiv").html("");
        }, 'json');
    }
});

$('body').on('click touchend', '#open_map', function () {
    var win = window.open('/Map/mapStyle', 'Map');
    //$.post('/Map/openMap', {}, function () { });
});


$('body').on('contextmenu touched', '.tableBodyTower', function () { // remove device
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
            break;
    }
    $(".remove-menu").hide(100);
});


$('body').on('contextmenu touched', '#towe-remove', function () { // remove tower
    towerRemoveID = $(this).parent().parent().parent().attr("id");
    //DeviceName = $(this).attr("name");
    //towerID = $(this).attr("title");
    $(document).bind("contextmenu", function (event, ui) {
        event.preventDefault();
        $(this).unbind(event);
        $(".remove-tower-menu").finish().toggle(100).css({
            top: (event.pageY) + "px",
            left: (event.pageX) + "px"
        });
        event.stopPropagation();
    });
});



$(".remove-tower-menu li").click(function (event) {

    switch ($(this).attr("data-action")) {
        case "RemoveTower":
            deviceTopMinimizeID = 0;
            var towID = $('#' + towerID).parent().parent().attr("id");
            $('.add' + towerRemoveID + '  table').each(function () {   
                var deviceName = "tower_" + towerRemoveID;
                   deviceRemoveID = $(this).attr("id");
                $.post("/DeviceGroup/RemoveTower", { deviceName: deviceName, deviceRemoveID: deviceRemoveID }, function () { }, 'json');
            });
            jsPlumb.remove($('.device_list_name' + towerRemoveID));
            foo();
            saveDiagram();
            break;
    }
    $(".remove-tower-menu").hide(100);
});