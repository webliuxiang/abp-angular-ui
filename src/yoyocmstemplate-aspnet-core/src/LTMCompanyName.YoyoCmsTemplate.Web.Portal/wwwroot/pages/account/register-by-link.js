var CurrentPage = function () {

    var handleRegisterByLink = function () {

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
        var $timer = null;
        var $btnReSendTxt = $btnReSend.val();
        var $canClick = false;
        var $hasEmailAddress = $("#hasEmailAddress").val().toUpperCase() === "TRUE";

        $btnReSend.click(function () {
            if (!$canClick) {
                return;
            }

            if (!$reSendCodeForm.valid()) {
                return;
            }

            abp.ui.setBusy(
                null,
                abp.ajax({
                    contentType: abp.formex.contentTypeJson,
                    url: $reSendCodeForm.attr('action'),
                    data: JSON.stringify($reSendCodeForm.serializeJSON())
                }).done(function (data) {
                    abp.message.success('如果你输入的邮箱地址正确,激活邮件将会发送到你的邮箱!');
                    $canClick = false;
                    $time = 59;
                    $timer = setInterval(canUse, 1000);
                    $val_code_img.click();
                }).fail(function (error) {
                    $val_code_img.click();
                    abp.message.error(error.message);
                })
            );
        });

        function canUse() {
            $time--;
            if ($time <= 0) {
                clearInterval($timer);
                $canClick = true;
                $btnReSend.val($btnReSendTxt);
            } else {
                $btnReSend.val($btnReSendTxt + " (" + $time + ")");
            }
        }

        if ($hasEmailAddress) {
            $timer = setInterval(canUse, 1000);
        } else {
            $btnReSend.val("立即发送");
            $canClick = true;
        }


    }


    return {
        init: function () {
            handleRegisterByLink();
        }
    };

}();
