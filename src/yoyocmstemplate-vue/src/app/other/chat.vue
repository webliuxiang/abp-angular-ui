<template>
    <a-spin :spinning="spinning">
        <a-card :bordered="false">
            <a-row :gutter="16">
                <a-col class="gutter-row-left" :span="4">
                    <a-input-search :placeholder="l('AddFriend')" v-model="userNameFilter" enter-button @search="findUser" />
                    <ul class="friend-list">
                        <li v-for="(item, index) in friendlist" :key="index" @click="chatfriends(item)" :class="{'firend-active':item.checked}">
                            <a-avatar :src="item.userimg" />
                            <p>{{ item.friendUserName }}</p>
                            <p class="line-status">
                                <a-badge :status="item.isOnline ? 'success' : 'default'" />
                                <span v-if="item.isOnline " class="online-status">
                                    Online
                                </span>
                                <span v-if="!item.isOnline " class="offline-status">
                                    offline
                                </span>
                            </p>
                        </li>
                    </ul>
                </a-col>
                <a-col class="gutter-row" :span="20">
                    <section class="chat-container">
                        <!-- 用户信息 -->
                        <div class="user-box">
                            <div class="user-content">
                                <div class="badge-container">
                                    <a-badge :status="chatHistory.usermessage.isOnline ? 'success' : 'default'" />
                                    <a-avatar :src="chatHistory.usermessage.userimg" />
                                </div>
                                <strong>{{ chatHistory.usermessage.friendUserName }}</strong>
                                <em>Typing...</em>
                                <a-dropdown>
                                    <a-menu slot="overlay">
                                        <a-menu-item key="1" v-if="chatHistory.usermessage.state !== blocked" @click="blockUser()">
                                            Block
                                        </a-menu-item>
                                        <a-menu-item key="2" v-if="chatHistory.usermessage.state === blocked" @click="UnblockUser()">
                                            Unblock
                                        </a-menu-item>
                                    </a-menu>
                                    <a-button class="lock-btn" shape="circle" icon="small-dash" />
                                </a-dropdown>
                            </div>
                        </div>
                        <!-- 聊天信息 -->
                        <div class="chat-message-container">
                            <ul>
                                <li v-for="item in chatHistory.chatlist" :key="item.id" :class="{'right':item.side === 1, 'left':item.side !== 1}">
                                    <div class="user-message">
                                        <a-avatar :src="item.userimg" />
                                        <div class="chat__message-time">{{ item.shortTime }}</div>
                                    </div>
                                    <div class="chat__message-msg">
                                        <strong class="chat__message-msg--name">{{ item.side === 1 ?  'You' : chatHistory.usermessage.friendUserName  }}</strong>
                                        <div class="chat__message-msg--text">{{ item.message }}</div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <!-- 输入框 -->
                        <a-input :placeholder="l('TypeAMessageHere')" v-model="chatMessage" size="large">
                            <a-button class="reply-btn" slot="addonAfter" type="primary" @click="sendMessage()">
                                {{ l('Reply') }}
                            </a-button>
                        </a-input>
                    </section>
                </a-col>
            </a-row>
            <!-- 添加朋友 -->
            <a-modal
                title="查询用户"
                :visible="visible"
                footer=""
                @cancel="visible = false"
                v-loading="confirmLoading">
                <a-input-search :placeholder="l('AddFriend')" v-model="userNameFilter" enter-button @search="findUser" />
                <a-table :columns="columns" :pagination="false" :data-source="tableFrienditems.items">
                    <span slot="action" slot-scope="text, record">
                        <a @click="addfriend(record)">暂无数据</a>
                    </span>
                </a-table>
                <a-pagination class="modal-pagination" :default-current="page" :total="tableFrienditems.totalCount" @change="pagechange" />
            </a-modal>
        </a-card>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import {
    ChatServiceProxy,
    CommonLookupServiceProxy
} from "@/shared/service-proxies";
import AbpSignalrService from "@/shared/auth/abp-signalr-service";
import moment from "moment";
import {
    abpService,
    appSessionService,
    FriendshipServiceProxy
} from "@/shared";
import { FriendshipState } from "@/shared/service-proxies/service-proxies";

