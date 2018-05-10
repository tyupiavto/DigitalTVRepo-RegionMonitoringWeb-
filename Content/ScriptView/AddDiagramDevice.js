var idTarget, title,deviceID;
function foo() {
    $('.foo').each(function () {
        //Making dropped elements draggable again
        $(this).draggable({
            //containment: $(this).parent(),
            stack: '.foo',
            grid: [10, 10],
            drop: function (event, ui) {
                var pos = ui.draggable.offset(), dPos = $(this).offset();
               // saveDiagram();
            },
            drag: function () {
                //jsPlumb.addToDragSelection($('.foo'));
                var draggedItemId = $(this).attr('id');
                $('.contextmenu').hide();
                $(this).css({ 'z-index': '11' });
                $(".class" + draggedItemId).css({ 'z-index': '15' });
              //  saveDiagram();
            },
            stop: function () {
                var draggedItemId = $(this).attr('id');
                $(this).css({ 'z-index': '0' });
                $(".class" + draggedItemId).css({ 'z-index': '10' });
                if ($(this).position().left < 0) {
                    $(this).css({ 'left': '0' });
                }
                if ($(this).position().top < 0) {
                    $(this).css({ 'top': '0' });
                }
                $('#settingsDiv').html("");
                saveDiagram();
            }
        });
    });
}

