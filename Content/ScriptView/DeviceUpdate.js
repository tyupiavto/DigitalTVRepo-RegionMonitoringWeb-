var ID, Name, Model, Manufacture, Purpose, MibParser, TowerName, DeviceName, DeviceIP, DevSerialNumber,presetName;

function update(ID) {
  
    Name = $('#DeviceName').val();
    Model = $('#DeviceModel').val();
    Manufacture = $('#DeviceManufacture').val();
    Purpose = $('#TowDevicePurposeerIP').val();
    MibParser = $('#DeviceMibParser').val();
    $.ajax({
        type: 'POST',
        data: {
            'ID': ID, 'Name': Name, 'Model': Model, 'Manufacture': Manufacture, 'Purpose': Purpose, 'MibParser': MibParser
        },
        dataType: 'json',
        url: '/Device/DeviceUpdate',
        success: function (Response) {
            window.location.href = "/Device/DeviceOpen";
        }
    });
};


$('#AddSecond').click(function () {
    Second = $('#Interval').val();
    $.ajax({
        type: 'POST',
        data: {
            'Second': Second
        },
        dataType: 'json',
        url: '/Device/SecondInterval',
        success: function (Response) {
            //$('#Interval').val("");
            window.location.href = "/Home/Index";
        }
    });
});

$('#DeleteInterval').click(function () {
    DeleteID = $('#IntervalID').val();
    $.ajax({
        type: 'POST',
        data: {
            'DeleteID': DeleteID
        },
        dataType: 'json',
        url: '/Device/DeleteInterval',
        success: function (Response) {
            window.location.href = "/Home/Index";
        }
    });
});
//$('#Update').click(function () {
//    ID = $('.NumberID').data("id");
//    TowerName = $('#TowerName').val();
//    DeviceName = $('#DeviceName').val();
//    DeviceIP = $('#DeviceIP').val();
//    DevSerialNumber = $('#DevSerialNumber').val();
//    presetName = $('#preset_device').val();
//    $.ajax({
//        type: 'POST',
//        data: {
//            'ID': ID, 'TowerName': TowerName, 'DeviceName': DeviceName, 'DeviceIP': DeviceIP, 'DevSerialNumber': DevSerialNumber, 'presetName': presetName
//        },
//        dataType: 'json',
//        url: '/Tower/DevicePanelUpdate',
//        success: function (Response) {
//        }
//    });

//});
