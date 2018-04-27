
$('#get_next').click(function () { 
$.ajax({
        type: 'POST',
        data: {
        },
        dataType: 'json',
        url: '/GetNext/Get',
        success: function (Response) {
        }
    });

})