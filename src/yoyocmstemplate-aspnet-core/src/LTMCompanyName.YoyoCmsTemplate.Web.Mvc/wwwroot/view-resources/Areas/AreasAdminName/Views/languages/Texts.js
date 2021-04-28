﻿(function () {
    $(function () {

        var _$textsTable = $('#TextsTable');
        var _languageService = abp.services.app.language;

        var _editTextModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/Languages/EditTextModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/Languages/_EditTextModal.js',
            modalClass: 'EditLanguageTextModal'
        });

        var getFilter = function () {
            return {
                targetLanguageName: $('#TextTargetLanguageSelectionCombobox').val(),
                sourceName: $('#TextSourceSelectionCombobox').val(),
                baseLanguageName: $('#TextBaseLanguageSelectionCombobox').val(),
                targetValueFilter: $('#TargetValueFilterSelectionCombobox').val(),
                filterText: $('#TextFilter').val()
            };
        };
 
        var dataTable = _$textsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            searching:false,
            listAction: {
                ajaxFunction: _languageService.getLanguageTexts,
                inputFilter: getFilter
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
                    data: "key",
                    render: function (key) {
                        return '<span title="' + key + '">' + app.utils.string.truncate(key, 32) + '</span>';
                    }
                },
                {
                    targets: 2,
                    data: "baseValue",
                    render: function (baseValue) {
                        return $("<span/>").attr("title", (baseValue || '')).html((app.utils.string.truncate(baseValue, 32) || ''))[0].outerHTML;
                    }
                },
                {
                    targets: 3,
                    data: "targetValue",
                    render: function (targetValue) {
                        return $("<span/>").attr("title", (targetValue || '')).html((app.utils.string.truncate(targetValue, 32) || ''))[0].outerHTML;
                    }
                },
                {
                    targets: 4,
                    data: null,
                    orderable: false,
                    defaultContent: '',
                    rowAction: {
                        element: $('<button class="btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill" title="' + app.localize('Edit') + '"><i class="la la-edit"></i></button>')
                            .click(function () {
                                var data = $(this).data();
                                _editTextModal.open({
                                    sourceName: $('#TextSourceSelectionCombobox').val(),
                                    baseLanguageName: $('#TextBaseLanguageSelectionCombobox').val(),
                                    languageName: $('#TextTargetLanguageSelectionCombobox').val(),
                                    key: data.key
                                });
                            })
                    }
                }
            ]
        });

        $('#TextBaseLanguageSelectionCombobox,#TextTargetLanguageSelectionCombobox')
            .select2({
                theme: 'bootstrap4',
               
            });

        $('#TextSourceSelectionCombobox,#TargetValueFilterSelectionCombobox')
            .select2({
                theme: 'bootstrap4',
              
            });

        $('#RefreshTextsButton').click(function (e) {
            e.preventDefault();
            loadTable();
        });

        $('#TextsFilterForm select').change(function () {
            loadTable();
        });

        $('#TextFilter').focus();

        var loadTable = function () {
            dataTable.ajax.reload();
        };

        abp.event.on('app.editLanguageTextModalSaved', function () {
            dataTable.ajax.reloadPage();
        });

    });
})();