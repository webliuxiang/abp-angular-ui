<template>

    <a-dropdown class="alain-pro__nav-item" placement="bottomRight">
        <template slot="overlay">
            <a-spin :spinning="loading">
                <a-card :title="l('NewNotifications')" :bordered="false"
                        class="ant-card__body-nopadding"
                        style="min-width:450px;">
                    <template slot="extra">
                        <a v-show="notifications&&notifications.length===0" @click="setAllNotificationsAsRead()"
                           style="margin-right: 10px;">
                            <a-icon type="check"></a-icon>
                            {{l("SetAllAsRead")}}
                        </a>
                        <a class="mr-sm" @click="loadNotifications()">
                            <a-icon type="reload"></a-icon>
                            {{l("Refresh")}}
                        </a>
                        <a @click="chantNotificationSettings()">
                            <a-icon type="setting"></a-icon>
                            {{l("Settings")}}
                        </a>
                    </template>

                    <a-row v-for="(item,key,index) in notifications"
                           :key="index"
                           type="flex"
                           justify="center"
                           align="middle"
                           class="py-sm bg-grey-lighter-h point"

                           @click="gotoUrl(item.url)">

                        <a-col :span="20">
                            <a-tooltip>
                                <template slot="title">
                                    {{item.text}}
                                </template>

                                <a-badge v-show="item.icon!=='fatal'" :status="item.icon"></a-badge>
                                <a-badge v-show="item.icon==='fatal'" status="default"
                                         :style="{'background-color':'black'}">
                                </a-badge>
                                {{item.text}}
                            </a-tooltip>

                            <p>
                                {{item.time | momentFromNow}}
                                <a v-show="item.state==='UNREAD'"
                                   @click="setNotificationAsRead(item)">
                                    {{l('SetAsRead')}}
                                </a>
                            </p>

                        </a-col>
                    </a-row>

                    <!-- 没有数据 -->
                    <a-row v-if="!notifications||notifications.length===0">
                        <a-col :span="24" class="pt-md border-top-1 text-center text-grey point">
                            {{l("ThereIsNoNotification")}}
                        </a-col>
                    </a-row>

                    <!-- 查看所有通知 -->
                    <a-row v-if="notifications&&notifications.length!==0">
                        <a-col :span="24" class="pt-md border-top-1 text-center text-grey point">
                            <router-link to="/app/notifications">{{l('SeeAllNotifications')}}</router-link>

                        </a-col>
                    </a-row>

                </a-card>
            </a-spin>
        </template>

        <a-badge :count="unreadNotificationCount">
            <a-icon type="bell" class="alain-pro__nav-item-icon"></a-icon>
        </a-badge>
    </a-dropdown>

    <!--    -->
    <!--    <a-popover trigger="hover" placement="bottomRight">-->
    <!--        <template slot="title">-->

    <!--        </template>-->
    <!--        <template slot="content">-->

    <!--            <a-spin :spinning="loading">-->
    <!--                <a-card :title="l('NewNotifications')" :bordered="false"-->
    <!--                        class="ant-card__body-nopadding"-->
    <!--                        style="min-width:450px;">-->
    <!--                    <template slot="extra">-->
    <!--                        <a v-show="notifications&&notifications.length===0" @click="setAllNotificationsAsRead()"-->
    <!--                           style="margin-right: 10px;">-->
    <!--                            <a-icon type="check"></a-icon>-->
    <!--                            {{l("SetAllAsRead")}}-->
    <!--                        </a>-->
    <!--                        <a class="mr-sm" @click="loadNotifications()">-->
    <!--                            <a-icon type="reload"></a-icon>-->
    <!--                            {{l("Refresh")}}-->
    <!--                        </a>-->
    <!--                        <a @click="chantNotificationSettings()">-->
    <!--                            <a-icon type="setting"></a-icon>-->
    <!--                            {{l("Settings")}}-->
    <!--                        </a>-->
    <!--                    </template>-->

    <!--                    <a-row v-for="(item,key,index) in notifications"-->
    <!--                           :key="index"-->
    <!--                           type="flex"-->
    <!--                           justify="center"-->
    <!--                           align="middle"-->
    <!--                           class="py-sm bg-grey-lighter-h point"-->

    <!--                           @click="gotoUrl(item.url)">-->

    <!--                        <a-col :span="20">-->
    <!--                            <a-tooltip>-->
    <!--                                <template slot="title">-->
    <!--                                    {{item.text}}-->
    <!--                                </template>-->

    <!--                                <a-badge v-show="item.icon!=='fatal'" :status="item.icon"></a-badge>-->
    <!--                                <a-badge v-show="item.icon==='fatal'" status="default"-->
    <!--                                         :style="{'background-color':'black'}">-->
    <!--                                </a-badge>-->
    <!--                                {{item.text}}-->
    <!--                            </a-tooltip>-->

    <!--                            <p>-->
    <!--                                {{item.time | momentFromNow}}-->
    <!--                                <a v-show="item.state==='UNREAD'"-->
    <!--                                   @click="setNotificationAsRead(item)">-->
    <!--                                    {{l('SetAsRead')}}-->
    <!--                                </a>-->
    <!--                            </p>-->

    <!--                        </a-col>-->
    <!--                    </a-row>-->

    <!--                    &lt;!&ndash; 没有数据 &ndash;&gt;-->
    <!--                    <a-row v-if="!notifications||notifications.length===0">-->
    <!--                        <a-col :span="24" class="pt-md border-top-1 text-center text-grey point">-->
    <!--                            {{l("ThereIsNoNotification")}}-->
    <!--                        </a-col>-->
    <!--                    </a-row>-->

    <!--                    &lt;!&ndash; 查看所有通知 &ndash;&gt;-->
    <!--                    <a-row v-if="notifications&&notifications.length!==0">-->
    <!--                        <a-col :span="24" class="pt-md border-top-1 text-center text-grey point">-->
    <!--                            <router-link to="/app/notifications">{{l('SeeAllNotifications')}}</router-link>-->

    <!--                        </a-col>-->
    <!--                    </a-row>-->

    <!--                </a-card>-->
    <!--            </a-spin>-->


    <!--        </template>-->
    <!--        <a-badge :count="unreadNotificationCount">-->
    <!--            <a-icon type="bell" class="alain-pro__nav-item-icon"></a-icon>-->
    <!--        </a-badge>-->
    <!--    </a-popover>-->

