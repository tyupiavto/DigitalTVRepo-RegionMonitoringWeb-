var countrieName, stateName, cityName,html;
var checkcity;
var htmls = new Array();
var connections = new Array();
var connect = new Array();
var filedata, towerDeleteID, connectionInd, connectaddremove = 0, loadInd = 0, UnConnectio = 0, towerConnection, IPaddress, towerName, deviceId, idTarget,title,towerID,cityID;

//$(document).ready(function () {
    //$('.devInfo').hide();

    //window.onload = loadDiagram();

    //$(window).on("beforeunload", function (e) {
    //    //saveDiagram();
    //});
    //$('#countrie').click(function () {
    //    $("#countries").animate({ height: 'toggle', opacity: 'toggle' }, 300);
    //});

    //$('#countries li').draggable({
    //    helper: 'clone',
    //    revert: 'invalid'
    //});

    ////$('#deviceName li').draggable({
    ////    helper: 'clone',
    ////    revert: 'invalid'
    ////});

    //$('#tower_menu').click(function () {
    //    $("#tower_drag").animate({ height: 'toggle', opacity: 'toggle' }, 300);
    //});

    //$('.tower').draggable({
    //    helper: 'clone',
    //    revert: 'invalid'
    //});
    ////$('.device_list').draggable({
    ////    helper: 'clone',
    ////    revert: 'invalid'
    ////});

    //function foo() {
    //    $('.foo').each(function () {
    //        //Making dropped elements draggable again
    //        $(this).draggable({
    //            //containment: $(this).parent(),
    //            stack: '.foo',
    //            grid: [10, 10],
    //            drop: function (event, ui) {
    //                var pos = ui.draggable.offset(), dPos = $(this).offset();
    //            },
    //            drag: function () {
    //                //jsPlumb.addToDragSelection($('.foo'));
    //                var draggedItemId = $(this).attr('id');
    //                $('.contextmenu').hide();
    //                $(this).css({ 'z-index': '11' });
    //                $(".class" + draggedItemId).css({ 'z-index': '15' });
    //                //saveDiagram();
    //            },
    //            stop: function () {
    //                var draggedItemId = $(this).attr('id');
    //                $(this).css({ 'z-index': '0' });
    //                $(".class" + draggedItemId).css({ 'z-index': '10' });
    //                if ($(this).position().left < 0) {
    //                    $(this).css({ 'left': '0' });
    //                }
    //                if ($(this).position().top < 0) {
    //                    $(this).css({ 'top': '0' });
    //                }
    //                //saveDiagram();
    //            }
    //        });
    //    });
    //}

    //var xCoordinate = 0;
    //var yCoordinate = 0;
    //var elem;
    //var diagramLenght = $('.foo').length;
    //$("#mainDiv").droppable({
    //    drop: function (ev, ui) {

    //        //if (!ui.draggable.hasClass('foo') && !ui.draggable.hasClass('display')) {
    //        var pos = ui.draggable.offset(), dPos = $(this).offset();
    //        var droppedTop = ui.position.top - $(this).offset().top + $('#mainDiv').scrollTop();
    //        var droppedLeft = ui.position.left - $(this).offset().left + $('#mainDiv').scrollLeft();
    //        var ids = $(this).attr("id");

    //        if (!ui.draggable.hasClass('foo') && ui.draggable.hasClass('countries')) {
    //            diagramLenght = $('.foo').length;
    //             Class = ui.draggable.attr("class");
    //             title = ui.draggable.text().trim();
    //            var item = $('<table class="foo elementTable ' + Class + '" name="' + title + '" id="' + (diagramLenght + 1) + '" style="left: ' + droppedLeft + 'px; top: ' + droppedTop + 'px;"><tr class="tableHeader"><th class="thClass"><span style="margin-left:25px;" class="header' + (diagramLenght + 1) + '">' + title + '</span><span class="settings"><span id="state' + (diagramLenght + 1) + '"style="display:none">State</span><img src="/Icons/pignon.png" width="17px" height="17px" alt="Settings" title="Settings" /></span></th></tr><tr><td class="add' + (diagramLenght + 1) + '"><span class="addList' + (diagramLenght + 1) + '"></span></td></tr></table>');
    //            $(this).append(item);
    //            diagramLenght++;
    //            foo();
    //            saveDiagram();
    //        }
    //        if (!ui.draggable.hasClass('foo') && ui.draggable.hasClass('tower')) {
    //            diagramLenght = $('.foo').length;
    //             Class = ui.draggable.attr("class");
    //             title = ui.draggable.text().trim();
    //            var item = $('<table class="foo elementTable device_list_name' + (diagramLenght + 1) + ' ' + Class + ' name="' + title + '" id="' + (diagramLenght + 1) + '" style="left: ' + droppedLeft + 'px; top: ' + droppedTop + 'px;"><tr class="tableBody tableHeader"  id="tower_' + (diagramLenght + 1) + '"><th class="thClass"><span style="margin-left:22px;" value="a" class="header' + (diagramLenght + 1) + '">' + title + (diagramLenght + 1) + '</span><span class="GpsSetting" id="GpsSetting"><img src="/Icons/pignon.png" width="17px" height="17px" alt="Settings" title="Settings" /></span></th></tr><tr><td class="add' + (diagramLenght + 1) + '"><ul id="dropDivs" name="dropDivs"class="ui-droppable"></ul></td></tr></table>');
    //            $(this).append(item);
    //            diagramLenght++;
    //            //  foo();
    //            addPoints();
    //            saveDiagram();
    //        }

    //        $(".tower").droppable({
    //            containment: '.foo',
    //            drop: function (ev, ui) {
    //                //diagramLenght = $('.foo').length;
    //                var con = $('#tower_' + $(this).attr("id")).attr("class");
    //                if (!ui.draggable.hasClass('foo') && !ui.draggable.hasClass('display') && ~con.indexOf("jtk-connected")) {
    //                    var pos = ui.draggable.offset(), dPos = $(this).offset();
    //                    var droppedTop = ui.position.top - $(this).offset().top + $('#mainDiv').scrollTop();
    //                    var droppedLeft = ui.position.left - $(this).offset().left + $('#mainDiv').scrollLeft();

    //                    if (!ui.draggable.hasClass('foo') && ui.draggable.hasClass('device_list')) {
    //                        var Class = ui.draggable.attr("class");
    //                         idTarget = ev.target.id;
    //                         title = ui.draggable.text().trim(35);
    //                        var height = $(".add" + idTarget).height();
    //                        var lenght = $('#dropDivs').length;
    //                        var id = $('.foo').length;

    //                        $('#IPModal').css("display", "block");
    //                        $('#IPModal').css("opacity", "1");

    //                        if (title.length > 35) {
    //                            var titleName = title.substr(0, title.length - (title.length - 31)) + '...';
    //                        }
    //                        else {
    //                            titleName = title;
    //                        }
    //                        droppedTop = height;
    //                        var item = $('<table  class="foo elementTable tableBodyTower tower_name' + (id + 1) + '" ' + Class + '" name="' + title + '" id="' + (id + 1) + '" title="Tower' + idTarget + '" style="left: ' + 3 + 'px; top: ' + droppedTop + 'px;min-width: 245px;"><tr class="tableBodyTowers tableHeader" style="display:inline-table" id="towerDevice' + (idTarget) + '_' + (id + 1) + '"><th class="thClass"><span value="' + title + '" class="device_header' + (id + 1) + '">' + titleName + '</span><span class="device_settings"><img src="/Icons/pignon.png" width="17px" height="17px" alt="Settings" title="Settings"data-toggle="modal" data-target="#myModal" /></span></th></tr></table>');

    //                        $(".add" + idTarget).height(height + 35);
    //                        $(".add" + idTarget).append(item);
    //                        //diagramLenght++;
    //                        //foo();
    //                        //addPointsTower();
    //                        saveDiagram();
    //                        //$("table tbody").sortable({
    //                        //    update: function (event, ui) {
    //                        //        $(this).children().each(function (index) {
    //                        //            $(this).find('td').last().html(index + 1)
    //                        //        });
    //                        //    }
    //                        //});
    //                    }
    //                }
    //            }
    //        });
    //    }
    //});

    //function getElemByCoordinates(x, y) {
    //    elem = document.elementFromPoint(x, y);
    //}

    //$('.device_list').draggable();


    //checkcity = $('.rowCheckbox');
    //checkcity.on("click", "div", function (e) {

