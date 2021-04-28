import {
  Component,
  ChangeDetectionStrategy,
  Input,
  HostBinding,
  Output,
  EventEmitter,
  ViewChild,
  ChangeDetectorRef,
  OnInit,
  OnDestroy,
  Injector,
  ElementRef,
  ViewContainerRef,
} from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { InputNumber, InputBoolean } from '@delon/util';
import { AbpSignalrService } from '@shared/auth/abp-signalr.service';
import { ScrollbarDirective } from '../scrollbar/scrollbar.directive';
import { AppComponentBase } from '@shared/component-base';
import { ChatFriendDto } from './interfaces';
import {
  ChatServiceProxy,
  ChatMessageReadState,
  MarkAllUnreadMessagesOfUserAsReadInput,
} from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';

@Component({
  selector: 'app-quick-chat',
  templateUrl: './quick-chat.component.html',
  host: {
    '[class.quick-chat]': 'true',
    '[class.quick-chat__collapsed]': 'collapsed',
    '[class.d-none]': '!showDialog',
  },
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class QuickChatComponent extends AppComponentBase implements OnInit, OnDestroy {
  /**
   * 发送消息
   */
  chatMessage = '';

  dot = false;

  /**
   * 消息正在发送
   */
  sendingMessage = false;

  /**
   * 当前好友信息
   */
  selectedUser: ChatFriendDto;
  friends: ChatFriendDto[];
  userVisible = false;
  /**
   * 输入框
   */
  @ViewChild('chatMessageInput', { static: true }) chatMessageInput: ElementRef;
  /**
   * 聊天记录滚动条
   */
  @ViewChild('messageScrollbar', { static: true }) messageScrollbar?: ScrollbarDirective;

  @Input() @InputNumber() @HostBinding('style.width.px') width = 420;
  @Input() @InputBoolean() collapsed = true;

  @Output() readonly closed = new EventEmitter<boolean>();

  constructor(injector: Injector, private cdr: ChangeDetectorRef, private abpSignalr: AbpSignalrService) {
    super(injector);
    this.abpSignalr.selectUserSubject.subscribe(user => {
      this.selectedUser = user;
      setTimeout(() => {
        this.scrollToBottom();
      }, 500);
    });
    this.abpSignalr.friendsSubject.subscribe(friend => {
      this.friends = friend;
      this.dot = false;
      this.friends.forEach(element => {
        if (element.unreadMessageCount > 0) {
          this.dot = true;
        }
      });

      setTimeout(() => {
        this.scrollToBottom();
      }, 500);
    });
  }

  get showDialog() {
    return true;
  }

  /**
   * 滚动条重新计算
   */
  private scrollToBottom() {
    this.cdr.detectChanges();
    setTimeout(() => {
      this.messageScrollbar.scrollToBottom();
    }, 200);
  }

  ngOnInit(): void {
    this.scrollToBottom();
  }
  /**
   * 销毁
   */
  ngOnDestroy(): void { }

  /**
   * 回车发送聊天
   * @param e
   */
  enterSend(e: KeyboardEvent) {
    // tslint:disable-next-line: deprecation
    if (e.keyCode !== 13) { return; }
    if (e) {
      e.preventDefault();
      e.stopPropagation();
    }
    this.sendMessage();
  }
  /**
   * 发送消息
   * @param event
   */
  sendMessage(event?: any): void {
    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }

    if (!this.selectedUser) {
      return;
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
        this.cdr.detectChanges();
      },
    );
  }

  friendsOpen() {
    this.userVisible = true;
  }

  firendsClose() {
    this.userVisible = false;
  }

  choUser(i) {
    this.abpSignalr.choUser(i);
  }

  /**
   * 打开关闭聊天窗口
   */
  toggleCollapsed() {
    this.collapsed = !this.collapsed;
  }

  /**
   * 关闭窗口
   */
  close() {
    this.closed.emit(true);
  }
}
