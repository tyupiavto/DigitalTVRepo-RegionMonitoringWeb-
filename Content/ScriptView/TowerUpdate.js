var ID, Name, LattiTube, LongiTube, IP, Phone, Status, TowerID, TowerName, DeviceName, DeviceIP, DevSerialNumber, DeleteID, DvcID, tower_state_id, Type, presetName, device_IP;

    function updateTower(ID) {

        Name = $('#TowerName').val();
        LattiTube = $('#TowerLattiTube').val();
        LongiTube = $('#TowerLongiTube').val();
        IP = $('#TowerIP').val();
        Phone = $('#TowerPhone').val();
        Status = $('#TowerStatus').val();
        $.ajax({
            type: 'POST',
            data: {
                'ID': ID, 'Name': Name, 'LattiTube': LattiTube, 'LongiTube': LongiTube, 'IP': IP, 'Phone': Phone, 'Status': Status
            },
            dataType: 'json',
            url: '/Tower/TowerUpdate',
            success: function (Response) {
                window.location.href = "/Tower/TowerOpen";
            }
        });
    };

    var deletetower;
    deletetower = $("#tower_body");
    deletetower.on("click", "tr", function (e) {
        if (e.target.id == "delete_tower_state") {
            DeleteID = $(this).data("id");
            $.ajax({
                type: 'Post',
                data: {
                    'DeleteID': DeleteID
                },
                dataType: 'text',
                url: '/Tower/DeleteState',
                success: function (Response) {
                    $('#tower_state').html("");
                    $('#tower_state').html(Response);
                }
            });
        }
    });

    var edittower;
    edittower = $("#tower_bodyy");
    edittower.on("click", "tr", function (e) {
        //e.preventDefault();
        if (e.target.id == "update_tower") {
            TowerID = $(this).data("id");
            TowerName = $('#TowerName').val();
            DeviceName = $('#DeviceName' + TowerID).val();
            DeviceIP = $('#DeviceIP' + TowerID).val();
            DevSerialNumber = $('#DevSerialNumber' + TowerID).val();
            presetName = $('#preset_device' + TowerID).val();
            $.ajax({
                type: 'Post',
                data: {
                    'TowerID': TowerID, 'TowerName': TowerName, 'DeviceName': DeviceName, 'DeviceIP': DeviceIP, 'DevSerialNumber': DevSerialNumber, 'presetName': presetName
                },
                dataType: 'json',
                url: '/Tower/DevicePanelUpdate',
                success: function (Response) {
                    alert("განახლება წარმატებით განხორციელდა")
                }
            });
        }
        if (e.target.id == "walk_send") {
            DvcID = $(this).data("id");
            IP = $('#DeviceIP' + DvcID).val();
            DeviceName = $('#DeviceName' + DvcID).val();
            $.ajax({
                type: 'POST',
                data: {
                    'IP': IP, 'DeviceName': DeviceName
                },
                dataType: 'text',
                url: '/Walk/WalkSend',
                success: function (Response) {
                    $('#walkdraw').html("");
                    $('#walkdraw').html(Response);
                    window.location.href = "/Walk/Index";
                }
            });

        }

        if (e.target.id == "delete_tower") {
            DeleteID = $(this).data("id");
            tower_state_id = $('#TowerState').val();
            $.ajax({
                type: 'POST',
                data: {
                    'DeleteID': DeleteID, 'tower_state_id': tower_state_id
                },
                dataType: 'text',
                url: '/Tower/DevicePanelDelete',
                success: function (Response) {
                    $('#search').html("");
                    $('#search').html(Response);
                }
            });
        }

        if (e.target.id == "diagram_send") {
          var  TowerIP = $(this).data("id");
          device_IP = $('#DeviceIP' + TowerIP).val();
          alert(device_IP);
          mib_name = $('#DeviceName' + TowerIP).val();
          //$.ajax({
          //    //type: 'get',
          //    data: {
          //        //'device_tower_IP': device_tower_IP
          //    },
          //    dataType: 'json',
          //    url: '/Diagram/Index',
          //    success: function (Response) {
          //        alert("shem");
          //    }
          $.post("/Diagram/PresetDiagramSearch", { 'device_IP': device_IP }, function (response) {
              alert("s");
              window.location.hrf = "/Diagram/Index";
                }, "text");

          //});
        }

    });

function getval(sel) {

    //TowerID = $('#TowerState').val();
    //var select = document.getElementById("TowerState");

        TowerID = $('#TowerState').val();
        $.ajax({
            type: 'Post',
            data: {
                'TowerID': TowerID
            },
            dataType: 'text',
            url: '/Tower/TowerState',
            success: function (Response) {
                $('#search').html("");
                $('#search').html(Response);
            }
        });
    }
