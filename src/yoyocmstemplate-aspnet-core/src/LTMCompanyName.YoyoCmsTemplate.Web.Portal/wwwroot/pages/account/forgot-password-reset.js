var CurrentPage = function () {

    var handleForgotpasswordReset = function () {

       
        var $val_code_img = $("#val_code_img");
       

        var $resetPasswordForm = $('#resetPasswordForm');

        // initialization of form validation
        $.HSCore.components.HSValidation.init('.js-validate', {
            rules: {
                confirmPassword: {
                    equalTo: '#newPassword'
                }
            }
        });

        $resetPasswordForm.submit(function (e) {
            e.preventDefault();

            if (!$resetPasswordForm.valid()) {
                return;
            }
            var input = $resetPasswordForm.serializeFormToObject();

            abp.ui.setBusy(
                null,
                abp.ajax({
                    contentType: abp.formex.contentTypeUrl,
                    url: $resetPasswordForm.attr('action'),
                    data: input
                }).done(function (data) {
                    if (data) {
                        abp.message.success("用户密码修改成功!")
                            .then(function () {
                                window.location.href = "/";
                            });
                    }

                }).fail(function (error) {
                    $val_code_img.click();
                    abp.message.error(error.message);
                })
            );
        });

    }


    return {
        init: function () {
            handleForgotpasswordReset();
        }
    };

}();

