var mapTowerName, mapTowerID;


    //<script type="text/javascript">
       
$(document).ready(function () {

    $('body').on('click touched', '.towerMap', function (event) {
        var winGoogle = window.open('/Trap/LogSetting');
        mapTowerName = $(this).attr("name");
        mapTowerID = $(this).attr("value");
           $.post('/Trap/TowerLog', { mapTowerName: mapTowerName, mapTowerID: mapTowerID }, function (Response) {
                $('#loginformation').html("");
                $('#loginformation').html(Response);
                TrapPageCheck();
            });
    });
});

    function LogInformation() {
    
        $.post('/Trap/TowerLog', { mapTowerName: mapTowerName, mapTowerID: mapTowerID }, function (Response) {
            $('#loginformation').html("");
            $('#loginformation').html(Response);
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
