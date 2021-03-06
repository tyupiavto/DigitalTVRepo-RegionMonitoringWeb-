﻿var SearchName, searchTxt, searchID, SearchClear,startTime,endTime,alarmColor,alarmText,deviceName,listNumber,check,returnOidText,currentOidText,alarmDescription,correctColor=" ",errorColor=" ",crashColor=" ", whiteColor=" ",all;
var response,error=0,crash=0,correct=0,white=0;
$('body').on('click touchend', '#start_log', function () {
    var winGoogle = window.open('/Trap/LogSetting');
});
function LogInformations() {

    $.post('/Trap/TowerLog', { mapTowerName: mapTowerName, mapTowerID: mapTowerID }, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
        response = Response;
        alert("shemovida");
    });
}

$('body').on('click touchend', '#LiveTrapLog', function () {
    var win = window.open("/LiveTrap/LiveTrap");
    if (win) { win.focus(); }
});

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
    $('#load_walk').css("display", "block");
    $.post("/Trap/LogSearch", { SearchName: SearchName, SearchClear: SearchClear, startTime: startTime, endTime: endTime }, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
        TrapPageCheck();
        $('#load_walk').css("display", "none");
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
    $('#correctErrorDescription').val($('#alarmDescriptionColumn' + $(this).attr('value')).text());
});

$('body').on('click touched', '#logError', function () {
    alarmColor = "red";
    deviceName = $('#deviceColumn' + $(this).attr('value')).text();
    returnOidText = $('#oidColumn' + $(this).attr('value')).attr("name");
    currentOidText = $('#oidColumn' + $(this).attr('value')).text();
    $('#textCorrectError').val($('#valueColumn' + $(this).attr('value')).text());
    $('#modalColor').css('background-color', 'red');
    $('#correctErrorDescription').val($('#alarmDescriptionColumn' + $(this).attr('value')).text());
});

$('body').on('click touched', '#logCrash', function () {
    alarmColor = "yellow";
    deviceName = $('#deviceColumn' + $(this).attr('value')).text();
    returnOidText = $('#oidColumn' + $(this).attr('value')).attr("name");
    currentOidText = $('#oidColumn' + $(this).attr('value')).text();
    $('#textCorrectError').val($('#valueColumn' + $(this).attr('value')).text());
    $('#modalColor').css('background-color', 'yellow');
    $('#correctErrorDescription').val($('#alarmDescriptionColumn' + $(this).attr('value')).text());
});

$('body').on('click touched', '#logClear', function () {
    alarmColor = "white";
    deviceName = $('#deviceColumn' + $(this).attr('value')).text();
    returnOidText = $('#oidColumn' + $(this).attr('value')).attr("name");
    currentOidText = $('#oidColumn' + $(this).attr('value')).text();
    $('#textCorrectError').val($('#valueColumn' + $(this).attr('value')).text());
    $('#modalColor').css('background-color', 'white');
    $('#correctErrorDescription').val($('#alarmDescriptionColumn' + $(this).attr('value')).text());
});

$('body').on('click touched', '#alarmSave', function () {
    alarmText = encodeURIComponent($('#textCorrectError').val());
    alarmDescription = $('#correctErrorDescription').val();
    $('#logTableInf').find("td.valueInformation").map(function (i, val) {
        var id = val.id.substring(11);
        var value = val.innerText.indexOf($('#textCorrectError').val());

        if (value != -1 && $('#oidColumn' + id).text() == currentOidText && $('#oidreturnedColumn' + id).text() == returnOidText) {
            if (alarmColor == "white") {
                $('#alarmDescriptionColumn' + id).text("");
            }
            else {
                $('#alarmDescriptionColumn' + id).text($('#correctErrorDescription').val());
            }
            $('#' + val.id).parent().css('background-color', alarmColor);
           
        }
    });
    $.post("/Trap/AlarmLog", { alarmColor: alarmColor, deviceName: deviceName, alarmText: alarmText, returnOidText: returnOidText, currentOidText: currentOidText, alarmDescription:alarmDescription}, function (Response) {});
});

$('body').on('contextmenu touched', '#trap_log_information tr td', function () {
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
        case "Copy":
            copyToCli();
            break;
    }
    $(".log-menu").hide(10);
});

function copyToCli() {
    var msg = $('#' + searchID).text();
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(msg).select();
    document.execCommand("copy");
    $temp.remove();
}

function copyToClipboard() {
    var copyText = document.getElementById("Copy" + searchID);
    copyText.select();
    document.execCommand("copy");
    alert("Copied the text: " + copyText.value);
}

$('body').on('click touched', '#pagelistlog li', function () {
    $('#traplogpagelist').text($(this).text());
    listNumber = $(this).text();
    $.post('/Trap/LogListCountSize', { listNumber: listNumber }, function (Response) {
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

$('body').on('click touched', '#whiteColor', function () { // mxolod tetri feris archeva
    all = 0;
    if (white == 0) {
        white++;
        whiteColor = "white";
    }
    else {
        white--
        whiteColor = " ";
    }

    $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
        TrapPageCheck();
    });

});

$('body').on('click touched', '#correctColor', function () {//mxolod mwvane feris archeva
    all = 0;
    if (correct == 0) {
        correct++;
        correctColor = "green";
    }
    else {
        correct--;
        correctColor = " ";
    }
    //whiteColor = " ";
    //errorColor = " ";
    //crashColor = " ";
    $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
        TrapPageCheck();
    });
});

$('body').on('click touched', '#errorColor', function () {// mxolod witeli feris archeva
    all = 0;
    //whiteColor = " ";
    //correctColor = " ";
    //crashColor = " ";
    if (error == 0) {
        error++;
        errorColor = "red";
    }
    else {
        error--;
        errorColor = " ";
    }

     $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
        TrapPageCheck();
    });
});

