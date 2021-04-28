import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { IWechatMenuInfo } from './interface';

@Component({
  selector: 'app-wechat-menu-button',
  templateUrl: './wechat-menu-button.component.html',
  styles: []
})
export class WechatMenuButtonComponent extends AppComponentBase implements OnInit {

  @Input()
  height: string;
  @Input()
  width: string;

  @Input()
  parentMenu: IWechatMenuInfo;

  private _menuList: IWechatMenuInfo[];

  @Output()
  dataChanged: EventEmitter<IWechatMenuInfo[]> = new EventEmitter<IWechatMenuInfo[]>();

  @Input()
  get menuList(): IWechatMenuInfo[] {
    if (this._menuList) {
      this.showAddMenuButton = this._menuList.length < this.maxLength;
    }

    return this._menuList;
  }

  set menuList(value: IWechatMenuInfo[]) {
    this._menuList = value;

    if (this._menuList) {
      this.showAddMenuButton = this._menuList.length < this.maxLength;
      this._menuList.forEach((item) => {
        item.component = this;
      });
    }

    if (this.dataChanged) {
      this.dataChanged.emit(value);
    }
  }

  @Output()
  menuClicked: EventEmitter<IWechatMenuInfo> = new EventEmitter<IWechatMenuInfo>();


  @Input()
  maxLength = 3;


  /**
   * 是否水平放置，默认为false
   */
  @Input()
  isHorizontal: boolean;

  /**
   * 子级是否水平放置,默认为false
   */
  @Input()
  subMenuIsHorizontal: boolean;

  get display(): string {
    if (this.isHorizontal) {
      return 'inline-block';
    }
    return 'block';
  }

  showAddMenuButton: boolean;

  constructor(
    injector: Injector
  ) {
    super(injector);
  }

  ngOnInit() {

  }

  /**
   * 添加菜单
   */
  addMenu() {
    if (this.menuList.length === this.maxLength) {
      return;
    }
    const newMenu: IWechatMenuInfo = {
      parentId: this.parentMenu ? this.parentMenu.id : null,
      parent: this.parentMenu,
      id: this.menuList.length + '',
      text: (this.parentMenu ? '子' : '主') + '菜单' + (this.menuList.length + 1),
      subMenu: this.parentMenu ? null : [],
      subMenuHorizontal: this.subMenuIsHorizontal,
      component: this,
      origin: null
    };

    // 插入菜单项
    const index: number = this.parentMenu ? 0 : this.menuList.length;
    this.menuList.splice(index, 0, newMenu);


    this.showAddMenuButton = !(this.menuList.length === this.maxLength);
  }


  /**
   * 点击菜单项
   * @param menu 
   */
  menuClick(menu: IWechatMenuInfo) {
    if (this.menuClicked) {
      this.menuClicked.emit(menu);
    }
  }

  /**
   * 删除一项
   * @param item 
   */
  removeMenu(item: IWechatMenuInfo) {
    const index = this.menuList.indexOf(item);
    if (index > -1) {
      this.menuList.splice(index, 1);
    }
  }


  /**
   * 移动菜单项
   * @param item 菜单项
   * @param direction 移动方向， left:左移 right:右移  up:上移 down:下移
   */
  moveMenuItem(item: IWechatMenuInfo, direction: string) {
    const index = this.menuList.indexOf(item);

    if (this.isHorizontal) {
      // 显示和使用是相同的，所以这里是正的
      if (direction === 'left') {
        this.zIndexDown(this.menuList, index);
        return;
      }
      if (direction === 'right') {
        this.zIndexUp(this.menuList, index, this.menuList.length);
        return;
      }
    }
    else {
      // 显示和实际是相反的，所以这里是反的
      if (direction === 'up') {
        this.zIndexDown(this.menuList, index);
        return;
      }
      if (direction === 'down') {
        this.zIndexUp(this.menuList, index, this.menuList.length);
        return;
      }
    }
  }

  /**
  * 数组元素交换位置
  * @param {array} arr 数组
  * @param {number} index1 添加项目的位置
  * @param {number} index2 删除项目的位置
  * index1和index2分别是两个数组的索引值，即是两个要交换元素位置的索引值，如1，5就是数组中下标为1和5的两个元素交换位置
  */
  private swapArray(arr, index1, index2) {
    arr[index1] = arr.splice(index2, 1, arr[index1])[0];
    return arr;
  }

  /**
   * 上移 将当前数组index索引与后面一个元素互换位置，向数组后面移动一位
   * @param arr 
   * @param index 
   * @param length 
   */
  private zIndexUp(arr, index, length) {
    if (index + 1 != length) {
      this.swapArray(arr, index, index + 1);
    } else {

    }
  }

  /**
   * 下移 将当前数组index索引与前面一个元素互换位置，向数组前面移动一位
   * @param arr 
   * @param index 
   */
  private zIndexDown(arr, index) {
    if (index != 0) {
      this.swapArray(arr, index, index - 1);
    } else {

    }
  }
}
