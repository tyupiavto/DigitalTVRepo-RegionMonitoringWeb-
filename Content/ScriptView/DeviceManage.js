
var dvcID, DeviceName;
$(document).on('click touchend', '.device_settings', function () { // add device setting open
    dvcID = $(this).closest($(".foo")).attr("id");
    DeviceName = $('.header' + dvcID).text();
    if (DeviceName.length > 15) {
        DeviceName = DeviceName.substr(0, DeviceName.length - (DeviceName.length - 12)) + '...';
    }

    //$.post("/DeviceGroup/DeviceManegeSetting", { dvcID: dvcID, DeviceName: DeviceName}, function (Response) {
    //    $('#settingsDiv').html("");
    //    $('#settingsDiv').html(Response);
    //}, 'text');

    $.post("/DeviceGroup/WalkMib", {}, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
    }, 'text');

});


var handled = false; var width;                                          // countries search and full countries
$('body').on('click touchend', '#Select_Time', function (e) {
    width = $(this).width();

    if (e.type == "touchend") {
        handled = true;
        $('#Select_Time_List').css({ 'width': width + 12 }).toggle();
        $('.Select_Time li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#Select_Time_List').css({ 'width': width + 12 }).toggle();
            
            if ($('#Select_Time_List').css("display") == "block") {
                $('#Select_Time_List').css("display", "block");
            }
            else {
                $('#Select_Time_List').css("display", "block");
            }
            $.post("/DeviceGroup/ScanIntervalDvc", {}, function (Response) {
                $('#interva_partial').html("");
                $('#interva_partial').html(Response);
            }, 'text');
          
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend', '#Select_Time_List li', function () {
    var value = $(this).text();
    $('.header' + $('#Select_Time_List').attr('value')).text(value);
    $('#time_interval').text(value);
    $('#Select_Time_List').css({ 'width': width + 12 }).toggle();
    $('#Select_Time_List').css("display", "none");
});
