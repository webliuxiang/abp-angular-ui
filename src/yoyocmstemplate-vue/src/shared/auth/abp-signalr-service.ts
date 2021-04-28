import {AppConsts} from '@/abpPro/AppConsts';
import {ChatFriendDto} from '@/shared/components/quick-chat/interfaces';
import {HubConnection} from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import * as _ from 'lodash';
import {Subject} from 'rxjs';
import {ChatMessageReadState, ChatServiceProxy, MarkAllUnreadMessagesOfUserAsReadInput} from '../service-proxies';
import {apiHttpClient} from '../utils';

class AbpSignalrService {
    public reconnectTime = 5000;
    public tries = 1;
    public maxTries = 8;


    private _chatService: ChatServiceProxy;

    get chatService(): ChatServiceProxy {
        if (!this._chatService) {
            this._chatService = new ChatServiceProxy(AppConsts.remoteServiceBaseUrl, apiHttpClient);
        }
        return this._chatService;
    }

    public chatHub: HubConnection;

    /**
     * 是否成功链接
     */
    public isChatConnected = false;

    /**
     * 好友列表
     */
    public friends: ChatFriendDto[];
    public friendsSubject = new Subject<ChatFriendDto[]>();

    /**
     * 当前选择的好友
     */
    public _selectedUser: ChatFriendDto = new ChatFriendDto();
    public selectUserSubject = new Subject<ChatFriendDto>();

    // #endregion
    set selectedUser(newValue: ChatFriendDto) {
        if (newValue === this._selectedUser) {
            return;
        }
        // 如果这个用户第一次被点击 就清空 聊天记录并且 设置等待加载记录
        this._selectedUser = newValue;
        if (newValue.messages) {
            newValue.messages = [];
            newValue.messagesLoaded = false;
        }
        this.selectUserSubject.next(this.selectedUser);
    }

    get selectedUser(): ChatFriendDto {
        return this._selectedUser;
    }

    /**
     * 链接初始化
     */
    public init(): void {
        let url = abp.appPath + 'signalr-chat';

        if (abp.signalr.remoteServiceBaseUrl) {
            url = abp.signalr.remoteServiceBaseUrl + url;
        }

        // Add query string: https://github.com/aspnet/SignalR/issues/680
        if (abp.signalr.qs) {
            url += (url.indexOf('?') === -1 ? '?' : '&') + abp.signalr.qs;
        }

        this.startConnection(url).then(() => {
            abp.event.trigger('app.chat.connected');
            this.isChatConnected = true;
        });
    }

    /**
     * 配置链接检测 断开
     * @param connection 集线器
     */
    public configureConnection(connection): void {
        // 设置全局集线器
        this.chatHub = connection;

        // 如果链接断开就调用重试方法
        connection.onclose((e) => {
            this.isChatConnected = false;

            if (e) {
                abp.log.debug('Chat connection closed with error: ' + e);
            } else {
                abp.log.debug('Chat disconnected');
            }

            this.reconnectHub(connection).then(() => {
                this.isChatConnected = true;
            });
        });

        // 注册消息通知
        this.registerChatEvents(connection);
    }

    /**
     * 注册链接成功后的消息通知
     * @param connection  集线器
     */
    public registerChatEvents(connection): void {
        const self = this;
        self.getFriendsAndSettings();

        connection.on('getChatMessage', (message) => {
            self.onMessageReceived(message);
            abp.event.trigger('app.chat.messageReceived', message);
        });

        connection.on('getAllFriends', (friends) => {
            abp.event.trigger('abp.chat.friendListChanged', friends);
        });

        connection.on('getFriendshipRequest', (friendData, isOwnRequest) => {
            abp.event.trigger('app.chat.friendshipRequestReceived', friendData, isOwnRequest);
        });

        connection.on('getUserConnectNotification', (friend, isConnected) => {
            abp.event.trigger('app.chat.userConnectionStateChanged', {
                friend,
                isConnected,
            });
        });

        connection.on('getUserStateChange', (friend, state) => {
            self.onUserStateChanged(friend, state);
            abp.event.trigger('app.chat.userStateChanged', friend, state);
        });

        connection.on('getallUnreadMessagesOfUserRead', (friend) => {
            self.onAllUnreadMessagesOfUserRead(friend);
            abp.event.trigger('app.chat.allUnreadMessagesOfUserRead', {
                friend,
            });
        });

        connection.on('getReadStateChange', (friend) => {
            abp.event.trigger('app.chat.readStateChange', {
                friend,
            });
        });
    }

