var SearchName, searchTxt, searchID, SearchInd,startTime,endTime;
$('body').on('click touchend', '#start_log', function () {
    window.open('/Trap/LogSetting');

    //$.post("/Trap/LogShow", {}, function (Response) {
    //    $('#loginformation').html("");
    //    $('#loginformation').html(Response);
    //});
});

$("#buttrap").click(function () {
    $.post("/Trap/LogShow", {}, function (Response) {
        $('#loginformation').html("");
        $('#loginformation').html(Response);
    });
});

$('#search_log').on('click', function () {
    SearchName = $('#SearchLogName').val();
    SearchInd = 1;
    startTime = $('#startDateTime').val();
    endTime = $('#endDateTime').val();
    $.post("/Trap/LogSearch", { SearchName: SearchName, SearchInd: SearchInd, startTime: startTime, endTime: endTime }, function (Response) {
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
            SearchInd = 1;
            $.post("/Trap/LogSearch", { SearchName: SearchName, SearchInd: SearchInd }, function (Response) {
                $('#loginformation').html("");
                $('#loginformation').html(Response);
            },'text');
            break;
        case "Clear":

            SearchInd = 0;
            $.post("/Trap/LogSearch", { SearchName: SearchName, SearchInd: SearchInd }, function (Response) {
                $('#loginformation').html("");
                $('#loginformation').html(Response);
            }, 'text');
            break;
    }
    $(".log-menu").hide(100);
});