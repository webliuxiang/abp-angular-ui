(function () {

    // 更新验证码
    function refreshImgCode() {
        var $valType = $("#codeType").val();
        var $valCodeImg = $("#val_code_img");
        var imgApiurl = app.consts.Verification.url + '?type=' + $valType + '&t=' + Date.parse(new Date());

        $valCodeImg.attr('src', imgApiurl);

        $valCodeImg.click(function () {
            refreshImgCode();
        });
    }




    refreshImgCode();

})();