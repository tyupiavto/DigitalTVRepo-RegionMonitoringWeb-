$(function () {
    $('#datetimepickerStart').datetimepicker();
    $('#datetimepickerEnd').datetimepicker();

    $('.dropdown-toggle::after').css("display", "none");
    $('#dropdown li').click(function () {
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
});