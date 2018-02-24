var countrieName,stateName, cityName;
var checkcity;
checkcity = $('.rowCheckbox');
checkcity.on("click", "div", function (e) {
    var cityid = $(this).attr("id");
    if ($('#city_checked' + cityid).is(':checked') == false) {
        $('#checked_add' + cityid).removeClass("").addClass("checked");
        $("#city_checked" + cityid).prop('checked', true);
        var addnewcity = '<tr class="tableBody" id="' + $('#city_checked' + cityid).val() + '"><td class="tableConn">' + $('#city_checked' + cityid).val() + '</td></tr>';
        $(addnewcity).insertBefore('.addList' + parentId);
        addPoints();
        cityName = $('#city_checked' + cityid).val();
        countrieName=$('#typeSelect #countrie').text();
        stateName = $('#typeSelectState #state').text();
        $.post("/DeviceGroup/TowerInsert", { countrieName: countrieName, stateName: stateName, cityName: cityName, cityid: cityid }, function () { });
    }
    else {
        $('#checked_add' + cityid).removeClass("checked").addClass("");
        $("#city_checked" + cityid).prop('checked', false);
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
            if (!$(this).hasClass('screen')) {
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
}

function setImage() { //Setting image to endpoints without connection
    var eps = $('.blankEndpoint');
    eps.each(function () {
        if (!$(this).hasClass('jtk-endpoint-connected')) {
            $(this).css({ 'min-width': '18px', 'min-height': '18px', 'background': 'url(/image/no_connection_radiobutton.png) no-repeat', 'background-size': '100%', 'margin-left': '-9px', 'margin-top': '-9px' ,'z-index':'1'});
        }
    });
}


function setImageOnConnection() {
    $('.jtk-endpoint-connected').css({ 'min-width': '18px', 'min-height': '18px', 'background': 'url(/image/connected_radiobutton.png) no-repeat', 'background-size': '100%', 'margin-left': '-9px', 'margin-top': '-9px' });
}

jsPlumb.bind("connection", function (connection) {
    setImage();
    setImageOnConnection();
});

jsPlumb.bind("connectionDetached", function (connection) {
    connection.sourceEndpoint.removeClass('jtk-endpoint-connected');
    setImage();
});
