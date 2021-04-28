﻿(function () {

    var _tenantChangeModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Account/TenantChangeModal',
        scriptUrl: abp.appPath + 'view-resources/Views/Shared/Components/TenantChange/_ChangeModal.js',
        modalClass: 'TenantChangeModal'
    });

    $('.tenant-change-component a')
        .click(function (e) {
            e.preventDefault();
         //第三个参数可改变模态框的大小
            _tenantChangeModal.open(null,null,"md");
        });

})();