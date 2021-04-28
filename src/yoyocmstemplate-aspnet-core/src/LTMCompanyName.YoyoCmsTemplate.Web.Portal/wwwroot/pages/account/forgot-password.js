
var CurrentPage = function () {

    var handleForgetPassword = function () {
        var $val_code_img = $("#val_code_img");
     


        var $forgotPasswordForm = $('#forgotPasswordForm');
        if ($forgotPasswordForm) {
            $forgotPasswordForm.submit(function (e) {
                e.preventDefault();

                if (!$forgotPasswordForm.valid()) {
                    return;
                }

                var input = $forgotPasswordForm.serializeFormToObject();


                abp.ui.setBusy(
                    null,
                    abp.ajax({
                        contentType: abp.formex.contentTypeUrl,
                        url: $forgotPasswordForm.attr('action'),
                        data: input
                    }).done(function (data) {
                        window.location.href = data;
                    }).fail(function () {
                        $val_code_img.click();
                    })
                );
            });

        }



    }


    return {
        init: function () {
            handleForgetPassword();
        }
    };

}();