    /**
     * 发送消息
     * @param messageData  消息内容配置
     * @param callback 回调方法
     */
    public sendMessage(messageData, callback): void {
        if (!this.isChatConnected) {
            if (callback) {
                callback();
            }

            // abp.notify.warn(this.l('ChatIsNotConnectedWarning'));
            return;
        }

        this.chatHub
            .invoke('sendMessage', messageData)
            .then((result) => {
                if (result) {
                    abp.notify.warn(result);
                }

                if (callback) {
                    callback();
                }
            })
            .catch((error) => {
                abp.log.error(error);

                if (callback) {
                    callback();
                }
            });
    }

    /**
     * 链接集线器
     * @param url 集线器地址
     */
    public startConnection(url): any {
        return new Promise((resolve, rej) => {
            const connection = new signalR.HubConnectionBuilder().withUrl(url, signalR.HttpTransportType.WebSockets).build();
            this.configureConnection(connection);

            connection
                .start()
                .then(function () {
                    return resolve(connection);
                })
                .catch(function (error) {
                    abp.log.debug(
                        'Cannot start the connection using ' +
                        signalR.HttpTransportType[signalR.HttpTransportType.WebSockets] +
                        ' transport. ' +
                        error.message
                    );
                    return rej(error);
                });
        });
    }

    /**
     * 链接重试
     * @param connection 集线器
     */
    public reconnectHub(connection) {
        // 连接重试

        return this.reconnectstart(connection);
    }

    /**
     * 重试调用
     * @param connection 集线器
     * @param reconnectTime 超时时间
     * @param tries 当前次数
     * @param maxTries 链接重试最大次数
     */
    public reconnectstart(connection) {
        const _self = this;

        return new Promise(function (resolve, reject) {
            if (_self.tries > _self.maxTries) {
                reject();
            } else {
                connection
                    .start()
                    .then(resolve)
                    .then(() => {
                        _self.reconnectTime = 5000;
                        _self.tries = 1;
                    })
                    .catch(() => {
                        setTimeout(() => {
                            this.reconnectstart().then(resolve);
                        }, _self.reconnectTime);
                        _self.reconnectTime *= 2;
                        _self.tries += 1;
                    });
            }
        });
    }

    public close() {
    }

    /**
     * 把当前聊天变成已读
     * @param user 用户信息
     */
    public markAllUnreadMessagesOfUserAsRead(user: ChatFriendDto): void {
        if (!user) {
            return;
        }
        // 获取未读的消息列表
        const unreadMessages = _.filter(user.messages, (m) => m.readState === ChatMessageReadState.Unread);
        // 筛选得到消息Id
        const unreadMessageIds = _.map(unreadMessages, 'id');

        if (!unreadMessageIds.length) {
            return;
        }

        const input = new MarkAllUnreadMessagesOfUserAsReadInput();
        input.tenantId = user.friendTenantId;
        input.userId = user.friendUserId;

        this.chatService.markAllUnreadMessagesOfUserAsRead(input)
            .then(() => {
                _.forEach(user.messages, (message) => {
                    // 对比后台成功设置的消息 读取状态
                    if (unreadMessageIds.indexOf(message.id) >= 0) {
                        // 把信息变成已读
                        message.readState = ChatMessageReadState.Read;
                    }

                    this.friendsSubject.next(this.friends);
                    this.selectUserSubject.next(this.selectedUser);
                });
            });
    }

