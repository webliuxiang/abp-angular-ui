<template>
    <div>
        <div class="modal-header">
            <div class="modal-title">
                <i class="anticon anticon-share-alt mr-sm"></i>
                <span>{{l("NotificationSettings")}}</span>
            </div>
        </div>

        <div v-if="settings">
            <h4>{{l("ReceiveNotifications")}}</h4>
            <a-checkbox name="receiveNotifications" v-model="settings.receiveNotifications"></a-checkbox>
            <span class="help-block">{{l("ReceiveNotifications_Definition")}}</span>

            <h4 v-if="settings.notifications&&settings.notifications.length">{{l("NotificationTypes")}}</h4>
            <p class="text-danger"
               v-if="settings.notifications&&settings.notifications.length && !settings.receiveNotifications">
                <small>{{l("ReceiveNotifications_DisableInfo")}}</small>
            </p>

            <a-checkbox :disabled="!settings.receiveNotifications"
                        :key="index"
                        v-for="(item,key,index) in settings.notifications"
                        v-model="item.isSubscribed">
                {{item.displayName}}
            </a-checkbox>
        </div>


        <div class="modal-footer">
            <a-button @click="close()">
                <i class="anticon anticon-close-circle-o"></i>
                {{l("Cancel")}}
            </a-button>
            <a-button :loading="saving" @click="save()" type="primary">
                <i class="anticon anticon-save" v-if="!saving"></i>
                {{l("Save")}}
            </a-button>
        </div>

    </div>
</template>

<script>
    import {ModalComponentBase} from '@/shared/component-base';
    import {NotificationServiceProxy, UpdateNotificationSettingsInput} from "@/shared/service-proxies";
    import * as _ from 'lodash';

    export default {
        name: "notification-settings",
        mixins: [ModalComponentBase],
        props: ['data'],
        components: {},
        data() {
            return {
                notificationService: null,
                settingsVal: null
            }
        },
        created() {
            this.notificationService = new NotificationServiceProxy(this.$apiUrl, this.$api);

            this.loading = true;
            this.notificationService.getNotificationSettings()
                .finally(() => {
                    this.loading = false;
                }).then((res) => {
                    this.settings = res;
                });
        },
        computed: {
            settings: {
                get() {
                    return this.settingsVal;
                },
                set(val) {
                    this.settingsVal = val;
                }
            }
        },
        watch: {},
        methods: {
            save() {
                this.saving = true;

                let input = new UpdateNotificationSettingsInput();
                input.receiveNotifications = this.settings.receiveNotifications;
                input.notifications = _.map(this.settings.notifications,
                    (n) => {
                        var subscription = new NotificationSubscriptionDto();
                        subscription.name = n.name;
                        subscription.isSubscribed = n.isSubscribed;
                        return subscription;
                    });


                this.notificationService.updateNotificationSettings(input)
                    .finally(() => {
                        this.saving = false;
                    })
                    .then(() => {
                        // this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                    });
            }
        },
    }
</script>

<style scoped>

</style>
