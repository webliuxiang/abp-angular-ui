(function ($) {

    $(function () {



        var $loginForm = $('#loginForm');
        var $rememberMe = $("#Login_RememberMe");



        $loginForm.submit(function (e) {
            //  console.log("����س���");
            e.preventDefault();
            //  console.log("����س���+11");
            if (!$loginForm.valid()) {
                return;
            }

            //if ($rememberMe.val() === "on") {
            //    $rememberMe.val(true);
            //}
            var input = $loginForm.serializeFormToObject();

            console.log(input);

            abp.ui.setBusy(
                null,
                abp.ajax({
                    contentType: abp.formex.contentTypeUrl,
                    url: $loginForm.attr('action'),
                    data: $loginForm.serializeFormToObject()
                }).fail(function () {
                    //    googleToekRefresh();
                    //   $val_code_img.click();
                })
            );
        });



    });

})(jQuery);


