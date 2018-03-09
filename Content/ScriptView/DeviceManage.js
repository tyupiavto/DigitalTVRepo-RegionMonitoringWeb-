
var dvcID, DeviceName;

$(document).ready(function () { 

$(document).on('click touchend', '.device_settings', function () { // add device setting open
    dvcID = $(this).closest($(".foo")).attr("id");
    DeviceName = $('.header' + dvcID).text();
    if (DeviceName.length > 15) {
        DeviceName = DeviceName.substr(0, DeviceName.length - (DeviceName.length - 12)) + '...';
    }

    //$.post("/DeviceGroup/DeviceManegeSetting", { dvcID: dvcID, DeviceName: DeviceName}, function (Response) {
    //    $('#search_time_interval').html("");
    //    $('#search_time_interval').html(Response);
    //}, 'text');

    var $modal = $('#myModal');
    $.post("/DeviceGroup/WalkMib", {}, function (Response) {
        $('#device_settings').html("");
        $('#device_settings').html(Response);
        //$modal.modal("show");
    }, 'text');
    
});


var handled = false; var width;                                          // countries search and full countries
$('body').on('click touchend', '#select_time', function (e) {
    width = $(this).width();

    if (e.type == "touchend") {
        handled = true;
        $('#select_time_list').css({ 'width': width + 12 }).toggle();
        $('.select_time li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#select_time_list').css({ 'width': width + 12 }).toggle();
            
            if ($('#select_time_list').css("display") == "block") {
                $('#select_time_list').css("display", "block");
            }
            else {
                $('#select_time_list').css("display", "block");
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

});