    /**
     * 从列表点击了一个用户
     * @param friend 用户信息
     */
    public choUser(friend: ChatFriendDto) {
        // 设置用户被选中
        this.selectedUser.active = false;
        friend.active = true;
        this.selectedUser = friend;

        const chatUser = this.getFriendOrNull(friend.friendUserId, friend.friendTenantId);
        this.selectedUser = chatUser;
        if (!chatUser) {
            return;
        }

        // 用户信息是否加载过 历史聊天记录
        if (!chatUser.messagesLoaded) {
            // 调用补充历史聊天记录
            this.loadMessages(chatUser, () => {
                chatUser.messagesLoaded = true;
                // this.scrollToBottom();
                // this.chatMessageInput.nativeElement.focus();
            });
        } else {
            // 把发来的消息变成已读
            this.markAllUnreadMessagesOfUserAsRead(this.selectedUser);
            // this.scrollToBottom();
            // this.chatMessageInput.nativeElement.focus();
        }
        this.friendsSubject.next(this.friends);
        this.selectUserSubject.next(this.selectedUser);
    }

    /**
     * 收到消息
     * @param message
     */
    public onMessageReceived(message) {
        const self = this;

        // 判断接收人是否存在
        const user = self.getFriendOrNull(message.targetUserId, message.targetTenantId);
        if (!user) {
            return;
        }

        console.log('message', message);
        user.messages = user.messages || [];
        user.messages.push(message);

        if (message.side === 2) {
            // ChatSide.Receiver
            user.unreadMessageCount += 1;
            message.readState = ChatMessageReadState.Unread;
            self.triggerUnreadMessageCountChangeEvent();

            if (
                self.selectedUser !== null &&
                user.friendTenantId === self.selectedUser.friendTenantId &&
                user.friendUserId === self.selectedUser.friendUserId
            ) {
                self.markAllUnreadMessagesOfUserAsRead(user);
            }
        }

        this.friendsSubject.next(this.friends);
        this.selectUserSubject.next(this.selectedUser);
    }

    /**
     * 获取用户历史聊天记录
     * @param user  用户信息
     * @param callback  回调方法
     */
    public loadMessages(user: ChatFriendDto, callback: any): void {
        // 获取俩天记录ID
        let minMessageId;
        if (user.messages && user.messages.length) {
            minMessageId = _.min(_.map(user.messages, (m) => m.id));
        }

        // 获取历史消息记录
        this.chatService
            .getUserChatMessages(user.friendTenantId ? user.friendTenantId : undefined, user.friendUserId, minMessageId)
            .then((result) => {
                if (!user.messages) {
                    user.messages = [];
                }

                user.messages = result.items.concat(user.messages);

                // 设置消息已读
                this.markAllUnreadMessagesOfUserAsRead(user);

                if (!result.items.length) {
                    user.allPreviousMessagesLoaded = true;
                }

                if (callback) {
                    callback();
                }
                this.friendsSubject.next(this.friends);
                this.selectUserSubject.next(this.selectedUser);
            });
    }

    /**
     * 获取好友列表
     */
    public getFriendsAndSettings(): void {
        this.chatService.getUserChatFriendsWithSettings()
            .then((result) => {
                this.friends = result.friends as ChatFriendDto[];
                this.friendsSubject.next(this.friends);
            });
    }

    public getFriendOrNull(userId: number, tenantId?: number): ChatFriendDto {
        const friends = _.filter(
            this.friends,
            (friend) => friend.friendUserId === userId && friend.friendTenantId === tenantId
        );
        if (friends.length) {
            return friends[0];
        }

        return null;
    }

    public onAllUnreadMessagesOfUserRead(data) {
        const self = this;

        const user = self.getFriendOrNull(data.userId, data.tenantId);
        if (!user) {
            return;
        }

        user.unreadMessageCount = 0;
        self.triggerUnreadMessageCountChangeEvent();
        this.selectUserSubject.next(this.selectedUser);
    }

    public onUserStateChanged(friend, state) {
        const self = this;
        const user = self.getFriendOrNull(friend.userId, friend.tenantId);
        if (!user) {
            return;
        }

        user.state = state;
        this.selectUserSubject.next(this.selectedUser);
    }

    public triggerUnreadMessageCountChangeEvent(): void {
        let totalUnreadMessageCount = 0;

        if (this.friends) {
            totalUnreadMessageCount = _.reduce(this.friends, (memo, friend) => memo + friend.unreadMessageCount, 0);
        }
    }

}

const abpSignalrService = new AbpSignalrService();
export default abpSignalrService;
