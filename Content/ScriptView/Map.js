var mapTowerName, mapTowerID;


    //<script type="text/javascript">
       
$(document).ready(function () {
    jQuery('.gm-style-iw').prev('div').remove();
    jQuery('.gm-style-iw').next().remove();
    $('.gm-style-iw').parent().parent().css('top', '40px');
    $('.gm-style-iw').parent().parent().css('left', '10px');
    window.open('/Trap/LogSetting');
    $('body').on('click touched', '.towerMap', function (event) {
        mapTowerName = $(this).attr("name");
        mapTowerID = $(this).attr("value");
        var winGoogle = window.open('/Trap/LogSetting');
        winGoogle.onload = function () {
            //mapTowerName = $(this).attr("name");
            //mapTowerID = $(this).attr("value");
            //$.post('/Trap/TowerLog', { mapTowerName: mapTowerName, mapTowerID: mapTowerID }, function (Response) {
            //    $('#loginformation').html("");
            //    $('#loginformation').html(Response);
            //    alert("shemovida");
            // TrapPageCheck();
            //});
          //  setTimeout(LogInformation, 8000);
        }
    });
});

    function LogInformation() {
    
        $.post('/Trap/TowerLog', { mapTowerName: mapTowerName, mapTowerID: mapTowerID }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
            alert("shemovida");
        });
    }
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
