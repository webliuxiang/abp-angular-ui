"use strict";

// Class definition
var KTProfile = function () {
	// Elements
	var avatar;

	var _initForm = function() {
		avatar = new KTImageInput('kt_profile_avatar');
	}

	return {
		// public functions
		init: function() {
			_initForm();
		}
	};
}();

jQuery(document).ready(function() {
	KTProfile.init();
});
