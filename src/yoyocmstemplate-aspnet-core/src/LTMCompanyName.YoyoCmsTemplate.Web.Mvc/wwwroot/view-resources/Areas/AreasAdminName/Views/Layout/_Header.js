(function ($) {
    $(function () {

        //Back to my account

        $('#UserProfileBackToMyAccountButton').click(function (e) {
            e.preventDefault();
            abp.ajax({
                url: abp.appPath + 'Account/BackToImpersonator',
                success: function () {
                    if (!app.supportsTenancyNameInUrl) {
                        abp.multiTenancy.setTenantIdCookie(abp.session.impersonatorTenantId);
                    }
                }
            });
        });

        //My settings

        var mySettingsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/TopBarProfile/MySettingsModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/TopBarProfile/_MySettingsModal.js',
            modalClass: 'MySettingsModal'
        });

        $('#TopBarProfileMySettingsLink').click(function (e) {
            e.preventDefault();
            mySettingsModal.open("","","md");
        });

        $('#UserDownloadCollectedDataLink').click(function (e) {
            e.preventDefault();
            abp.services.app.profile.prepareCollectedData().done(function () {
                abp.message.success(app.localize("GdprDataPrepareStartedNotification"));
            });
        });

        //修改密码

        var changePasswordModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/TopBarProfile/ChangePasswordModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/TopBarProfile/_ChangePasswordModal.js',
            modalClass: 'ChangePasswordModal'
        });

        $('#TopBarProfileChangePasswordLink').click(function (e) {
            e.preventDefault();
            changePasswordModal.open("", "", "md");
        });

        //修改头像

        var changeProfilePictureModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/TopBarProfile/ChangePictureModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/TopBarProfile/_ChangePictureModal.js',
            modalClass: 'ChangeProfilePictureModal'
        });

        $('#TopBarProfileChangeProfilePictureLink').click(function (e) {
            e.preventDefault();
            changeProfilePictureModal.open("", "", "md");
        });

        //登录记录
        var userLoginAttemptsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/TopBarProfile/LoginAttemptsModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/TopBarProfile/_LoginAttemptsModal.js',
            modalClass: 'LoginAttemptsModal'
        });

        $('#TopBarProfileLoginAttemptsLink').click(function (e) {
            e.preventDefault();
            userLoginAttemptsModal.open();
        });

        //管理账户关联
        var _userLinkService = abp.services.app.userLink;

        var manageLinkedAccountsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AreasAdminName/TopBarProfile/LinkedAccountsModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AreasAdminName/Views/TopBarProfile/_LinkedAccountsModal.js',
            modalClass: 'LinkedAccountsModal'
        });

        var getRecentlyLinkedUsers = function () {
            _userLinkService.getRecentlyUsedLinkedUsers()
                .done(function (result) {

                    loadRecentlyUsedLinkedUsers(result);

                    $("#ManageLinkedAccountsLink").click(function (e) {
                        e.preventDefault();
                        manageLinkedAccountsModal.open();
                    });

                    $(".recently-linked-user").click(function (e) {
                        e.preventDefault();
                        var userId = $(this).attr("data-user-id");
                        var tenantId = $(this).attr("data-tenant-id");
                        if (userId) {
                            switchToUser(userId, tenantId);
                        }
                    });
                });
        };

        function loadRecentlyUsedLinkedUsers(result) {
            var $ul = $("ul#RecentlyUsedLinkedUsers");

            $.each(result.items, function (index, linkedUser) {
                linkedUser.shownUserName = app.getShownLinkedUserName(linkedUser);
            });

            result.hasLinkedUsers = function () {
                return this.items.length > 0;
            };

            var template = $('#linkedAccountsSubMenuTemplate').html();
            Mustache.parse(template);
            var rendered = Mustache.render(template, result);
            $ul.html(rendered);
        }

        function switchToUser(linkedUserId, linkedTenantId) {
            abp.ajax({
                url: abp.appPath + 'Account/SwitchToLinkedAccount',
                data: JSON.stringify({
                    targetUserId: linkedUserId,
                    targetTenantId: linkedTenantId
                }),
                success: function () {
                    if (!app.supportsTenancyNameInUrl) {
                        abp.multiTenancy.setTenantIdCookie(linkedTenantId);
                    }
                }
            });
        }

        manageLinkedAccountsModal.onClose(function () {
            getRecentlyLinkedUsers();
        });

        //Notifications
        var _appUserNotificationHelper = new app.UserNotificationHelper();
        var _notificationService = abp.services.app.notification;

        function bindNotificationEvents() {
            $('#setAllNotificationsAsReadLink').click(function (e) {
                e.preventDefault();
                e.stopPropagation();

                _appUserNotificationHelper.setAllAsRead(function () {
                    loadNotifications();
                });
            });

            $('.set-notification-as-read').click(function (e) {
                e.preventDefault();
                e.stopPropagation();

                var notificationId = $(this).attr("data-notification-id");
                if (notificationId) {
                    _appUserNotificationHelper.setAsRead(notificationId, function () {
                        loadNotifications();
                    });
                }
            });

            $('#openNotificationSettingsModalLink').click(function (e) {
                e.preventDefault();
                e.stopPropagation();

                _appUserNotificationHelper.openSettingsModal();
            });

            $('div.user-notification-item-clickable').click(function () {
                var url = $(this).attr('data-url');
                document.location.href = url;
            });
        }

        function loadNotifications() {
            _notificationService.getPagedUserNotifications({
                maxResultCount: 3
            }).done(function (result) {
                result.notifications = [];
                result.unreadMessageExists = result.unreadCount > 0;
                $.each(result.items, function (index, item) {
                    result.notifications.push(_appUserNotificationHelper.format(item));
                });

                var $li = $('#header_notification_bar');

                var template = $('#headerNotificationBarTemplate').html();
                Mustache.parse(template);
                var rendered = Mustache.render(template, result);

                $li.html(rendered);

                bindNotificationEvents();
            });
        }

        abp.event.on('abp.notifications.received', function (userNotification) {
            _appUserNotificationHelper.show(userNotification);
            loadNotifications();
        });

        abp.event.on('app.notifications.refresh', function () {
            loadNotifications();
        });

        abp.event.on('app.notifications.read', function (userNotificationId) {
            loadNotifications();
        });

        //Chat
        abp.event.on('app.chat.unreadMessageCountChanged', function (messageCount) {
            if (messageCount) {
                $('#UnreadChatMessageCount').removeClass('d-none');
            } else {
                $('#UnreadChatMessageCount').addClass('d-none');
            }

            $('#UnreadChatMessageCount').html(messageCount);
        });

        function init() {
            loadNotifications();
            getRecentlyLinkedUsers();
        }

        init();
    });
})(jQuery);