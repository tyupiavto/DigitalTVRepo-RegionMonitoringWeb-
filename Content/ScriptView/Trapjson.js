var SearchName, searchTxt, searchID, SearchClear,startTime,endTime,alarmColor,alarmText,deviceName,listNumber,check,returnOidText,currentOidText,alarmDescription,correctColor=" ",errorColor=" ",crashColor=" ", whiteColor=" ",all;
$('body').on('click touchend', '#start_log', function () {
    window.open('/Trap/LogSetting', '/Trap/LogSetting');
//    $(document).ready(function () {
//    $.post("/Trap/LogShow", {}, function (Response) {
//        $('#loginformation').html("");
//        $('#loginformation').html(Response);
//    });
//});
//    var winGoogle = window.open('/Trap/LogSetting');
//    winGoogle.onload = function () {
//        setTimeout(LogInformations, 8000);
//    }
//});
//function LogInformations() {

//    $.post('/Trap/TowerLog', { mapTowerName: mapTowerName, mapTowerID: mapTowerID }, function (Response) {
//        $('#loginformation').html("");
//        $('#loginformation').html(Response);
//        alert("shemovida");
    });
//}
//$(document).ready(function () {
//    $.post("/Trap/LogShow", {}, function (Response) {
//        $('#loginformation').html("");
//        $('#loginformation').html(Response);
//    });
//});
$("#buttrap").click(function () {
    $.post("/Trap/LogShow", {}, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
        TrapPageCheck();
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
        TrapPageCheck();
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
    deviceName = $('#deviceColumn' + $(this).attr('value')).text();
    returnOidText = $('#oidColumn' + $(this).attr('value')).attr("name");
    currentOidText = $('#oidColumn' + $(this).attr('value')).text();
    $('#textCorrectError').val($('#valueColumn' + $(this).attr('value')).text());
    $('#modalColor').css('background-color', 'green');
    $('#correctErrorDescription').val("");
});

$('body').on('click touched', '#logError', function () {
    alarmColor = "red";
    deviceName = $('#deviceColumn' + $(this).attr('value')).text();
    returnOidText = $('#oidColumn' + $(this).attr('value')).attr("name");
    currentOidText = $('#oidColumn' + $(this).attr('value')).text();
    $('#textCorrectError').val($('#valueColumn' + $(this).attr('value')).text());
    $('#modalColor').css('background-color', 'red');
    $('#correctErrorDescription').val("");
});

$('body').on('click touched', '#logCrash', function () {
    alarmColor = "yellow";
    deviceName = $('#deviceColumn' + $(this).attr('value')).text();
    returnOidText = $('#oidColumn' + $(this).attr('value')).attr("name");
    currentOidText = $('#oidColumn' + $(this).attr('value')).text();
    $('#textCorrectError').val($('#valueColumn' + $(this).attr('value')).text());
    $('#modalColor').css('background-color', 'yellow');
    $('#correctErrorDescription').val("");
});

$('body').on('click touched', '#logClear', function () {
    alarmColor = "white";
    deviceName = $('#deviceColumn' + $(this).attr('value')).text();
    returnOidText = $('#oidColumn' + $(this).attr('value')).attr("name");
    currentOidText = $('#oidColumn' + $(this).attr('value')).text();
    $('#textCorrectError').val($('#valueColumn' + $(this).attr('value')).text());
    $('#modalColor').css('background-color', 'white');
    $('#correctErrorDescription').val("");
});

$('body').on('click touched', '#alarmSave', function () {
    alarmText = encodeURIComponent($('#textCorrectError').val());
    alarmDescription = $('#correctErrorDescription').val();
    $.post("/Trap/AlarmLog", { alarmColor: alarmColor, deviceName: deviceName, alarmText: alarmText, returnOidText: returnOidText, currentOidText: currentOidText, alarmDescription:alarmDescription}, function (Response) {
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
            if (SearchName != "") {
                $.post("/Trap/LogSearch", { SearchName: SearchName, SearchClear: SearchClear }, function (Response) {
                    $('#loginformation').html("");
                    $('#loginformation').html(Response);
                    TrapPageCheck();
                }, 'text');
            }
            break;
        case "Clear":
            
            SearchClear = 0;
            $.post("/Trap/LogSearch", { SearchName: SearchName, SearchClear: SearchClear }, function (Response) {
                $('#loginformation').html("");
                $('#loginformation').html(Response);
                TrapPageCheck();
            }, 'text');
            break;
    }
    $(".log-menu").hide(100);
});

$('body').on('click touched', '#pagelistlog li', function () {
    $('#traplogpagelist').text($(this).text());
    listNumber = $(this).text();
    $.post('/Trap/PageLogList', { listNumber: listNumber }, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
        TrapPageCheck();
    });
});

$('body').on('click touched', '#dropdown li', function () {
    var listID = $(this).attr('id');
    if ($(this).children().children().is(':checked') == false) {
        check = false;
    }
    else {
        check = true;
    }
    var trapListName = $('#' + $(this).attr('id')).text();
    
    $.post('/Trap/TrapNameCheck', { trapListName: trapListName, check: check}, function () { });
});
// search ferebis mixedvit
$('body').on('click touched', '#logAllColor', function () {
    if ($('#logAllColor').is(':checked') == false) {
        $('#logAllColor').removeClass("checked").addClass("");
        $("#logAllColor").prop('checked', false);

        $('#logCorrectColor').removeClass("checked").addClass("");
        $("#logCorrectColor").prop('checked', false);

        $('#logErrorColor').removeClass("checked").addClass("");
        $("#logErrorColor").prop('checked', false);

        $('#logCrashColor').removeClass("checked").addClass("");
        $("#logCrashColor").prop('checked', false);

        $('#logWhiteColor').removeClass("checked").addClass("");
        $("#logWhiteColor").prop('checked', false);
        correctColor = " ";
        errorColor = " ";
        crashColor = " ";
        whiteColor = " ";
        all = 0;
    }
    else {
        $('#logAllColor').removeClass("").addClass("checked");
        $("#logAllColor").prop('checked', true);

        $('#logCorrectColor').removeClass("").addClass("checked");
        $("#logCorrectColor").prop('checked', true);

        $('#logErrorColor').removeClass("").addClass("checked");
        $("#logErrorColor").prop('checked', true);

        $('#logCrashColor').removeClass("").addClass("checked");
        $("#logCrashColor").prop('checked', true);

        $('#logWhiteColor').removeClass("").addClass("checked");
        $("#logWhiteColor").prop('checked', true);

        all = 1;
        correctColor = " ";
        errorColor = " ";
        crashColor = " ";
        whiteColor = " ";

        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
            TrapPageCheck();
        });
    }
});
$('body').on('click touched', '#logWhiteColor', function () {
    if ($('#logWhiteColor').is(':checked') == false) {
        $('#logWhiteColor').removeClass("checked").addClass("");
        $("#logWhiteColor").prop('checked', false);
        whiteColor = " ";
        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
            TrapPageCheck();
        });
    }
    else {
        whiteColor = "white";
        $('#logWhiteColor').removeClass("").addClass("checked");
        $("#logWhiteColor").prop('checked', true);

        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
            TrapPageCheck();
        });
    }
});
$('body').on('click touched', '#logCorrectColor', function () {
    if ($('#logCorrectColor').is(':checked') == false) {
        $('#logCorrectColor').removeClass("checked").addClass("");
        $("#logCorrectColor").prop('checked', false);
        correctColor = " ";
        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all  }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
            TrapPageCheck();
        });
    }
    else {
        correctColor = "green";
        $('#logCorrectColor').removeClass("").addClass("checked");
        $("#logCorrectColor").prop('checked', true);

        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all  }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
            TrapPageCheck();
        });
    }
});
$('body').on('click touched', '#logErrorColor', function () {
    if ($('#logErrorColor').is(':checked') == false) {
        $('#logErrorColor').removeClass("checked").addClass("");
        $("#logErrorColor").prop('checked', false);
        errorColor = " ";
        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
            TrapPageCheck();
        });
    }
    else {
        errorColor = "red";
        $('#logErrorColor').removeClass("").addClass("checked");
        $("#logErrorColor").prop('checked', true);

        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all  }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
            TrapPageCheck();
        });
    }
});
$('body').on('click touched', '#logCrashColor', function () {
    if ($('#logCrashColor').is(':checked') == false) {
        $('#logCrashColor').removeClass("checked").addClass("");
        $("#logCrashColor").prop('checked', false);
        crashColor = " ";
        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all  }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
            TrapPageCheck();
        });
    }
    else {
        crashColor = "yellow";
        $('#logCrashColor').removeClass("").addClass("checked");
        $("#logCrashColor").prop('checked', true);

        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor,all: all }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
            TrapPageCheck();
        });
    }
});
function TrapPageCheck() {
    $('#dropdown li').each(function () {
        if ($(this).children().children().is(':checked') == false) {
            $('td:nth-child(' + $(this).children().children().attr("value") + '),th:nth-child(' + $(this).children().children().attr("value") + ')').hide();
            var column = "table #" + $(this).attr("id");
            $(column).hide();
        }
        else {
            var column = "table #" + $(this).attr("id");
            $(column).show();
            $('td:nth-child(' + $(this).children().children().attr("value") + '),th:nth-child(' + $(this).children().children().attr("value") + ')').show();
        }
    });
}