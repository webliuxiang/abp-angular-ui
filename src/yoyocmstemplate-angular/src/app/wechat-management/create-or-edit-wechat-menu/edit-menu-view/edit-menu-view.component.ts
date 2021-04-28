import { Component, OnInit, ViewEncapsulation, Injector, Input, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { IWechatMenuInfo } from '../../components/wechat-menu-button/interface';

@Component({
  selector: 'app-edit-menu-view',
  templateUrl: './edit-menu-view.component.html',
  styleUrls: ['./edit-menu-view.component.less'],
  encapsulation: ViewEncapsulation.None,
})
export class EditMenuViewComponent extends AppComponentBase implements OnInit {


  private _menuList: IWechatMenuInfo[];

  @Output()
  dataChanged: EventEmitter<IWechatMenuInfo[]> = new EventEmitter<IWechatMenuInfo[]>();

  @Input()
  get menuList(): IWechatMenuInfo[] {
    return this._menuList;
  }

  @Output()
  menuClicked: EventEmitter<IWechatMenuInfo> = new EventEmitter<IWechatMenuInfo>();

  set menuList(value: IWechatMenuInfo[]) {
    this._menuList = value;

    if (this.dataChanged) {
      this.dataChanged.emit(value);
    }
  }



  constructor(
    _injector: Injector
  ) {
    super(_injector);
  }

  ngOnInit() {

  }



  menuSelectChanged(menu: IWechatMenuInfo) {
    if (this.menuClicked) {
      this.menuClicked.emit(menu);
    }
  }

}
