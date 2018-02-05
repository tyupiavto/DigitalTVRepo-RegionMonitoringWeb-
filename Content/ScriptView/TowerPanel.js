//var Name, LattiTube, LongiTube,IP,Phone,Status;
var presetName, device_tower_IP;
//$('#TowerInsert').click(function () {
////    TowerOpen($('#TowerName').val(), $('#TowerLattiTube').val(),
////        $('#TowerLongiTube').val(), $('#TowerIP').val(),
////        $('#TowerPhone').val(), $('#TowerStatus').val());
////});
////function TowerOpen(Name,LattiTube,LongiTube,IP,Phone,Status) {

//    name = $('#TowerName').val();
//    LattiTube = $('#TowerLattiTube').val();
//    LongiTube = $('#TowerLongiTube').val();
//    IP = $('#TowerIP').val();
//    Phone = $('#TowerPhone').val();
//    Status = $('#TowerStatus').val();

//    $.ajax({
//        type: 'POST',
//        data: {
//            'Name': Name, 'LattiTube': LattiTube, 'LongiTube': LongiTube, 'IP': IP, 'Phone': Phone, 'Status': Status
//        },
//        dataType: 'json',
//        url:'/Home/TowerInsert',
//        success: function (Response) {
//            $('#TowerName').val("")
//            $('#TowerLattiTube').val("");
//            $('#TowerLongiTube').val("");
//            $('#TowerIP').val("");
//            $('#TowerPhone').val("");
//            $('#TowerStatus').val("");
//        }
//    });
//});

var edit;
    edit = $("#tower_body");
    edit.on("click", "tr", function (e) {
        e.preventDefault();
        if (e.target.id == "edit_tower") {
            window.location.href = "/Tower/TowerEdit/" + $(this).data("id");
        }
    });

    $('#preset_delete').click(function () {
        presetName = $('#preset_inf').val();
            $.ajax({
                type: 'POST',
                data: {
                    'presetName': presetName
                },
                dataType: 'json',
                url: '/Tower/PresetDelete',
                success: function (Response) {
                    window.location.href = "/Tower/Tower";
                }
            });
    });


    $('#diagram_send').click(function () {
       
    });
