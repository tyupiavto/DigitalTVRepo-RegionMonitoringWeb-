var countrieSearchName, StateName, CountrieName, citySearchName;
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
    var parentId = $(this).closest($(".foo")).attr("id");
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