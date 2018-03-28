var countrieName, stateName, cityName,html;
var checkcity;
var htmls = new Array();
var connections = new Array();
var connect = new Array();
var filedata, towerDeleteID, connectionInd,connectaddremove=0;

    //checkcity = $('.rowCheckbox');
    //checkcity.on("click", "div", function (e) {
    $('body').on('click touched', '#city_check div', function() {
        var cityid = $(this).attr("id");
        
        if ($('#city_checked' + cityid).is(':checked') == false) {
            $('#checked_add' + cityid).removeClass("").addClass("checked");
            $("#city_checked" + cityid).prop('checked', true);
          
            var addnewcity = '<tr class="tableBody  city_remove' + cityid + '_' + $(this).parent().attr("value")+'" id="city_' + ($('.tableConn').length + 1) + '"><td class="tableConn">' + $('#city_checked' + cityid).val() + '</td></tr>';
            $(addnewcity).insertBefore('.add' + parentId);
         
            addPoints();
            //saveDiagram();

            cityName = $('#city_checked' + cityid).val();
            countrieName = $('#typeSelect #countrieName').text();
            stateName = $('#typeSelectState #state').text();
            $.post("/DeviceGroup/TowerInsert", { countrieName: countrieName, stateName: stateName, cityName: cityName, cityid: cityid }, function () {
            });
        }
        else {
            $('#checked_add' + cityid).removeClass("checked").addClass("");
            $("#city_checked" + cityid).prop('checked', false);
            var id = $(".city_remove" + cityid + "_" + $(this).parent().attr("value")).attr("id");
            jsPlumb.remove($('#' + id));
            towerDeleteID = $(this).parent().attr("id");
            $.post("/DeviceGroup/TowerDelete", { towerDeleteID: towerDeleteID, cityid: cityid }, function () {
              saveDiagram();
            });
        }
    });

    function addPoints() {
        jsPlumb.setContainer("mainDiv");
        jsPlumb.draggable($('.foo'), {
            //containment:"parent",
            stack: '.foo',
            grid: [10, 10]
        });
        $('.tableBody').each(function () {
            if (!$(this).hasClass('jtk-endpoint-anchor')) {
                var objId = $(this).closest('.foo').attr("id");
                var objname = $(this).closest('.foo').attr("name");
                if (!$(this).hasClass('screen')) {
                    if (objname != "Countrie") {
                        jsPlumb.addEndpoint($(this), {
                            anchor: "Right",
                            isSource: true,
                            isTarget: false,
                            connector: ["Bezier", { curviness: 130 }],
                            endpoint: "Blank",
                            cssClass: "blankEndpoint class" + objId,
                            connectorOverlays: [
                                ["Arrow", { width: 10, height: 10, length: 10, location: 0.97, id: "arrow", foldback: 0.8 }]
                            ],
                            connectorStyle: { stroke: "grey", strokeWidth: 3 },
                            connectorHoverStyle: { lineWidth: 3 },
                            maxConnections: -1,
                            uuid: $(this).attr("id") + 'r'
                        });
                        jsPlumb.addEndpoint($(this), {
                            anchor: "Left",
                            isSource: false,
                            isTarget: true,
                            connector: ["Bezier", { curviness: 130 }],
                            endpoint: "Blank",
                            cssClass: "blankEndpoint class" + objId,
                            connectorOverlays: [
                                ["Arrow", { width: 10, height: 10, length: 10, location: 0.97, id: "arrow", foldback: 0.8 }]
                            ],
                            connectorStyle: { stroke: "grey", strokeWidth: 3 },
                            connectorHoverStyle: { lineWidth: 3 },
                            uuid: $(this).attr("id") + 'l'
                        });
                    }
                    else {
                        jsPlumb.addEndpoint($(this), {
                            anchor: "Right",
                            isSource: true,
                            isTarget: false,
                            connector: ["Bezier", { curviness: 130 }],
                            endpoint: "Blank",
                            cssClass: "blankEndpoint class" + objId,
                            connectorOverlays: [
                                ["Arrow", { width: 10, height: 10, length: 10, location: 0.97, id: "arrow", foldback: 0.8 }]
                            ],
                            connectorStyle: { stroke: "grey", strokeWidth: 3 },
                            connectorHoverStyle: { lineWidth: 3 },
                            maxConnections: -1,
                            uuid: $(this).attr("id") + 'r'
                        });
                    }
                }
                else {
                    jsPlumb.addEndpoint($(this), {
                        anchor: "Top",
                        isSource: false,
                        isTarget: true,
                        connector: ["Bezier", { curviness: 130 }],
                        endpoint: "Blank",
                        cssClass: "blankEndpoint class" + objId,
                        /*connectorOverlays:[ 
                            [ "Arrow", { width:10, height: 10, length:10, location:0.97, id:"arrow", foldback: 0.8} ]
                        ],*/
                        connectorStyle: { stroke: "grey", strokeWidth: 3 },
                        connectorHoverStyle: { lineWidth: 3 },
                        maxConnections: 1,
                        uuid: $(this).attr("id") + 't'
                    });
                }
            }
        });
        setImage();
        //saveDiagram();
    }

    function setImage() { //Setting image to endpoints without connection
        var eps = $('.blankEndpoint');
        eps.each(function () {
            if (!$(this).hasClass('jtk-endpoint-connected')) {
                $(this).css({ 'min-width': '18px', 'min-height': '18px', 'background': 'url(/image/no_connection_radiobutton.png) no-repeat', 'background-size': '100%', 'margin-left': '-9px', 'margin-top': '-9px', 'z-index': '13' });
            }
        });
        saveDiagram();
    }


    function setImageOnConnection() {
        $('.jtk-endpoint-connected').css({ 'min-width': '18px', 'min-height': '18px', 'background': 'url(/image/connected_radiobutton.png) no-repeat', 'background-size': '100%', 'margin-left': '-9px', 'margin-top': '-9px' });
    }

    jsPlumb.bind("connection", function (connection) {
        setImage();
        setImageOnConnection();
        connectionInd = 0;
        ConnectionPoint();
        saveDiagram();
        $.post("/DeviceGroup/PointConnections", { connections: connections, connectionInd: connectionInd }, function (Response) {

        }, 'json');
    });

    jsPlumb.bind("connectionDetached", function (connection) {
        connection.sourceEndpoint.removeClass('jtk-endpoint-connected');
        setImage();
        if (connections.length == 1) {
            connectionInd = 1;
        }
        else {
            connectionInd = 0;
        }
        saveDiagram();
        ConnectionPoint();
        $.post("/DeviceGroup/PointConnections", { connections: connections, connectionInd: connectionInd }, function (Response) {

        }, 'json');
    });

    function ConnectionPoint() {
        connections = [];
        $.each(jsPlumb.getConnections(), function (idx, connection) {
            connect.push(connection.id);
            connect.push(connection.sourceId);
            connect.push(connection.targetId);
            connect.push(connection.getUuids()[0]);
            connect.push(connection.getUuids()[1]);
            connections.push(connect);
            connect = [];
        });
    }

    function saveDiagram() {
       
        $(".tableBody").removeClass("jtk-endpoint-anchor");
      //  $(".tableBodyTower").removeClass("jtk-endpoint-anchor");
        //Creating array to save all existing connections
        //connections = [];
        //$.each(jsPlumb.getConnections(), function (idx, connection) {
        //    connect.push(connection.id);
        //    connect.push(connection.sourceId);
        //    connect.push(connection.targetId);
        //    connect.push(connection.getUuids()[0]);
        //    connect.push(connection.getUuids()[1]);
        //    connections.push(connect);
        //    connect = [];
        //});

        filedata = new FormData();
        filedata.append("connect[]", connections);
        filedata.append("Html", $('#mainDiv').html());
        //$.post("/DeviceGroup/PointConnections", { connections: connections, connectionInd: connectionInd }, function (Response) {

        //}, 'json');
       // $(".tableBody").removeClass("jtk-endpoint-anchor");
      //  $(".tableBodyTower").removeClass("jtk-endpoint-anchor");
                
        $.ajax({
            type: 'post',
            contentType: false,
            processData: false,
            data: filedata,
            dataType: 'json',
            url: '/DeviceGroup/SaveDiagram',
            success: function (Response) {
                $(".tableBody").addClass("jtk-endpoint-anchor");
            //    $(".tableBodyTower").addClass("jtk-endpoint-anchor");
            }
        });
    }

    function removePointsAndConnections() {
        var oldEndPoints = document.getElementsByClassName('jtk-endpoint');
        while (oldEndPoints[0]) {
            oldEndPoints[0].parentNode.removeChild(oldEndPoints[0]);
        }
        var oldConnectors = document.getElementsByClassName('jtk-connector');
        while (oldConnectors[0]) {
            oldConnectors[0].parentNode.removeChild(oldConnectors[0]);
        }
    }

    function loadDiagram() {
        connectaddremove = 1;
            $("#mainDiv").html("");
            $.post("/DeviceGroup/LoadDiagram", {}, function (Response) {
                $("#mainDiv").html(Response.htmlData);
                $("#mainDiv .foo").draggable();
                removePointsAndConnections();
                addPoints();
                //addPointsTower();
                $.each(Response.pointData, function (i,item) {
                    var pointright = item.PointRight;
                    var pointLeft = item.PointLeft;
                    jsPlumb.connect({
                        uuids: [pointright.toString(), pointLeft.toString()]
                });
                });
            });

    }



        //function addPointsTower() {
        //    jsPlumb.setContainer("mainDiv");
        //    jsPlumb.draggable($('.foo'), {
        //        //containment:"parent",
        //        stack: '.foo',
        //        grid: [10, 10]
        //    });
        //    $('.tableBodyTower').each(function () {
        //        if (!$(this).hasClass('jtk-endpoint-anchor')) {
        //            var objId = $(this).closest('.foo').attr("id");
        //            if (!$(this).hasClass('screen')) {
        //                jsPlumb.addEndpoint($(this), {
        //                    anchor: "Right",
        //                    isSource: true,
        //                    isTarget: false,
        //                    connector: ["Bezier", { curviness: 130 }],
        //                    endpoint: "Blank",
        //                    cssClass: "blankEndpoint class" + objId,
        //                    connectorOverlays: [
        //                        ["Arrow", { width: 10, height: 10, length: 10, location: 0.97, id: "arrow", foldback: 0.8 }]
        //                    ],
        //                    connectorStyle: { stroke: "grey", strokeWidth: 3 },
        //                    connectorHoverStyle: { lineWidth: 3 },
        //                    maxConnections: -1,
        //                    uuid: $(this).attr("id") + 'r'
        //                });
        //                jsPlumb.addEndpoint($(this), {
        //                    anchor: "Left",
        //                    isSource: false,
        //                    isTarget: true,
        //                    connector: ["Bezier", { curviness: 130 }],
        //                    endpoint: "Blank",
        //                    cssClass: "blankEndpoint class" + objId,
        //                    connectorOverlays: [
        //                        ["Arrow", { width: 10, height: 10, length: 10, location: 0.97, id: "arrow", foldback: 0.8 }]
        //                    ],
        //                    connectorStyle: { stroke: "grey", strokeWidth: 3 },
        //                    connectorHoverStyle: { lineWidth: 3 },
        //                    uuid: $(this).attr("id") + 'l'
        //                });
        //            }
        //            else {
        //                alert($(this));
        //                jsPlumb.addEndpoint($(this), {
        //                    anchor: "Top",
        //                    isSource: false,
        //                    isTarget: true,
        //                    connector: ["Bezier", { curviness: 130 }],
        //                    endpoint: "Blank",
        //                    cssClass: "blankEndpoint class" + objId,
        //                    /*connectorOverlays:[ 
        //                        [ "Arrow", { width:10, height: 10, length:10, location:0.97, id:"arrow", foldback: 0.8} ]
        //                    ],*/
        //                    connectorStyle: { stroke: "grey", strokeWidth: 3 },
        //                    connectorHoverStyle: { lineWidth: 3 },
        //                    maxConnections: 1,
        //                    uuid: $(this).attr("id") + 't'
        //                });
        //            }
        //        }
        //    });
        //    setImage();
        //    //saveDiagram();
        //}
