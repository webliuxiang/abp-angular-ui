import {
  WechatMenuAppSeviceServiceProxy,
  MenuMatchRule,
  GetWechatMenuForEditOutput,
  MenuFull_RootButton,
  CreateOrEditWechatMenuInput
} from '@shared/service-proxies/service-proxies';
import { Component, OnInit, Injector, Input } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { ReuseTabService } from '@delon/abc';
import { ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { IWechatMenuInfo } from '../components/wechat-menu-button/interface';

@Component({
  selector: 'app-create-or-edit-wechat-menu',
  templateUrl: './create-or-edit-wechat-menu.component.html',
  styleUrls: ['./create-or-edit-wechat-menu.component.less']
})
export class CreateOrEditWechatMenuComponent extends AppComponentBase
  implements OnInit {
  /**
   * Wechat AppId
   */
  appId: string;

  /**
   * Wechat AppName
   */
  appName: string;

  /**
   * 加载
   */
  loading = true;

  /**
   * 菜单信息
   */
  allMemu: GetWechatMenuForEditOutput = new GetWechatMenuForEditOutput();

  /**
   *
   */
  currentMenuItem: IWechatMenuInfo;

  /**
   * 界面展示用的数据结构
   */
  menuList: IWechatMenuInfo[] = [];

  constructor(
    _injector: Injector,
    private activatedRoute: ActivatedRoute,
    private reuseTabService: ReuseTabService,
    private _wechatMenuService: WechatMenuAppSeviceServiceProxy
  ) {
    super(_injector);
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      // 初始化获取数据,如果未获取到,跳转到管理列表页
      this.appId = params.appId;
      this.appName = params.appName;
      if (!this.appId || !this.appName) {
        this.reuseTabService.replace('/app/wechat/wechat-app-config');
        return;
      }
      const currentTitle = this.l('EditWechatMenu') + ':' + this.appName;
      this.reuseTabService.title = currentTitle;
      this.titleSrvice.setTitle(currentTitle);

      if (!this.allMemu.menuTypeList) {
        this.getMenuData();
      }
    });
  }

  /**
   * 获取所有菜单数据
   * @param isRefresh 是否为刷新菜单,如果为true将显示消息通知
   */
  getMenuData(isRefresh: boolean = false) {
    this.loading = true;
    this.resetDatas();

    this._wechatMenuService
      .getMenuForEdit(this.appId)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe(result => {
        this.allMemu = result;
        this.resetDatas();

        (this.allMemu.menu as any).button.forEach(item => {
          this.menuList.push(this.processMenu(item));
        });

        if (isRefresh) {
          this.notify.success(this.l('PullWechatMenusSuccessfully'));
        }
      });
  }

  /**
   * 将数据格式组织成符合菜单按钮组件的数据结构
   */
  processMenu(sourceMenuItem: any, parent?: IWechatMenuInfo): IWechatMenuInfo {
    const menuItem: IWechatMenuInfo = {
      parentId: parent ? parent.text : null,
      parent: parent,
      id: sourceMenuItem.name,
      text: sourceMenuItem.name,
      subMenu: parent ? null : [],
      subMenuHorizontal: false,
      origin: sourceMenuItem
    };

    if (sourceMenuItem.sub_button) {
      sourceMenuItem.sub_button.forEach(item => {
        menuItem.subMenu.push(this.processMenu(item, menuItem));
      });
    }

    return menuItem;
  }

  /**
   * 选中的菜单按钮发生了改变
   * @param menu
   */
  menuSelectChanged(menuItem: IWechatMenuInfo) {
    if (!menuItem.origin) {
      menuItem.origin = new MenuFull_RootButton();
      menuItem.origin.name = menuItem.text;
      menuItem.origin.type = this.allMemu.menuTypeList[0].value;
    }
    this.currentMenuItem = menuItem;
  }

  /**
   * 提交菜单数据
   */
  save(): void {
    this.loading = true;
    const input: CreateOrEditWechatMenuInput = new CreateOrEditWechatMenuInput();
    input.appId = this.appId;
    input.menu = [];

    this.menuList.forEach(item => {
      input.menu.push(this.processSubmitMenuData(item));
    });

    this._wechatMenuService
      .createOrWechatEditMenu(input)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe(() => {
        this.notify.success(this.l('PushWechatMenusSuccessfully'));
      });
  }

  /**
   * 处理当前菜单数据格式为需要提交的数据格式
   * @param menuItem 菜单集合
   * @param parentMenu 父级菜单
   */
  private processSubmitMenuData(
    menuItem: IWechatMenuInfo,
    parentMenu?: MenuFull_RootButton
  ): MenuFull_RootButton {
    const meunFull: MenuFull_RootButton = MenuFull_RootButton.fromJS(
      menuItem.origin
    );

    if (menuItem.subMenu && menuItem.subMenu.length > 0) {
      meunFull.sub_button = [];

      menuItem.subMenu.forEach(item => {
        meunFull.sub_button.push(this.processSubmitMenuData(item, meunFull));
      });
    }

    return meunFull;
  }

  delete(item) {
    this._wechatMenuService
      .deleteMenuConditional(this.appId, item.menuid)
      .subscribe(() => {});
  }

  /**
   * 删除当前菜单项
   */
  removeCurrentMenuItem() {
    this.currentMenuItem.component.removeMenu(this.currentMenuItem);
    this.currentMenuItem = null;
  }

  /**
   * 移动菜单
   * @param direction 移动方向， left:左移 right:右移  up:上移 down:下移
   */
  moveMenuItem(direction: string) {
    this.currentMenuItem.component.moveMenuItem(
      this.currentMenuItem,
      direction
    );
  }

  /**
   * 重置数据
   */
  private resetDatas() {
    this.menuList = [];
    this.currentMenuItem = null;
  }
}
