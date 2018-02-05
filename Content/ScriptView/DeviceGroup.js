var GroupName;
$('#create_group').click(function () {
    GroupName = $('#group_name').val();
    $.ajax({
        type: 'POST',
        data: {
            'GroupName': GroupName
        },
        dataType: 'json',
        url: '/DeviceGroup/GroupCreate',
        success: function (Response) {

        }
    });
});