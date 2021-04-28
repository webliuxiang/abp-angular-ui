"use strict";
// Class definition

var YoyosoftAppInit = function () {

    // Private functions
    var PreBootstrap = function () {
        // minimum setup
        $('.yoyo_select2').select2({
            theme: 'bootstrap4',
        });
    };

    return {
        // public functions
        init: function () {
            PreBootstrap();
        }
    };
}();

jQuery(document).ready(function () {
    YoyosoftAppInit.init();
});