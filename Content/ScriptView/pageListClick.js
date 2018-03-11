
$('#page_list').on('click', 'a', function (e) {
    e.preventDefault();
    $.ajax({
        url: this.href,
        type: 'GET',
        cache: false,
        success: function (result) {
            $('#device_settings').html("");
            $('#device_settings').html(result);
        }
    }, 'text');
});