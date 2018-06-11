var SearchName, searchTxt, searchID, SearchClear,startTime,endTime,alarmColor,alarmText,deviceName,ll;
$('body').on('click touchend', '#start_log', function () {
    window.open('/Trap/LogSetting', $.post("/Trap/LogShow", {}, function (Response) {
     //   window.open('/Trap/LogSetting');
        $('#loginformation').html("");
        $('#loginformation').html(Response);
        ll = Response;
    }));
    //var l;
    //$.post("/Trap/LogShow", {}, function (Response) {
    //    window.open('/Trap/LogSetting');
    //    $('#loginformation').html("");
    //    $('#loginformation').html(Response);
    //    l = Response;
    //});
    //$('#loginformation').html(l);
});

$("#buttrap").click(function () {
    $.post("/Trap/LogShow", {}, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
    });
});

$('#search_log').on('click', function () {
    SearchName = $('#SearchLogName').val();
    SearchClear = 1;
    startTime = $('#startDateTime').val();
    endTime = $('#endDateTime').val();
    $.post("/Trap/LogSearch", { SearchName: SearchName, SearchClear: SearchClear, startTime: startTime, endTime: endTime }, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
    },'text');
});

//$('#search_time_log').on('click', function () {
//    startTime = $('#startDateTime').val();
//    endTime = $('#endDateTime').val();
//    $.post("/Trap/SearchDateTime", { startTime: startTime, endTime: endTime }, function (Response) {
//        $('#loginformation').html("");
//        $('#loginformation').html(Response);
//    }, 'text');
//});

$('body').on('click touched', '#logCorrect', function () {
    alarmColor = "green";
    //alarmText = $('#valueColumn' + $(this).attr('value')).text();
    deviceName = $('#deviceColumn' + $(this).attr('value')).text();
    $('#textCorrectError').val($('#valueColumn' + $(this).attr('value')).text());
    //$.post('Trap/AlarmLog', { alarmColor: alarmColor, alarmText: alarmText, deviceName: deviceName }, function () { });
});

$('body').on('click touched', '#logError', function () {
    alarmColor = "red";
    //alarmText = $('#valueColumn' + $(this).attr('value')).text();
    deviceName = $('#deviceColumn' + $(this).attr('value')).text();
    $('#textCorrectError').val($('#valueColumn' + $(this).attr('value')).text());
   // $.post('Trap/AlarmLog', { alarmColor: alarmColor, alarmText: alarmText, deviceName: deviceName }, function () { });
});

$('body').on('click touched', '#logCrash', function () {
    alarmColor = "yellow";
   // alarmText = $('#valueColumn' + $(this).attr('value')).text();
    deviceName = $('#deviceColumn' + $(this).attr('value')).text();
    $('#textCorrectError').val($('#valueColumn' + $(this).attr('value')).text());
    //$.post('Trap/AlarmLog', { alarmColor: alarmColor, alarmText: alarmText, deviceName: deviceName}, function () {});
});

$('body').on('click touched', '#alarmSave', function () {
    alarmText = encodeURIComponent( $('#textCorrectError').val());
   //// alarmText = "<M>";
   // $.ajax({
   //     type: 'POST',
   //     data: {
   //         'alarmText': alarmText
   //     },
   //     contentType: "application/json; charset=utf-8",
   //     dataType: 'json',
   //     url: "/Trap/AlarmLog",
   //     success: function (Response) {
   //     }
   // });

    $.post("/Trap/AlarmLog", { alarmColor: alarmColor, deviceName: deviceName, alarmText: alarmText }, function (Response) {
    });
});

$('body').on('contextmenu touched', '#trap_log_information tr td', function () { // checked gps right click
    searchID = $(this).attr("id");

    $(document).bind("contextmenu", function (event, ui) {
        event.preventDefault();
        $(this).unbind(event);
        $(".log-menu").finish().toggle(100).css({
            top: (event.pageY) + "px",
            left: (event.pageX) + "px"
        });
        event.stopPropagation();
    });
});
$(".log-menu li").click(function (event) {

    switch ($(this).attr("data-action")) {
        case "Search":
            SearchName = $('#' + searchID).text();
            SearchClear = 1;
            $.post("/Trap/LogSearch", { SearchName: SearchName, SearchClear: SearchClear }, function (Response) {
                $('#loginformation').html("");
                $('#loginformation').html(Response);
            },'text');
            break;
        case "Clear":

            SearchClear = 0;
            $.post("/Trap/LogSearch", { SearchName: SearchName, SearchClear: SearchClear }, function (Response) {
                $('#loginformation').html("");
                $('#loginformation').html(Response);
            }, 'text');
            break;
    }
    $(".log-menu").hide(100);
});