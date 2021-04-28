import { _HttpClient } from '@delon/theme';
import {
  Component,
  ChangeDetectionStrategy,
  OnInit,
  OnDestroy,
  ChangeDetectorRef,
  ViewChild,
  Injector,
  Input,
  NgZone,
  ElementRef,
} from '@angular/core';
import { timer, Subject } from 'rxjs';
import { filter, concatMap, takeUntil } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { ScrollbarDirective } from '@shared/components/scrollbar/scrollbar.directive';
import {
  CreateFriendshipRequestByUserNameInput,
  FriendshipServiceProxy,
  CommonLookupServiceProxy,
  CommonLookupFindUsersInput,
  CreateFriendshipRequestInput,
  ChatServiceProxy,
  FriendDto,
  UnblockUserInput,
  BlockUserInput,
  FriendshipState,
  ChatMessageReadState,
  MarkAllUnreadMessagesOfUserAsReadInput,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/component-base';
import { LookupComponent } from '@shared/components/lookup/lookup.component';
import { ChatFriendDto } from '@shared/components/quick-chat/interfaces';
import * as _ from 'lodash';
import { AbpSignalrService } from '@shared/auth/abp-signalr.service';
import { SFSchema, SFArrayWidgetSchema } from '@delon/form';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatComponent extends AppComponentBase implements OnInit, OnDestroy {
  private unsubscribe$ = new Subject<void>();

  /**
   * 正在加载聊天记录
   */
  loadingPreviousUserMessages = false;

  _selectedUser: ChatFriendDto = new ChatFriendDto();
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
  }
  get selectedUser(): ChatFriendDto {
    return this._selectedUser;
  }

  /**
   * 定义好友状态
   */
  friendDtoState: typeof FriendshipState = FriendshipState;
  sendingMessage = false;

  /**
   * 好友搜索
   */
  userNameFilter = '';
  /**
   * 发送消息
   */
  chatMessage = '';
  /**
   * 滚动条指令
   */
  @ViewChild('messageScrollbar')
  messageScrollbar?: ScrollbarDirective;
  /**
   * 输入框
   */
  @ViewChild('chatMessageInput', { static: true }) chatMessageInput: ElementRef;

  /**
   * 好友列表
   */
  friends: ChatFriendDto[];

  constructor(
    injector: Injector,
    private _friendshipService: FriendshipServiceProxy,
    private _commonLookupService: CommonLookupServiceProxy,
    private modalService: NzModalService,
    private _chatService: ChatServiceProxy,
    // public brand: BrandService,
    private http: _HttpClient,
    public msg: NzMessageService,
    private cd: ChangeDetectorRef,
    private abpSignalr: AbpSignalrService,
    public _zone: NgZone,
  ) {
    super(injector);

    this.abpSignalr.selectUserSubject.subscribe(user => {
      this.selectedUser = user;
      setTimeout(() => {
        this.scrollToBottom();
      }, 500);
    });
    this.abpSignalr.friendsSubject.subscribe(friend => {
      this.friends = friend;
      setTimeout(() => {
        this.scrollToBottom();
      }, 500);
    });
  }

  private scrollToBottom() {
    if (!this.unsubscribe$.closed) { this.cd.detectChanges(); }
    setTimeout(() => this.messageScrollbar.scrollToBottom());
  }

  ngOnInit() {
    this.abpSignalr.getFriendsAndSettings();
    this.registerEvents();
  }

  /**
   * 从列表点击了一个用户
   * @param friend 用户信息
   */
  choUser(friend: ChatFriendDto) {
    this.abpSignalr.choUser(friend);
  }

  findUser() { }

  enterSend(e: KeyboardEvent) {
    // tslint:disable-next-line: deprecation
    if (e.keyCode !== 13) { return; }
    this.sendMessage(e);
  }

  sendMessage(event?: any): void {
    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }

    if (!this.chatMessage) {
      return;
    }

    this.sendingMessage = true;
    const tenancyName = this.appSession.tenant ? this.appSession.tenant.tenancyName : null;
    this.abpSignalr.sendMessage(
      {
        tenantId: this.selectedUser.friendTenantId,
        userId: this.selectedUser.friendUserId,
        message: this.chatMessage,
        tenancyName: tenancyName,
        userName: this.appSession.user.userName,
        // profilePictureId: this.appSession.user.profilePictureId,
      },
      () => {
        this.chatMessage = '';
        this.sendingMessage = false;
        this.cd.detectChanges();
        this.chatMessage = '';
        this.sendingMessage = false;
        this.cd.detectChanges();
      },
    );
  }

  ngOnDestroy() {
    const { unsubscribe$ } = this;
    unsubscribe$.next();
    unsubscribe$.complete();
  }

  /**
   * 查询用户
   */
  search() {
    const input = new CreateFriendshipRequestByUserNameInput();
    input.userName = this.userNameFilter;
    const tenantId = this.appSession.tenant ? this.appSession.tenant.id : null;
    // 后续尽量支持跨租户 现在是同租户内部
    this.openSearchModal(input.userName, tenantId);
  }

  block(user: FriendDto): void {
    const blockUserInput = new BlockUserInput();
    blockUserInput.tenantId = user.friendTenantId;
    blockUserInput.userId = user.friendUserId;

    this._friendshipService.blockUser(blockUserInput).subscribe(() => {
      this.notify.info(this.l('UserBlocked'));
    });
  }

  unblock(user: FriendDto): void {
    const unblockUserInput = new UnblockUserInput();
    unblockUserInput.tenantId = user.friendTenantId;
    unblockUserInput.userId = user.friendUserId;

    this._friendshipService.unblockUser(unblockUserInput).subscribe(() => {
      this.notify.info(this.l('UserUnblocked'));
    });
  }

  registerEvents(): void {
    const self = this;

    abp.event.on('app.chat.messageReceived', message => {
      self._zone.run(() => {
        // self.onMessageReceived(message);
      });
    });

    abp.event.on('app.chat.allUnreadMessagesOfUserRead', data => {
      self._zone.run(() => {
        // self.onAllUnreadMessagesOfUserRead(data);
      });
    });

    abp.event.on('app.chat.userStateChanged', (friend, state: FriendshipState) => {
      self._zone.run(() => {
        // self.onUserStateChanged(friend, state);
      });
    });
  }

  /**
   * 添加用户信息
   * @param userName
   * @param tenantId
   */
  openSearchModal(userName: string, tenantId?: number): void {
    const modal = this.modalService.create({
      nzTitle: '查询用户',
      nzContent: LookupComponent,
      nzComponentParams: {
        filterText: this.userNameFilter,
        tenantId: this.appSession.tenantId,
        dataSource: (skipCount: number, maxResultCount: number, filter: string, tenantId?: number) => {
          const input = new CommonLookupFindUsersInput();
          input.filterText = filter;
          input.maxResultCount = maxResultCount;
          input.skipCount = skipCount;
          input.tenantId = tenantId;
          return this._commonLookupService.findUsers(input);
        },
      },
      nzFooter: null,
    });
    // Return a result when closed
    modal.afterClose.subscribe(result => {
      if (result === undefined) {
        return;
      }
      const userId = result.value;
      const input = new CreateFriendshipRequestInput();
      input.userId = parseInt(userId);
      input.tenantId = this.appSession.tenant ? this.appSession.tenant.id : null;

      this._friendshipService.createFriendshipRequest(input).subscribe(() => {
        this.userNameFilter = '';
        this.abpSignalr.getFriendsAndSettings();
        this.cd.detectChanges();
      });
    });
  }
}
