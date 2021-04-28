(function () {
    $(function () {

        var _$editionsTable = $('#EditionsTable');
        var _editionService = abp.services.app.edition;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Editions.Create'),
            edit: abp.auth.hasPermission('Pages.Editions.Edit'),
            'delete': abp.auth.hasPermission('Pages.Editions.Delete'),
            moveTenantsToAnotherEdition: abp.auth.hasPermission('Pages.Editions.MoveTenantsToAnotherEdition')
        };

        var _createModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/Editions/CreateModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/Editions/_CreateModal.js',
            modalClass: 'CreateOrEditEditionModal'
        });

        var _editModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/Editions/EditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/Editions/_EditModal.js',
            modalClass: 'CreateOrEditEditionModal'
        });

        var _moveTenantsToAnotherEditionModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/Editions/MoveTenantsToAnotherEdition',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/Editions/_MoveTenantsToAnotherEditionModal.js',
            modalClass: 'MoveTenantsToAnotherEditionModal'
        });

        function deleteEdition(edition) {
            abp.message.confirm(
                app.localize('EditionDeleteWarningMessage', edition.displayName),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _editionService.deleteEdition({
                            id: edition.id
                        }).done(function () {
                            getEditions();
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#CreateNewEditionButton').click(function () {
            _createModal.open();
        });

        abp.event.on('app.createOrEditEditionModalSaved', function () {
            getEditions();
        });
     
       
        var dataTable = _$editionsTable.DataTable({
            responsive: true,
            searching: true,
            paging: true,
            processing: true,
            listAction: {
                ajaxFunction: _editionService.getEditions
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _editModal.open({ id: data.record.id });
                                }
                            }, {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteEdition(data.record);
                                }
                            }, {
                                text: app.localize('MoveTenantsToAnotherEdition'),
                                visible: function () {
                                    return _permissions.moveTenantsToAnotherEdition;
                                },
                                action: function (data) {
                                    _moveTenantsToAnotherEditionModal.open({ id: data.record.id });
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 2,
                    data: "displayName"
                },
                {
                    targets: 3,
                    data: "price",
                    render: function (displayName, type, row, meta) {
                        if (row.monthlyPrice || row.annualPrice) {
                            return '$ ' + row.monthlyPrice + ' ' + app.localize('Monthly') + ' / $ ' + row.annualPrice + ' ' + app.localize('Annual');
                        }

                        return app.localize('Free');
                    }
                },
                {
                    targets: 4,
                    data: "trialDayCount",
                    render: function (trialDayCount) {


                        if (trialDayCount > 0) {
                            return app.localize('Yes') + ', ' + trialDayCount + ' ' + app.localize('Days');
                        }

                        return app.localize('No');
                    }
                },
               
            ]
        });


     



        function getEditions() {
            dataTable.ajax.reload();
        }
    });
})();