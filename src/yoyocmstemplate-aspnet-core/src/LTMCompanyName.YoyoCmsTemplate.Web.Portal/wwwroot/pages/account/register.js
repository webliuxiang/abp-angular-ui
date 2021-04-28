
var CurrentPage = function() {


    var handelRegisterUser = function() {


        // 更新验证码
        var $val_code_img = $("#val_code_img");


        var $registerForm = $("#registerForm");

        // initialization of form validation
        $.HSCore.components.HSValidation.init(".js-validate",
            {
                rules: {
                    confirmPassword: {
                        equalTo: "#password"
                    }
                }
            });

        $registerForm.submit(function(e) {
            e.preventDefault();

            if (!$registerForm.valid()) {
                return;
            }

            abp.ui.setBusy(
                null,
                abp.ajax({
                    contentType: abp.formex.contentTypeUrl,
                    url: $registerForm.attr("action"),
                    data: $registerForm.serializeFormToObject()
                }).done(function(result) {
                    window.location.href = result;
                }).fail(function() {
                    $val_code_img.click();
                })
            );


        });


    };


    return {
        init: function() {
            handelRegisterUser();
        }
    };


}();