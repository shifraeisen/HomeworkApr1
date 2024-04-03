$(() => {
    $(".btn-dark").on('click', function () {

        $.get('/home/jsontest', function (obj) {
            console.log(obj);
        });

    });

    $("#email").on('keyup', function () {
        const value = $(this).val();

        $.get('/account/IsEmailAvailable', { email: value }, function (obj) {
            $(".btn-lg").prop('disabled', !obj.isAvailable);
        });
    });
})