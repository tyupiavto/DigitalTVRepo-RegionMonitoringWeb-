var Tower, Device, IP, DevSerialNumber, Second, page,presetName;

$('#DevicePanelInsert').click(function () {
    Tower = $('#Tower').val(); Device = $('#DeviceID').val(); IP = $('#DevicePanelIP').val();
    DevSerialNumber = $('#DevSerialNumber').val();
    presetName = $('#preset_add').val();
    $.ajax({
        type: 'POST',
        data: {
            'Tower': Tower, 'Device': Device, 'IP': IP, 'DevSerialNumber': DevSerialNumber, 'presetName': presetName
        },
        dataType: 'json',
        url: '/Device/DevicePanelInsert',
        success: function (Response) {
            $('#DevicePanelIP').val("");
            $('#DevSerialNumber').val("");
        }
    });
});


//$('#GoWalk').click(function () {
//    window.location.href = "/Walk/Index";
//});
var edittower;
$(document).ready(function () {
    edittower = $("#device_body");
    edittower.on("click", "tr", function (e) {
        e.preventDefault();
        if (e.target.id == "edit_device") {
            window.location.href = "/Device/DeviceEdit/" + $(this).data("id");
        }

        if (e.target.id == "delete_device") {
            var DeleteID = $(this).data("id");
            $.ajax({
                type: 'Post',
                data: {
                    'DeleteID': DeleteID
                },
                dataType: 'text',
                url: '/Device/DeleteDevice',
                success: function (Response) {
                    $('#device_view').html("");
                    $('#device_view').html(Response);
                }
            });
        }
    });
});