export default {
    mixins: [AppComponentBase],
    name: "chat",
    components: {},
    data() {
        return {
            spinning: false,
            _chatServiceProxy: "",
            // 用户名
            userNameFilter: "",
            // 好友列表
            friendlist: [],
            chatHistory: {
                usermessage: {
                    isOnline: false,
                    userimg: require("./img/default-profile-picture.png")
                },
                chatlist: []
            },
            // 朋友查找框
            visible: false,
            page: 1,
            confirmLoading: false,
            columns: [
                {
                    title: "名字",
                    key: "name",
                    dataIndex: "name",
                    scopedSlots: { customRender: "name" }
                },
                {
                    title: "选择",
                    key: "action",
                    scopedSlots: { customRender: "action" }
                }
            ],
            tableFrienditems: {},
            _commonLookupServiceProxy: "",
            _friendshipServiceProxy: "",
            blocked: "",
            chatMessage: "",
            _abpSignalrService: "",
            // 选择的用户item
            userItem: ""
        };
    },
    created() {
        console.log(FriendshipState);
        this.blocked = FriendshipState.Blocked;
        this._chatServiceProxy = new ChatServiceProxy(this.$apiUrl, this.$api);
        // this._abpSignalrService = new AbpSignalrService();
        this._commonLookupServiceProxy = new CommonLookupServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this._friendshipServiceProxy = new FriendshipServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.getUserChatFriendsWithSettings();
    },
    methods: {
        /**
         * 获取好友
         */
        getUserChatFriendsWithSettings() {
            this.spinning = true;
            this._chatServiceProxy
                .getUserChatFriendsWithSettings()
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.friendlist = res.friends;
                    this.friendlist.map(data => {
                        this.$set(data, "checked", false);
                    });
                    this.friendlist.map(item => {
                        item.userimg = item.friendProfilePictureId
                            ? item.friendProfilePictureId
                            : require("./img/default-profile-picture.png");
                    });
                    console.log(this.friendlist);
                });
        },
        /**
         * 获取聊天记录
         */
        getChatList(item) {
            this.spinning = true;
            this._chatServiceProxy
                .getUserChatMessages(item.friendTenantId, item.friendUserId)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.chatHistory.chatlist = res.items;
                    this.chatHistory.chatlist.map(item => {
                        item.userimg = this.chatHistory.usermessage.userimg;
                        item.shortTime = moment(item.creationTime).format(
                            "a h:mm"
                        );
                    });
                    console.log(res.items);
                });
        },
        /**
         * 查找朋友
         */
        findUser() {
            this.visible = true;
            this.confirmLoading = true;
            this._commonLookupServiceProxy
                .findUsers({
                    filterText: this.userNameFilter,
                    maxResultCount: 10,
                    skipCount: this.page - 1,
                    tenantId: appSessionService.tenant.id
                })
                .finally(() => {
                    this.confirmLoading = false;
                })
                .then(res => {
                    this.tableFrienditems = res;
                });
        },
        /**
         * 选择朋友聊天
         */
        chatfriends(item) {
            item.checked = true;
            this.friendlist
                .filter(data => data.friendUserId !== item.friendUserId)
                .map(item => {
                    item.checked = false;
                });
            if (item.checked) {
                this.chatHistory.usermessage = item;
                this.chatHistory.usermessage.userimg = this.chatHistory
                    .usermessage.friendProfilePictureId
                    ? this.chatHistory.usermessage.friendProfilePictureId
                    : require("./img/default-profile-picture.png");
                this.getChatList(item);
                this.userItem = item;
            }
            console.log(this.chatHistory.usermessage);
        },
        /**
         * 分页
         */
        pagechange(val) {
            console.log(val);
            this.page = val;
            this.findUser();
        },
        /**
         * 添加朋友
         */
        addfriend(item) {
            this._friendshipServiceProxy
                .createFriendshipRequest({
                    userId: item.value,
                    tenantId: appSessionService.tenant.id
                })
                .finally(() => {})
                .then(res => {
                    this.visible = false;
                    this.getUserChatFriendsWithSettings();
                });
        },
        /**
         * 锁定用户
         */
        blockUser() {
            console.log(this.chatHistory.usermessage.friendUserId);
            this.spinning = true;
            this._friendshipServiceProxy
                .blockUser({
                    tenantId: appSessionService.tenant.id,
                    userId: this.chatHistory.usermessage.friendUserId
                })
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.getUserChatFriendsWithSettings(true);
                    this.chatHistory = {
                        usermessage: {
                            isOnline: false,
                            userimg: require("./img/default-profile-picture.png")
                        },
                        chatlist: []
                    };
                });
        },
        UnblockUser() {
            console.log(this.chatHistory.usermessage.friendUserId);
            this.spinning = true;
            this._friendshipServiceProxy
                .unblockUser({
                    tenantId: appSessionService.tenant.id,
                    userId: this.chatHistory.usermessage.friendUserId
                })
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.getUserChatFriendsWithSettings(true);
                    this.chatHistory = {
                        usermessage: {
                            isOnline: false,
                            userimg: require("./img/default-profile-picture.png")
                        },
                        chatlist: []
                    };
                });
        },
        /**
         * 发送
         */
        sendMessage() {
            this.sendingMessage = true;
            const tenancyName = appSessionService.tenant
                ? appSessionService.tenant.tenancyName
                : null;
            AbpSignalrService.sendMessage(
                {
                    tenantId: appSessionService.tenant.id,
                    userId: this.chatHistory.usermessage.friendUserId,
                    message: this.chatMessage,
                    tenancyName: tenancyName,
                    userName: appSessionService.user.userName
                },
                () => {
                    this.chatMessage = "";
                    this.getChatList(this.userItem);
                }
            );
        }
    }
};
</script>

<style scoped lang="less">
@import "./chat.less";
</style>
