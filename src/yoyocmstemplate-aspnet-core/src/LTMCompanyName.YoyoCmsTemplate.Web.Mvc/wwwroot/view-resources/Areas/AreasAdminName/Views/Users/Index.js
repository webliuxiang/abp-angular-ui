(function () {
    $(function () {

        var _$usersTable = $('#UsersTable');
        var _userService = abp.services.app.user;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Users.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Users.Edit'),
            changePermissions: abp.auth.hasPermission('Pages.Administration.Users.ChangePermissions'),
            impersonation: abp.auth.hasPermission('Pages.Administration.Users.Impersonation'),
            'delete': abp.auth.hasPermission('Pages.Administration.Users.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/Users/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/Users/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditUserModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/Users/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        

       
      
       

        var input = {};
        
      
        function getInput() {
            var permission = $("#PermissionSelectionCombo").val();
            var role = $("#RoleSelectionCombo").val();
            role = Number(role); //转换为number类型
            var roleList = [];
            var permissionList = [];
            input.filterText = $('#UsersTableFilter').val();
            input.onlyLockedUsers = $("#UsersTable_OnlyLockedUsers").is(':checked');
            if (permission != "") {
                permissionList.push(permission);
                input.permission = permissionList;
              
            }
            if (role != 0) {
                roleList.push(role);
                input.role = roleList;
              
            }
            return input;
        }
        



        var dataTable = _$usersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            searching: false,
            listAction: {
                ajaxFunction: _userService.getPaged,
                inputFilter: function () {
                    return getInput();
                } 
            },
            columnDefs: [{
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
                        items: [{
                            text: app.localize('LoginAsThisUser'),
                            visible: function (data) {
                                return _permissions.impersonation && data.record.id !== abp.session.userId;
                            },
                            action: function (data) {
                                abp.ajax({
                                    url: abp.appPath + 'Account/Impersonate',
                                    data: JSON.stringify({
                                        tenantId: abp.session.tenantId,
                                        userId: data.record.id
                                    })
                                });
                            }
                        }, {
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({
                                    id: data.record.id
                                });
                            }
                        }, {
                            text: app.localize('Permissions'),
                            visible: function () {
                                return _permissions.changePermissions;
                            },
                            action: function (data) {
                                _userPermissionsModal.open({
                                    id: data.record.id
                                });
                            }
                        }, {
                            text: app.localize('Unlock'),
                            visible: function () {
                                return _permissions.changePermissions;
                            },
                            action: function (data) {
                                _userService.unlockUser({
                                    id: data.record.id
                                }).done(function () {
                                    abp.notify.success(app.localize('UnlockedTheUser', data.record.userName));
                                });
                            }
                        }, {
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteUser(data.record);
                            }
                        }]
                    }
                },
                {
                    targets: 2,
                    data: "userName",
                    render: function (userName, type, row, meta) {
                        var $container = $("<span/>");
                        if (row.profilePictureId) {
                            var profilePictureUrl = "/Profile/GetProfilePictureById?id=" + row.profilePictureId;
                            var $link = $("<a/>").attr("href", profilePictureUrl).attr("target", "_blank");
                            var $img = $("<img/>")
                                .addClass("img-circle")
                                .attr("src", profilePictureUrl);

                            $link.append($img);
                            $container.append($link);
                        }

                        $container.append(userName);
                        return $container[0].outerHTML;
                    }
                },

                {
                    targets: 3,
                    data: "roles",
                    orderable: false,
                    render: function (roles) {
                        var roleNames = '';
                        for (var j = 0; j < roles.length; j++) {
                            if (roleNames.length) {
                                roleNames = roleNames + ', ';
                            }

                            roleNames = roleNames + roles[j].roleName;
                        }

                        return roleNames;
                    }
                },
                {
                    targets: 4,
                    data: "emailAddress"
                },
                {
                    targets: 5,
                    data: "isEmailConfirmed",
                    render: function (isEmailConfirmed) {
                        if (isEmailConfirmed) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
                {
                    targets: 6,
                    data: "isActive",
                    render: function (isActive) {
                        if (isActive) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
                {
                    targets: 7,
                    data: "creationTime",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    }
                }
            ]
        });


        function getUsers() {
            input = {};
            dataTable.ajax.reload();
        }

        function deleteUser(user) {
            if (user.userName === app.consts.userManagement.defaultAdminUserName) {
                abp.message.warn(app.localize("{0}UserCannotBeDeleted", app.consts.userManagement.defaultAdminUserName));
                return;
            }

            abp.message.confirm(
                app.localize('UserDeleteWarningMessage', user.userName),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userService.deleteUser({
                            id: user.id
                        }).done(function () {
                            getUsers(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewUserButton').click(function () {
            _createOrEditModal.open();
        });

        var getSortingFromDatatable = function () {
            if (dataTable.ajax.params().order.length > 0) {
                var columnIndex = dataTable.ajax.params().order[0].column;
                var dir = dataTable.ajax.params().order[0].dir;
                var columnName = dataTable.ajax.params().columns[columnIndex].data;

                return columnName + ' ' + dir;
            } else {
                return '';
            }
        };

        $('#ExportUsersToExcelButton').click(function (e) {
            e.preventDefault();
            _userService
                .getUsersToExcel({
                    filter: $('#UsersTableFilter').val(),
                    permission: $("#PermissionSelectionCombo").val(),
                    role: $("#RoleSelectionCombo").val(),
                    onlyLockedUsers: $("#UsersTable_OnlyLockedUsers").is(':checked'),
                    sorting: getSortingFromDatatable()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetUsersButton, #RefreshUserListButton').click(function (e) {
            e.preventDefault();
            getUsers();
        });

        $('#UsersTableFilter').on('keydown', function (e) {
            if (e.keyCode !== 13) {
                return;
            }

            e.preventDefault();
            getUsers();
        });

        abp.event.on('app.createOrEditUserModalSaved', function () {
            getUsers();
        });

        $('#UsersTableFilter').focus();

        $('#ImportUsersFromExcelButton').fileupload({
                url: abp.appPath + 'Users/ImportFromExcel',
                dataType: 'json',
                maxFileSize: 1048576 * 100,
                done: function (e, response) {
                    var jsonResult = response.result;
                    if (jsonResult.success) {
                        abp.notify.info(app.localize('ImportUsersProcessStart'));
                    } else {
                        abp.notify.warn(app.localize('ImportUsersUploadFailed'));
                    }
                }
            }).prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled');
    });
})();