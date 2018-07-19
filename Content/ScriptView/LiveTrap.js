
$('body').on('click touched', '#start_live_trap', function () {
    var win = window.open("/LiveTrap/LiveTrap");
    if (win) { win.focus(); }
});