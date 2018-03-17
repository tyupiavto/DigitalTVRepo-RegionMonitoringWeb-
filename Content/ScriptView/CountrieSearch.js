var countrieSearchName, StateName, CountrieName, citySearchName, parentId, addcityName;
var countrieSettingName, stateSettingName;
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
    countrieSettingName = $('.header' + parentId).text();
    stateSettingName = $('#state' + parentId).text();
    $.post("/DeviceGroup/Countries", { parentId: parentId, countrieSettingName: countrieSettingName, stateSettingName: stateSettingName }, function (Response) {
        $('#settingsDiv').html("");
        $('#settingsDiv').html(Response);
    }, 'text');

});




$('#countries_search').keyup(function () {
    countrieSearchName = $('#countries_search').val();
    //if ($('#countries_search').val().length == 1) {
    //    //$('#countries_search').val(countrieSearchName.toUpperCase());
    //    countrieSearchName = countrieSearchName.toUpperCase();
    //}


    $.post("/DeviceGroup/countrieSearch", { countrieSearchName: countrieSearchName}, function (Response) {
        $('#countrie_partial').html("");
        $('#countrie_partial').html(Response);
    }, 'text');
});

$('#state_search').keyup(function () {
    stateSearchName = $('#state_search').val();
    CountrieName = $('#countrieName').text();

    $.post("/DeviceGroup/stateSearch", { CountrieName: CountrieName ,stateSearchName: stateSearchName }, function (Response) {
        $('#state_partial').html("");
        $('#state_partial').html(Response);
    }, 'text');
});

$('#city_search').keyup(function () {
    CountrieName = $('#countrieName').text();
    StateName=$('#state_search').val();
    citySearchName = $('#city_search').val();

    $.post("/DeviceGroup/citySearch", { CountrieName: CountrieName, StateName: StateName, citySearchName: citySearchName }, function (Response) {
        $('#city_partial').html("");
        $('#city_partial').html(Response);
    }, 'text');
});

$('#city_list').click(function () {
    if ($("#city_all").css("display") == "block") {
        $('#city_allselect').css("display", "none");
    } else {
        $('#city_allselect').css("display", "block");
    }
});

$('#city_all').click(function () {
    $('#name_all').text("All");
    $('#city_allselect').css("display", "none");
    //$('#city_select').css("display", "none");
    var selectallName = "All";
    stateName = $('#state').text();
    $.post("/DeviceGroup/SelectAll", { selectallName: selectallName, stateName: stateName }, function (Response) {
        $('#city_partial').html("");
        $('#city_partial').html(Response);
    }, 'text');

});

$('#city_select').click(function () {
    $('#name_all').text("Select");
    //$('#city_all').css("display", "none");
    $('#city_allselect').css("display", "none");
    var selectallName = "Select";
    stateName = $('#state').text();
    $.post("/DeviceGroup/SelectAll", { selectallName: selectallName, stateName: stateName }, function (Response) {
        $('#city_partial').html("");
        $('#city_partial').html(Response);
    }, 'text');
});

//$('#city_all').click(function () {
//    $('#name_all').text("All");
//    $('#city_all').css("display", "none");
//    $('#city_select').css("display", "none");
//    var selectallName = "All";
//    stateName = $('#state').text();
//    $.post("/DeviceGroup/SelectAll", { selectallName: selectallName, stateName: stateName }, function () {
//        alert(Response);
//        $('#city_partial').html("");
//        $('#city_partial').html(Response);
//    },'text');

//});

//$('#city_select').click(function () {
//    $('#name_all').text("Select");
//    $('#city_all').css("display", "none");
//    $('#city_select').css("display", "none");
//    var selectallName = "Select";
//    stateName = $('#state').text();
//    $.post("/DeviceGroup/SelectAll", { selectallName: selectallName, stateName: stateName }, function () {
//        alert(Response);
//        $('#city_partial').html("");
//        $('#city_partial').html(Response);
//    },'text');
//});

$('#add_city').click(function () {
    addcityName = $('#city_name_add').val();

    $.post("/DeviceGroup/CityAdd", { StateName: StateName, addcityName:addcityName }, function (Response) {
        $('#city_partial').html("");
        $('#city_partial').html(Response);
        $('#city_name_add').val("");
    }, 'text');
});

var handled = false; var width;                                          // countries search and full countries
$('body').on('click touchend', '#typeSelect', function (e) {
    width = $(this).width()-5;
    if (e.type == "touchend") {
        handled = true;
        $('#typeDropDownList').css({ 'width': width + 12 }).toggle();
        $('.typeSelect li').css('widht', '93%');
    }
    else
        if (e.type == "click" && !handled) {
            $('#typeDropDownList').css({ 'width': width + 12 }).toggle();
            $.post("/DeviceGroup/countrieSearch", { CountrieName: CountrieName, StateName: StateName, CityName: CityName }, function (Response) {
                $('#countrie_partial').html("");
                $('#countrie_partial').html(Response);
            }, 'text');
        }
        else {
            handled = false;
        }
});

$('body').on('click touchend', '#typeDropDownList li', function () {
    var value = $(this).text();
    CountrieName = value;
    StateName = null;
    $('#countrieName').text(value);
    $('.header' + $('#typeDropDownList').attr('value')).text(value);
    $('#typeSelect #countrie').text(value);
    $('#typeDropDownList').css({ 'width': width + 12 }).toggle();
});

var handledstate = false;                                           // states search and full states
$('body').on('click touchend', '#typeSelectState', function (e) {
    var width = $(this).width()-5;
    if (e.type == "touchend") {
        handledstate = true;
        $('#typeDropDownListState').css({ 'width': width + 12 }).toggle();
    }
    else
        if (e.type == "click" && !handledstate) {
            $('#typeDropDownListState').css({ 'width': width + 12 }).toggle();
            CountrieName = $('#typeSelect #countrieName').text();
            //alert($('#countrie').attr("value"));
            //alert($('#countrie').text());
            $.post("/DeviceGroup/stateSearch", { CountrieName: CountrieName, CityName: CityName }, function (Response) {
                $('#state_partial').html("");
                $('#state_partial').html(Response);
            }, 'text');
        }
        else {
            handledstate = false;
        }

});

$('body').on('click touchend', '#typeDropDownListState li', function () {
    var value = $(this).text();
    StateName = value;
    $('#typeSelectState #state').text(value);
    $('#state' + $('#typeDropDownListState').attr("value")).text(value);
    $('#typeDropDownListState').css({ 'width': width + 12 }).toggle();

});

var handledcity = false;
$('body').on('click touchend', '#typeSelectCity', function (e) {
    var width = $(this).width()-5;
    if (e.type == "touchend") {
        handledcity = true;
        $('#typeDropDownListCity').css({ 'width': width + 12 }).toggle();
    }
    else
        if (e.type == "click" && !handledcity) {
            $('#typeDropDownListCity').css({ 'width': width + 12 }).toggle();
            CountrieName = $('#typeSelect #countrieName').text();
            StateName = $('#state').text();
            $.post("/DeviceGroup/citySearch", { CountrieName: CountrieName, StateName: StateName, CityName: CityName }, function (Response) {
                $('#city_partial').html("");
                $('#city_partial').html(Response);
            }, 'text');
        }
        else {
            handledcity = false;
        }

});
//$('#group_li' + $('#group_li').attr("value"))

//$('#group_li').click(function () {
//    $("#group_li").animate({ height: 'toggle', opacity: 'toggle' }, 300);
//});