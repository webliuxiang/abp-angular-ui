var CurrentPage = function () {

    var handleRegister_result = function () {



        // 更新验证码
        var $valType = 3;
        var $val_code_img = $("#val_code_img");
        var imgApiurl = app.consts.Verification.url + '?type=' + $valType + '&t=' + Date.parse(new Date());
        $val_code_img.attr('src', imgApiurl);
        $val_code_img.click(function () {
            $val_code_img.attr('src', imgApiurl);
        });



        var $reSendCodeForm = $("#reSendCodeForm");
        var $btnReSend = $("#btnReSend");
        var $time = 59;
        var $timer = setInterval(canUse, 1000);
        var $btnReSendTxt = $btnReSend.text();
        var $canClick = false;


        $btnReSend.click(function () {
            if (!$canClick) {
                return;
            }

            abp.ui.setBusy(
                null,
                abp.ajax({
                    contentType: abp.formex.contentTypeUrl,
                    url: $reSendCodeForm.attr('action'),
                    data: $reSendCodeForm.serializeFormToObject()
                }).done(function (data) {
                    abp.message.success('新的激活邮件已发送!');
                    $val_code_img.click();

                    $canClick = false;
                    $time = 59;
                    $timer = setInterval(canUse, 1000);
                }).fail(function () {
                    $val_code_img.click();
                })
            );
        });


        function canUse() {
            $time--;
            if ($time <= 0) {
                clearInterval($timer);
                $canClick = true;
                $btnReSend.text($btnReSendTxt);
            } else {
                $btnReSend.text($btnReSendTxt + " (" + $time + ")");
            }
        }
    }


    return {
        init: function () {
            handleRegister_result();
        }
    };

}();

