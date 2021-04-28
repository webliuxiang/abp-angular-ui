(function () {
    app.modals.AddRoleModal = function () {

        var _modalManager;

        var _options = {
            serviceMethod: null, //Required
            title: app.localize('SelectAnItem'),
            loadOnStartup: true,
            showFilter: true,
            filterText: '',
            pageSize: app.consts.grid.defaultPageSize
        };

        var _$table;
        var _$filterInput;
        var dataTable;

        function refreshTable() {
            dataTable.ajax.reload();
        }

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _options = $.extend(_options, _modalManager.getOptions().addRoleOptions);

            _$table = _modalManager.getModal().find('#addRoleModalTable');

            _$filterInput = _modalManager.getModal().find('.add-role-filter-text');
            _$filterInput.val(_options.filterText);

            dataTable = _$table.DataTable({
                paging: true,
                serverSide: true,
                processing: true,
                searching: false,
                
                listAction: {
                    ajaxFunction: _options.serviceMethod,
                    inputFilter: function () {
                        return {
                            filter: _$filterInput.val(),
                            organizationUnitId: _modalManager.getArgs().organizationUnitId
                        };
                    }
                },
                columnDefs: [
                    {
                        targets: 0,
                        data: null,
                        defaultContent: '',
                        render: function (data) {
                            return '<label for="checkbox_' + data.value + '" class="m-checkbox">' +
                                        '<input type="checkbox" id="checkbox_' + data.value + '" />' +
                                        '<span></span>' +
                                    '</label>';
                        }
                    },
                    {
                        targets: 1,
                        data: "name"
                    },
                    {
                        targets: 2,
                        visible: false,
                        data: "value"
                    }
                ],
                select: {
                    style: 'multi',
                    info: false,
                    selector: 'td:first-child label.m-checkbox span'
                }
            });

            _modalManager.getModal()
                .find('.add-role-filter-button')
                .click(function (e) {
                    e.preventDefault();
                    refreshTable();
                });

            _modalManager.getModal()
                .find('.modal-body')
                .keydown(function (e) {
                    if (e.which === 13) {
                        e.preventDefault();
                        refreshTable();
                    }
                });

            if (_options.loadOnStartup) {
                refreshTable();
            }

            _modalManager.getModal().find('#btnAddRolesToOrganization').click(function () {
                _modalManager.setResult(dataTable.rows({ selected: true }).data().toArray());
                _modalManager.close();
            });
        };
    };
})();