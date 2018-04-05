$('body').on('click touchend', '#start_log', function () {
    $.post("/Trap/SendTrap", {}, function () { });
});