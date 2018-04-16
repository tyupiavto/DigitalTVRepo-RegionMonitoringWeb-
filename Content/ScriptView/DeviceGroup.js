var GroupName, deviceName, Name, ManuFacture, Model, Purpose,CountrieName,StateName,CityName;
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
    $.post("/DeviceGroup/ClearDiagram", function (Response) {
        $("#mainDiv").html("");
    },'json');
});

$('body').on('click touchend', '#open_map', function () {
    var win = window.open('/Map/mapStyle', 'Map');
    //$.post('/Map/openMap', {}, function () { });
});