$('body').on('click touched', '#crashColor', function () { //mxolod yviteli feris archeva
    all = 0;
    //whiteColor = " ";
    //correctColor = " ";
    //errorColor = " ";
    if (crash == 0) {
        crash++;
        crashColor = "yellow";
    }
    else {
        crash--;
        crashColor = " ";
    }
    $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
        TrapPageCheck();
    });
});

$('body').on('click touched', '#allColor', function () { // yvela feri ertad
    all = 1;
    correctColor = " ";
    errorColor = " ";
    crashColor = " ";
    whiteColor = " ";
    correct = 0;
    error = 0;
    white = 0;
    crash = 0;
    $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
        TrapPageCheck();
    });
});

// search ferebis mixedvit
//$('body').on('click touched', '#logAllColor', function () {
//    if ($('#logAllColor').is(':checked') == false) {
//        $('#logAllColor').removeClass("checked").addClass("");
//        $("#logAllColor").prop('checked', false);

//        $('#logCorrectColor').removeClass("checked").addClass("");
//        $("#logCorrectColor").prop('checked', false);

//        $('#logErrorColor').removeClass("checked").addClass("");
//        $("#logErrorColor").prop('checked', false);

//        $('#logCrashColor').removeClass("checked").addClass("");
//        $("#logCrashColor").prop('checked', false);

//        $('#logWhiteColor').removeClass("checked").addClass("");
//        $("#logWhiteColor").prop('checked', false);
//        correctColor = " ";
//        errorColor = " ";
//        crashColor = " ";
//        whiteColor = " ";
//        all = 0;
//    }
//    else {
//        $('#logAllColor').removeClass("").addClass("checked");
//        $("#logAllColor").prop('checked', true);

//        $('#logCorrectColor').removeClass("").addClass("checked");
//        $("#logCorrectColor").prop('checked', true);

//        $('#logErrorColor').removeClass("").addClass("checked");
//        $("#logErrorColor").prop('checked', true);

//        $('#logCrashColor').removeClass("").addClass("checked");
//        $("#logCrashColor").prop('checked', true);

//        $('#logWhiteColor').removeClass("").addClass("checked");
//        $("#logWhiteColor").prop('checked', true);

//        all = 1;
//        correctColor = " ";
//        errorColor = " ";
//        crashColor = " ";
//        whiteColor = " ";

//        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
//            $('#loginformation').html("");
//            $('#loginformation').html(Response);
//            TrapPageCheck();
//        });
//    }
//});
//$('body').on('click touched', '#logWhiteColor', function () {
//    if ($('#logWhiteColor').is(':checked') == false) {
//        $('#logWhiteColor').removeClass("checked").addClass("");
//        $("#logWhiteColor").prop('checked', false);
//        whiteColor = " ";
//        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
//            $('#loginformation').html("");
//            $('#loginformation').html(Response);
//            TrapPageCheck();
//        });
//    }
//    else {
//        whiteColor = "white";
//        $('#logWhiteColor').removeClass("").addClass("checked");
//        $("#logWhiteColor").prop('checked', true);

//        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
//            $('#loginformation').html("");
//            $('#loginformation').html(Response);
//            TrapPageCheck();
//        });
//    }
//});
//$('body').on('click touched', '#logCorrectColor', function () {
//    if ($('#logCorrectColor').is(':checked') == false) {
//        $('#logCorrectColor').removeClass("checked").addClass("");
//        $("#logCorrectColor").prop('checked', false);
//        correctColor = " ";
//        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all  }, function (Response) {
//            $('#loginformation').html("");
//            $('#loginformation').html(Response);
//            TrapPageCheck();
//        });
//    }
//    else {
//        correctColor = "green";
//        $('#logCorrectColor').removeClass("").addClass("checked");
//        $("#logCorrectColor").prop('checked', true);

//        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all  }, function (Response) {
//            $('#loginformation').html("");
//            $('#loginformation').html(Response);
//            TrapPageCheck();
//        });
//    }
//});
//$('body').on('click touched', '#logErrorColor', function () {
//    if ($('#logErrorColor').is(':checked') == false) {
//        $('#logErrorColor').removeClass("checked").addClass("");
//        $("#logErrorColor").prop('checked', false);
//        errorColor = " ";
//        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all }, function (Response) {
//            $('#loginformation').html("");
//            $('#loginformation').html(Response);
//            TrapPageCheck();
//        });
//    }
//    else {
//        errorColor = "red";
//        $('#logErrorColor').removeClass("").addClass("checked");
//        $("#logErrorColor").prop('checked', true);

//        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all  }, function (Response) {
//            $('#loginformation').html("");
//            $('#loginformation').html(Response);
//            TrapPageCheck();
//        });
//    }
//});
//$('body').on('click touched', '#logCrashColor', function () {
//    if ($('#logCrashColor').is(':checked') == false) {
//        $('#logCrashColor').removeClass("checked").addClass("");
//        $("#logCrashColor").prop('checked', false);
//        crashColor = " ";
//        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor, all: all  }, function (Response) {
//            $('#loginformation').html("");
//            $('#loginformation').html(Response);
//            TrapPageCheck();
//        });
//    }
//    else {
//        crashColor = "yellow";
//        $('#logCrashColor').removeClass("").addClass("checked");
//        $("#logCrashColor").prop('checked', true);

//        $.post('/Trap/ColorSearch', { correctColor: correctColor, errorColor: errorColor, crashColor: crashColor, whiteColor: whiteColor,all: all }, function (Response) {
//            $('#loginformation').html("");
//            $('#loginformation').html(Response);
//            TrapPageCheck();
//        });
//    }
//});

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