$(document).on('click touchend', '.minimized', function () { // add device setting open
    var minimizeID = $(this).closest($(".foo")).attr("id");
    if ($('.add' + minimizeID).css("display") == "none") {
        $('.add' + minimizeID).css("display", "block");
        $('#minimizedImg' + minimizeID).css("transform", "rotate(0deg)");
    }
    else {
        $('.add' + minimizeID).css("display", "none");
        $('#minimizedImg' + minimizeID).css("transform", "rotate(180deg)");
    }
    
});

    $('body').on('click touched', '#city_check div', function () {
        var cityid = $(this).attr("id");

        if ($('#city_checked' + cityid).is(':checked') == false) {
            $('#checked_add' + cityid).removeClass("").addClass("checked");
            $("#city_checked" + cityid).prop('checked', true);

            var addnewcity = '<tr class="tableBody  city_remove' + cityid + '_' + $(this).parent().attr("value") + '" id="city_' + ($('.tableConn').length + 1) + '" ><td class="tableConn">' + $('#city_checked' + cityid).val() + '</td></tr>';
            $(addnewcity).insertBefore('.add' + parentId);

            addPoints();
            saveDiagram();

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
            towerDeleteID = $(this).parent().attr("value");
            $.post("/DeviceGroup/TowerDelete", { towerDeleteID: towerDeleteID, cityid: cityid }, function () {
                // saveDiagram();
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
                        connectorStyle: { stroke: "grey", strokeWidth: 3 },
                        connectorHoverStyle: { lineWidth: 3 },
                        maxConnections: 1,
                        uuid: $(this).attr("id") + 't'
                    });
                }
            }
        });
        setImage();
       // saveDiagram();
    }

    function setImage() { //Setting image to endpoints without connection
        var eps = $('.blankEndpoint');
        eps.each(function () {
            if (!$(this).hasClass('jtk-endpoint-connected')) {
                $(this).css({ 'min-width': '18px', 'min-height': '18px', 'background': 'url(/image/no_connection_radiobutton.png) no-repeat', 'background-size': '100%', 'margin-left': '-9px', 'margin-top': '-9px', 'z-index': '50' });
            }
        });
        // saveDiagram();
    }


    function setImageOnConnection() {
        $('.jtk-endpoint-connected').css({ 'min-width': '18px', 'min-height': '18px', 'background': 'url(/image/connected_radiobutton.png) no-repeat', 'background-size': '100%', 'margin-left': '-9px', 'margin-top': '-9px' });
    }

    jsPlumb.bind("connection", function (connection) {
        setImage();
        setImageOnConnection();
        towerConnection = 1;
        cityID = connection.sourceId;
        if (connectaddremove == 0) {
            SavePoint();
            saveDiagram();
        }
        $('.header' + $('#' + connection.targetId).parent().parent().attr("id")).text($('#' + connection.sourceId).text() + "_" + "Tower" + $('#' + connection.targetId).parent().parent().attr("id"));
    });

    jsPlumb.bind("connectionDetached", function (connection) {
        connection.sourceEndpoint.removeClass('jtk-endpoint-connected');
        setImage();
        $('.header' + $('#' + connection.targetId).parent().parent().attr("id")).text("Tower" + $('#' + connection.targetId).parent().parent().attr("id"));
        var sourceID = connection.sourceId;
        var targetID = connection.targetId;
        $.post("/DeviceGroup/UnConnection", { sourceID: sourceID, targetID: targetID }, function () {
            saveDiagram();
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

    function SavePoint() {
        ConnectionPoint();
        $.post("/DeviceGroup/PointConnections", { connections: connections, UnConnectio: UnConnectio }, function (Response) {

        }, 'json');
    }

    function saveDiagram() {

        $(".tableBody").removeClass("jtk-endpoint-anchor");
        connectaddremove = 0;
        filedata = new FormData();
        filedata.append("connect[]", connections);
        filedata.append("Html", $('#mainDiv').html());

        $.ajax({
            type: 'post',
            contentType: false,
            processData: false,
            data: filedata,
            dataType: 'json',
            url: '/DeviceGroup/SaveDiagram',
            success: function (Response) {
                $(".tableBody").addClass("jtk-endpoint-anchor");
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
        $.post("/Trap/SendTrap", {}, function () { });
        connectaddremove = 1;
        $("#mainDiv").html("");
        $.post("/DeviceGroup/LoadDiagram", {}, function (Response) {
            $("#mainDiv").html(Response.htmlData);
            $("#mainDiv .foo").draggable();
            removePointsAndConnections();
            addPoints();
            $.each(Response.pointData, function (i, item) {
                var pointright = item.PointRight;
                var pointLeft = item.PointLeft;
                jsPlumb.connect({
                    uuids: [pointright.toString(), pointLeft.toString()]
                });
            });
            saveDiagram();
        });
        
    }