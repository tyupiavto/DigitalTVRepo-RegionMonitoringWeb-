var countrieSearchName, StateName, CountrieName, citySearchName,parentId;
//var handled = false;
//$('body').on('click touchend', '#typeSelect', function (e) {
//    var width = $(this).width();
//    if (e.type == "touchend") {
//        handled = true;
//        $('#typeDropDownList').css({ 'width': width + 12 }).toggle();
//    }
//    else
//        if (e.type == "click" && !handled) {
//            $('#typeDropDownList').css({ 'width': width + 12 }).toggle();
//        }
//        else {
//            handled = false;
//        }

//});
//$('body').on('click touchend', '#typeDropDownList li', function () {
//    var value = $(this).text();
//    CountrieName = value;
//    $.post("/DeviceGroup/Countries", { CountrieName: CountrieName, StateName: StateName, CityName: CityName }, function (Response) {
//        $('#settingsDiv').html("");
//        $('#settingsDiv').html(Response);
//        $('#typeSelect .type').text(value);
//    }, 'text');
    
//});

//var handledstate = false;
//$('body').on('click touchend', '#typeSelectState', function (e) {
//    var width = $(this).width();
//    if (e.type == "touchend") {
//        handledstate = true;
//        $('#typeDropDownListState').css({ 'width': width + 12 }).toggle();
//    }
//    else
//        if (e.type == "click" && !handledstate) {
//            $('#typeDropDownListState').css({ 'width': width + 12 }).toggle();
//        }
//        else {
//            handledstate = false;
//        }

//});

//$('body').on('click touchend', '#typeDropDownListState li', function () {
//    var value = $(this).text();
//    $('#typeSelectState .type').text(value);
//    $('#typeDropDownListState').toggle();

//});

//var handledcity = false;
//$('body').on('click touchend', '#typeSelectCity', function (e) {
//    var width = $(this).width();
//    if (e.type == "touchend") {
//        handledcity = true;
//        $('#typeDropDownListCity').css({ 'width': width + 12 }).toggle();
//    }
//    else
//        if (e.type == "click" && !handledcity) {
//            $('#typeDropDownListCity').css({ 'width': width + 12 }).toggle();
//        }
//        else {
//            handledcity = false;
//        }

//});

//$('body').on('click touchend', '#typeDropDownListCity li', function () {
//    var value = $(this).text();
//    $('#typeSelectCity .type').text(value);
//    $('#typeDropDownListCity').toggle();

//});
//$(document).on('click touchend', '.settings', function () { // add device setting open
//    var parentId = $(this).closest($(".foo")).attr("id");
//    $.post("/DeviceGroup/Countries", {}, function (Response) {
//        $('#settingsDiv').html("");
//        $('#settingsDiv').html(Response);
//    }, 'text');

//    //}
//});

$(document).on('click touchend', '.settings', function () { // add device setting open
    parentId = $(this).closest($(".foo")).attr("id");
    $.post("/DeviceGroup/Countries", {}, function (Response) {
        $('#settingsDiv').html("");
        $('#settingsDiv').html(Response);
    }, 'text');

});
$('#countries_search').keyup(function () {
    countrieSearchName = $('#countries_search').val();
    if ($('#countries_search').val().length == 1) {
        $('#countries_search').val(countrieSearchName.toUpperCase());
        countrieSearchName = countrieSearchName.toUpperCase();
    }


    $.post("/DeviceGroup/countrieSearch", { countrieSearchName: countrieSearchName}, function (Response) {
        $('#countrie_partial').html("");
        $('#countrie_partial').html(Response);
    }, 'text');
});

$('#state_search').keyup(function () {
    stateSearchName = $('#state_search').val();
    if ($('#state_search').val().length == 1) {
        $('#state_search').val(stateSearchName.toUpperCase());
        stateSearchName = stateSearchName.toUpperCase();
    }
    $.post("/DeviceGroup/stateSearch", { CountrieName: CountrieName ,stateSearchName: stateSearchName }, function (Response) {
        $('#state_partial').html("");
        $('#state_partial').html(Response);
    }, 'text');
});

$('#city_search').keyup(function () {
    citySearchName = $('#city_search').val();
    if ($('#city_search').val().length == 1) {
        $('#city_search').val(citySearchName.toUpperCase());
        citySearchName = citySearchName.toUpperCase();
    }
    $.post("/DeviceGroup/citySearch", { StateName: StateName, citySearchName: citySearchName }, function (Response) {
        $('#city_partial').html("");
        $('#city_partial').html(Response);
    }, 'text');
});


var checkcity;
checkcity = $('.rowCheckbox');
checkcity.on("click", "div", function (e) {
    var cityid = $(this).attr("id");
    if ($('#city_checked' + cityid).is(':checked') == false) {
        $('#checked_add'+ cityid).removeClass("").addClass("checked");
        $("#city_checked" + cityid).prop('checked', true);
        //alert("true");
    }
    else {
        $('#checked_add' + cityid).removeClass("checked").addClass("");
        $("#city_checked" + cityid).prop('checked', false);
        //alert("false");
    }

    var addnewcity = '<tr class="tableBody" id="' + $('#city_checked' + cityid).val() + '"><td class="tableConn">' + $('#city_checked' + cityid).val() + '</td></tr>';
    $(addnewcity).insertBefore('.addList' + parentId);
    addPoints(); 
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
            $(this).css({ 'min-width': '18px', 'min-height': '18px', 'background': 'url(/image/no_connection_radiobutton.png) no-repeat', 'background-size': '100%', 'margin-left': '-9px', 'margin-top': '-9px' });
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
//$('#checked_add2').click(function () {
    
//    if ($('#city_checked2').is(':checked') == false) {
//        $('#checked_add2').removeClass("").addClass("checked");
//        $("#city_checked2").prop('checked', true);
//        //alert("true");
//    }
//    else {
//        $('#checked_add2').removeClass("checked").addClass("");
//        $("#city_checked2").prop('checked', false);
//        //alert("false");
//    }
//});
