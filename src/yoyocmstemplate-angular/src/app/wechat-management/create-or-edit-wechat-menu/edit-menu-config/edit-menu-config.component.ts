import {
  Component,
  OnInit,
  ViewEncapsulation,
  Injector,
  Input,
  Output,
  EventEmitter
} from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { IWechatMenuInfo } from '../../components/wechat-menu-button/interface';
import {
  MenuFull_RootButton,
  KeyValuePairOfStringString
} from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-edit-menu-config',
  templateUrl: './edit-menu-config.component.html',
  styleUrls: ['./edit-menu-config.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class EditMenuConfigComponent extends AppComponentBase
  implements OnInit {
  entity: MenuFull_RootButton;

  private _menuItem: IWechatMenuInfo;

  @Input()
  set menuItem(value: IWechatMenuInfo) {
    this._menuItem = value;
    if (this._menuItem) {
      this.entity = value.origin;
    }
  }
  get menuItem(): IWechatMenuInfo {
    return this._menuItem;
  }

  @Input()
  menuTypeList: KeyValuePairOfStringString[];

  constructor(_injector: Injector) {
    super(_injector);
  }

  ngOnInit() {}

  /**
   * 展示菜单类型选择器
   */
  get showMenuType(): boolean {
    return (
      this.entity &&
      this.menuItem &&
      (!this.menuItem.subMenu || this.menuItem.subMenu.length === 0)
    );
  }

  /**
   * 展示key输入框
   */
  get showKeyInput(): boolean {
    return (
      this.showMenuType &&
      (this.entity.type === 'click' ||
        this.entity.type === 'location_select' ||
        this.entity.type === 'pic_photo_or_album' ||
        this.entity.type === 'pic_sysphoto' ||
        this.entity.type === 'pic_weixin' ||
        this.entity.type === 'scancode_push' ||
        this.entity.type === 'scancode_waitmsg')
    );
  }

  /**
   * 展示菜单Url输入框
   */
  get showMenuUrl(): boolean {
    return (
      this.showMenuType &&
      (this.entity.type === 'view' || this.entity.type === 'miniprogram')
    );
  }

  /**
   * 展示AppId和AppPagePath输入框
   */
  get showAppIdAndAppPagePath(): boolean {
    return this.showMenuType && this.entity.type === 'miniprogram';
  }

  /**
   * 展示MediaId输入框
   */
  get showMediaId(): boolean {
    return (
      this.showMenuType &&
      (this.entity.type === 'media_id' || this.entity.type === 'view_limited')
    );
  }
}
