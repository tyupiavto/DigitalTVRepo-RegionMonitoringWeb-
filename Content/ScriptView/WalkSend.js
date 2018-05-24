//var OIDName, IpAddress, PresetName, i, click_ID, SearchName, SetOID, SetValue, IntervalID, DeleteID, page,listV,IP,search_id,presetName,preset_edit_search;
//var CheckArray = new Array();
//var walkArray = new Array();
//var array = new Array();
//var ChekedList = new Array();
//var TimeChange = new Array();
//var tm = new Array();
//var UnChecked = new Array();
////var search_time, val;
////var click_checked;
//$(function () {

//    $('#WalkSend').click(function () {

//        $.ajax({
//            type: 'POST',
//            data: {
//            },
//            dataType: 'text',
//            url: '/Walk/WalkSend',
//            success: function (Response) {
//                $('#walkdraw').html("");
//                $('#walkdraw').html(Response);
//            }
//        });
//    });

//    $('#savewalk').on('click', function () {
//        PresetName = $('#WalkSaveName').val();
//        //walkArray = [];
//        //$('input[type=checkbox]').each(function () {

//        //    if (this.checked) {
//        //        i = $(this).val();
//        //        var oid = '.';
//        //        //oid += $('#OIDName' + i).text();
//        //        array.push(i);
//        //        array.push($('#Time' + i).val());
//        //        //array.push($('#Value' + i).text());
//        //        walkArray.push(array);
//        //        array = [];
//        //    }
//        //});
//        IpAddress = $('#device_ip').text();
//        DeviceName = $('#DeviceID').val();
//        $.ajax({
//            type: 'POST',
//            data: { ChekedList: ChekedList, TimeChange: TimeChange, UnChecked: UnChecked, 'PresetName': PresetName, 'IpAddress': IpAddress},
//            dataType: 'json',
//            url: '/Walk/WalkSave',
//            success: function (Response) {
//                $('#WalkSaveName').val("");

//            }
//        });
//    });


//    var WalkPage;
//    WalkPage = $("#check_body");
//    WalkPage.on("click", "tr", function (e) {
//        click_ID = $(this).data("id");
//        if (e.target.id == "set_click") {
//            click_ID = $(this).data("id");
//            $('#descriptionOID').text($('#OIDName' + click_ID).text());
//            $('#SetOID').val($('#oid_name' + click_ID).text());
//            $('#SetType').text($('#Value' + click_ID).text());
//            $('#SetDataType').text($('#type_name' + click_ID).text());
//        }
//        if (e.target.id == "check_click" + click_ID) {
                
//            if ($('#check_click' + click_ID).prop('checked')) {
//                ChekedList.push(click_ID);
//            }
//            else {
//                UnChecked.push(click_ID);
//            }
//        }
//        if (e.target.id == "Time" + click_ID) {
//            $('#Time' + click_ID).change(function () {
//                tm.push(click_ID);
//                tm.push($('#Time' + click_ID).val());
//                TimeChange.push(tm);
//                tm = [];
//            });
//        }

//    });

//    $('#SendSet').click(function () {
//        SetOID = $('#SetOID').val();
//        SetValue = $('#SetValue').val();
//        $.ajax({
//            type: 'POST',
//            data: {
//                'SetOID': SetOID, 'SetValue': SetValue
//            },
//            dataType: 'json',
//            url: '/Walk/SetSend',
//            success: function (Response) { }
//        });
//    });

//    $('.pagination-container').click(function () { /// next page 

//        $.ajax({
//            type: 'POST',
//            data: { ChekedList: ChekedList, TimeChange: TimeChange, UnChecked: UnChecked },
//            dataType: 'json',
//            url: '/Walk/clickchecked',
//            success: function (Response) {
//            }
//        });
//    });

//    $('#ListView').change(function () {
//        listV = $('#ListView').val();
//        //$('input[type=checkbox]').each(function () {

//        //    if (this.checked) {
//        //        click_ID = $(this).val();
//        //        ChekedList.push(click_ID);
//        //        tm.push(click_ID);
//        //        tm.push($('#Time' + click_ID).val());
//        //        TimeChange.push(tm);
//        //        tm = [];
//        //    }
//        //});
//        $.ajax({
//            type: 'POST',
//            data: { ChekedList: ChekedList, TimeChange: TimeChange, UnChecked: UnChecked, 'listV': listV },
//            dataType: 'text',
//            url: '/Walk/ListView',
//            success: function (Response) {
//                window.location.href = "/Walk/Index";
//            }
//        });
//    });

//    $('#GoWalk').click(function () {
//        presetName = $('#preset_add').val();
//        TowerName = $('#TowerID').val();
//        IP = $('#DevicePanelIP').val();
//        DeviceName = $('#DeviceID').val();
//        $.ajax({
//            type: 'POST',
//            data: {
//                'presetName': presetName, 'TowerName': TowerName, 'IP': IP, 'DeviceName': DeviceName
//            },
//            dataType: 'text',
//            url: '/Walk/WalkSend',
//            success: function (Response) {
//                $('#SearchText').val("");
//                //$('#walkdraw').html("");
//                //$('#walkdraw').html(Response);
//                window.location.href = "/Walk/Index";
//            }
//        });
//    });

//    $('#search_click').click(function () {
//        SearchName = $('#SearchText').val();
//        $.ajax({
//            type: 'POST',
//            data: {
//                'SearchName': SearchName, ChekedList: ChekedList, TimeChange: TimeChange, UnChecked: UnChecked
//            },
//            dataType: 'text',
//            url: '/Walk/SearchList',
//            success: function (Response) {
//                $('#walkdraw').html("");
//                $('#walkdraw').html(Response);
//                window.location.href = "/Walk/Index";

//            }
//        });
//    });

//    $('#preset_edit').click(function () {
//        presetName = $('#preset_inf').val();

//        $.ajax({
//            type: 'POST',
//            data: {
//                'presetName': presetName
//            },
//            dataType: 'text',
//            url: '/Walk/WalkEditPreset',
//            success: function (Response) {
//                $('#walkdraw').html("");
//                $('#walkdraw').html(Response);
    
//                window.location.href = "/Walk/Index";

//            }
//        });
//    });

//    $('#selected_search').change(function () {
//        presetName = $('#selected_search').val();
//        $.ajax({
//            type: 'POST',
//            data: {
//                'presetName': presetName
//            },
//            dataType: 'text',
//            url: '/Walk/WalkEditPreset',
//            success: function (Response) {
//                $('#walkdraw').html("");
//                $('#walkdraw').html(Response);

//                window.location.href = "/Walk/Index";

//            }
//        });
//    });
//    $('#presetEdit_save').on('click', function () {
//        PresetName = $('#WalkSaveName').val();

//        IpAddress = $('#device_ip').text();
//        DeviceName = $('#DeviceID').val();
//        $.ajax({
//            type: 'POST',
//            data: { ChekedList: ChekedList, TimeChange: TimeChange, UnChecked: UnChecked, 'PresetName': PresetName, 'IpAddress': IpAddress },
//            dataType: 'json',
//            url: '/Walk/WalkSave',
//            success: function (Response) {
//                $('#WalkSaveName').val("");
//                window.location.href = "/Tower/Tower";

//            }
//        });
//    });

//});