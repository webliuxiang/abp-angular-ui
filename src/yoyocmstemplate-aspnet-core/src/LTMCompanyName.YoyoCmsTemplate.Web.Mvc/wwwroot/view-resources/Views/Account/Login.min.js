var CurrentPage = function () {

    var handleLogin = function () {
        var $loginForm = $('#login-signin');

        $loginForm.validate({
            rules: {
                username: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });

        $loginForm.find('input').keypress(function (e) {
            if (e.which == 13) {
                if ($('#login-signin').valid()) {
                    $('#login-signin').submit();
                }
                return false;
            }
        });

        $loginForm.submit(function (e) {
            e.preventDefault();

            if (!$('#login-signin').valid()) {
                return;
            }

            abp.ui.setBusy(
                null,
                abp.ajax({
                    contentType: app.consts.contentTypes.formUrlencoded,
                    url: $loginForm.attr('action'),
                    data: $loginForm.serialize()
                })
            );
        });

        $('a.social-login-icon').click(function() {
            var $a = $(this);
            var $form = $a.closest('form');
            $form.find('input[name=provider]').val($a.attr('data-provider'));
            $form.submit();
        });

        $loginForm.find('input[name=returnUrlHash]').val(location.hash);

        $('input[type=text]').first().focus();
    };

    return {
        init: function () {
            handleLogin();
        }
    };

}();