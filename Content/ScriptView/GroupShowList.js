//var deviceGroupList;
//var LInd = 0;
////device_list = $('#deviceName');
//$('#deviceName').on("click touched", "li", function () {
//    deviceGroupList = $(this).attr("value");
//    if (LInd == 0) {
//        $('#deviceGroupName' + deviceGroupList).animate({ height: 'toggle', opacity: 'toggle' }, 300);
//        LInd = 1;
//    }
//    $('#deviceGroupName' + deviceGroupList).animate({ height: 'toggle', opacity: 'toggle' }, 300);
//    $.post("/DeviceGroup/DeviceList", { deviceGroupList: deviceGroupList }, function (Response) {
//        $('#deviceGroupName' + deviceGroupList).html("");
//        $('#deviceGroupName' + deviceGroupList).html(Response);
//    }, 'text');
//});
//$('#deviceGroupName' + deviceGroupList + ' li').draggable({
//    helper: 'clone',
//    revert: 'invalid'
//});
//$('#deviceName').on("click", function () { $('#deviceGroupName' + deviceGroupList).animate({ height: 'toggle', opacity: 'toggle' }, 300); });
////$('#group_name2').on("click",function () {
//    $('#deviceGroupName' + deviceGroupList).animate({ height: 'toggle', opacity: 'toggle' }, 300);
//    $('.devInfo').css("max-height", "500px");
//});
//var device_drag; var id;
//device_drag = $('#deviceName');
//device_drag.on("click", "ul li", function (e) {
//    alert($(this).attr("value"));
//    id = $(this).attr("value");
//});

  //$('.device_list').draggable({
  //      helper: 'clone',
  //      revert: 'invalid'
  //  });
//$('#device_list').draggable({
//    helper: 'clone',
//    revert: 'invalid'
//});

//var diagramLenght = $('.foo').length;
//$("#mainDiv").droppable({
//    drop: function (ev, ui) {
//        diagramLenght = $('.foo').length;
//        //if (!ui.draggable.hasClass('foo') && !ui.draggable.hasClass('display')) {
//        var pos = ui.draggable.offset(), dPos = $(this).offset();
//        var droppedTop = ui.position.top - $(this).offset().top + $('#mainDiv').scrollTop();;
//        var droppedLeft = ui.position.left - $(this).offset().left + $('#mainDiv').scrollLeft();;

//        if (!ui.draggable.hasClass('foo') && ui.draggable.hasClass('deviceGroupName2')) {
//            var Class = ui.draggable.attr("class");
//            var title = ui.draggable.text().trim();
//            var item = $('<table class="foo elementTable ' + Class + '" name="' + title + '" id="' + (diagramLenght + 1) + '" style="left: ' + droppedLeft + 'px; top: ' + droppedTop + 'px;"><tr class="tableHeader"><th class="thClass"><span class="header' + (diagramLenght + 1) + '">' + title + '</span><span class="settings"><span id="state' + (diagramLenght + 1) + '"style="display:none">State</span><img src="/Icons/pignon.png" width="17px" height="17px" alt="Settings" title="Settings" /></span></th></tr><tr><td class="add' + (diagramLenght + 1) + '"><span class="addList' + (diagramLenght + 1) + '"></span></td></tr></table>');
//            $(this).append(item);
//            diagramLenght++;
//            foo();
//            //saveDiagram();
//        }
//    }
//});