var xCoordinate = 0;
var yCoordinate = 0;
var elem;
var diagramLenght = $('.foo').length;
$("#mainDiv").droppable({
    drop: function (ev, ui) {

        //if (!ui.draggable.hasClass('foo') && !ui.draggable.hasClass('display')) {
        var pos = ui.draggable.offset(), dPos = $(this).offset();
        var droppedTop = ui.position.top - $(this).offset().top + $('#mainDiv').scrollTop();
        var droppedLeft = ui.position.left - $(this).offset().left + $('#mainDiv').scrollLeft();
        var ids = $(this).attr("id");

        if (!ui.draggable.hasClass('foo') && ui.draggable.hasClass('countries')) {
            diagramLenght = $('.foo').length;
            var Class = ui.draggable.attr("class");
            title = ui.draggable.text().trim();
            var item = $('<table class="foo elementTable ' + Class + '" name="' + title + '" id="' + (diagramLenght + 1) + '" style="left: ' + (droppedLeft + 4) + 'px; top: ' + (droppedTop - 1) + 'px;"><tr class="tableHeader"><th class="thClass"><span style="margin-left:25px;" class="header' + (diagramLenght + 1) + '">' + title + '</span><span class="settings"><span id="state' + (diagramLenght + 1) + '"style="display:none">State</span><img src="/Icons/pignon.png" width="17px" height="17px" alt="Settings" title="Settings" /></span></th></tr><tr><td class="add' + (diagramLenght + 1) + '"><span class="addList' + (diagramLenght + 1) + '"></span></td></tr></table>');
            $(this).append(item);
            diagramLenght++;
            $('#settingsDiv').html("");
            foo();
            saveDiagram();
        }
        if (!ui.draggable.hasClass('foo') && ui.draggable.hasClass('tower')) {
            diagramLenght = $('.foo').length;
            var Class = ui.draggable.attr("class");
            title = ui.draggable.text().trim();
            var item = $('<table class="foo elementTable device_list_name' + (diagramLenght + 1) + ' ' + Class + ' name="' + title + '" id="' + (diagramLenght + 1) + '" style="left: ' + droppedLeft + 'px; top: ' + droppedTop + 'px;"><tr class="tableBody tableHeader"  id="tower_' + (diagramLenght + 1) + '"><th class="thClass"><span style="margin-left:22px;" class="header' + (diagramLenght + 1) + '">' + title + (diagramLenght + 1) + '</span><span class="GpsSetting" id="GpsSetting"><img src="/Icons/pignon.png" width="17px" height="17px" alt="Settings" title="Settings" /></span><span class="minimized" id="minimized"><img src="/image/arrow_drop_down_white.png" width="17px" height="17px" alt="Settings" title="Minimize" id="minimizedImg' + (diagramLenght + 1) + '"/></span></th></tr><tr><td class="add' + (diagramLenght + 1) + '"><ul id="deviceAdd' + (diagramLenght + 1) + '" name="dropDivs"class="ui-droppable"></ul></td></tr></table>');
            $(this).append(item);
            diagramLenght++;
            $('#settingsDiv').html("");
            foo();
            addPoints();
            saveDiagram();
        }

        $(".tower").droppable({
            containment: '.foo',
            drop: function (ev, ui) {
                //diagramLenght = $('.foo').length;
                var con = $('#tower_' + $(this).attr("id")).attr("class");
                if (!ui.draggable.hasClass('foo') && !ui.draggable.hasClass('display') && ~con.indexOf("jtk-connected")) {
                    var pos = ui.draggable.offset(), dPos = $(this).offset();
                    var droppedTop = ui.position.top - $(this).offset().top + $('#mainDiv').scrollTop();
                    var droppedLeft = ui.position.left - $(this).offset().left + $('#mainDiv').scrollLeft();

                    if (!ui.draggable.hasClass('foo') && ui.draggable.hasClass('device_list')) {
                        var Class = ui.draggable.attr("class");
                        idTarget = ev.target.id;
                        title = ui.draggable.text().trim(35);
                        var height = $(".add" + idTarget).height();
                        var lenght = $('#dropDivs').length;
                        var id = $('.foo').length;
                        deviceID = id + 1;
                        if (title.length > 35) {
                            var titleName = title.substr(0, title.length - (title.length - 31)) + '...';
                        }
                        else {
                            titleName = title;
                        }
                        droppedTop = height;
                        var item = $('<table class="foo elementTable tableBodyTower tower_name' + (id + 1) + '" ' + Class + '" name="' + title + '" id="' + (id + 1) + '" title="tower_' + idTarget + '" style="left: ' + 3 + 'px; top: ' + droppedTop + 'px;min-width: 245px;"><tr class="tableBodyTowers tableHeader" style="display:inline-table" id="towerDevice' + (idTarget) + '_' + (id + 1) + '"><th class="thClass"><span value="' + title + '" class="device_header' + (id + 1) + '">' + titleName + '</span><span class="device_settings"><img src="/Icons/pignon.png" width="17px" height="17px" alt="Settings" title="Settings"data-toggle="modal" data-target="#myModal" /></span></th></tr></table>');
                        $(".add" + idTarget).height(height + 35);
                        $(".add" + idTarget).append(item);
                        $('#IPModal').modal("show");
                        $('#IPModal').css("margin-top", "" + dPos.top + "px");
                        $('#IPModal').css("margin-left", "" + dPos.left - 100 + "px");
                        diagramLenght++;
                        $('#settingsDiv').html("");
                        foo();
                        saveDiagram();
                    }
                }
            }
        });
    }
});

$('body').on('click tounched', '#add_device_inf', function () {
    IPaddress = $('#ip_address').val();
    towerID = "tower_" + idTarget;
    deviceName = title;
    towerName = $('.header' + idTarget).text();
    //$('#existingDevice').modal("show");
    $.post("/DeviceGroup/TowerDeviceInformation", { IPaddress: IPaddress, towerID: towerID, deviceName: deviceName, cityID: cityID, towerName: towerName, deviceID: deviceID }, function (Response) {
        $('#ip_address').val("");
    }, 'json');
});

$('body').on('click tounched', '#SendFill', function () {
    $('#load_walk').css("display", "block");
    $.get("/Trap/TrapFillNewDevice", { IPaddress: IPaddress }, function () {
        $('#load_walk').css("display", "none");
    });
});

$('body').on('click tounched', '#notTrapFill', function () {
    $('#load_walk').css("display", "block");
    $.get("/Trap/NotTrapFill", { IPaddress: IPaddress }, function () {
        $('#load_walk').css("display", "none");
    });
});
