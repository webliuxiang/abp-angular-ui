(function ($) {
    $.validator.setDefaults({
        errorElement: 'div',
        errorClass: 'invalid-feedback',
        focusInvalid: false,
        submitOnKeyPress: true,
        ignore:':hidden',
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-danger');
        },

        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-danger');
        },

        errorPlacement: function (error, element) {
            if (element.closest('.input-icon').length === 1) {
                error.insertAfter(element.closest('.input-icon'));
            } else {
                error.insertAfter(element);
            }
        },

        success: function (label) {
            label.closest('.form-group').removeClass('has-danger');
            label.remove();
        },

        submitHandler: function (form) {
            $(form).find('.alert-danger').hide();
        }
    });

    $.validator.addMethod("email", function (value, element) {
        return /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(value);
    }, "Please enter a valid Email.");
})(jQuery);