</template>

<script>

    import {AppComponentBase} from '@/shared/component-base';
    import {NotificationServiceProxy} from "@/shared/service-proxies";
    import {UserNotificationHelper,ModalHelper} from "@/shared/helpers";
    import {abpService} from "@/shared/abp";
    import {NotificationSettings} from '../notification-settings';
    import {Modal} from 'ant-design-vue';

    export default {
        name: "header-notifications",
        mixins: [AppComponentBase],
        data() {
            return {
                unreadNotificationCountVal: 0,
                notificationService: null,
                notificationsVal: null
            }
        },
        created() {
            this.notificationService = new NotificationServiceProxy(this.$apiUrl, this.$api);
            this.loadNotifications();
            this.registerToEvents();
        },
        computed: {
            unreadNotificationCount: {
                get() {
                    return this.unreadNotificationCountVal;
                },
                set(val) {
                    this.unreadNotificationCountVal = val;
                }
            },
            notifications: {
                get() {
                    return this.notificationsVal;
                },
                set(val) {
                    this.notificationsVal = val;
                }
            },
        },
        methods: {
            loadNotifications() {
                this.loading = true;
                this.notificationService.getPagedUserNotifications(undefined, this.$notificationCount, 0)
                    .finally(() => {
                        this.loading = false;
                    })
                    .then(result => {
                        this.unreadNotificationCount = result.unreadCount;
                        this.notifications = [];
                        result.items.forEach((item) => {
                            this.notifications.push(UserNotificationHelper.format(item)
                            )
                            ;
                        });
                    });
            },
            registerToEvents() {
                abpService.abp.event.on('abp.notifications.received', userNotification => {
                    UserNotificationHelper.show(userNotification);
                    this.loadNotifications();
                });

                abpService.abp.event.on('app.notifications.refresh', () => {
                    this.loadNotifications();
                });


                abpService.abp.event.on('app.notifications.read', userNotificationId => {
                    for (let i = 0; i < this.notifications.length; i++) {
                        if (this.notifications[i].userNotificationId === userNotificationId) {
                            this.notifications[i].state = 'READ';
                        }
                    }
                    this.unreadNotificationCount -= 1;
                });
            },
            setAllNotificationsAsRead() {
                UserNotificationHelper.setAllAsRead();
            },
            chantNotificationSettings() {
                ModalHelper.create(NotificationSettings,null,{width:'900px'})
                    .subscribe((res) => {

                    });
            },
            setNotificationAsRead(userNotification) {
                UserNotificationHelper.setAsRead(userNotification.userNotificationId);
            },
            gotoUrl(url) {
                if (url) {
                    window.location.href = url;
                }
            }
        }
    }
</script>

<style scoped>

</style>
