var archiveInd=0;
$('body').on('click touched', '#start_live_trap', function () {
    var win = window.open("/LiveTrap/LiveTrap");
    if (win) { win.focus(); }
});

$('body').on('click touchend', '#TrapLogView', function () {
    var winGoogle = window.open('/Trap/LogSetting');
});

$('body').on('click touchend', '#trapClear', function () {
    var alertWaring = confirm("You want to delete ?");
    if (alertWaring == true) {
        var alertWaring = confirm("delete ???");
        if (alertWaring == true) {
            $.post("/LiveTrap/ClearTrapLog", {}, function () { });
        }
    }
});

$('body').on('click touchend', '#trapLiveOnOff', function () {
    if (archiveInd == 1) {
        archiveInd--;
    }
    else {
        archiveInd++;
    }
    $.post("/LiveTrap/TrapClearArchive", { archiveInd: archiveInd }, function (Response) {
        $('#live_trap').html("");
        $('#live_trap').html(Response);
    });
});