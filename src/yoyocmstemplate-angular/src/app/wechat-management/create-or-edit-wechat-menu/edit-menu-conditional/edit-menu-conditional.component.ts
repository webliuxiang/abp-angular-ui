import { Component, OnInit, Injector, Input, EventEmitter, Output } from '@angular/core';
import { MenuMatchRule } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/component-base';

@Component({
  selector: 'app-edit-menu-conditional',
  templateUrl: './edit-menu-conditional.component.html',
  styles: []
})
export class EditMenuConditionalComponent extends AppComponentBase implements OnInit {

  // TODO:个性化菜单配置,尚未完成，预留位置

  private _menuMatchRule: MenuMatchRule;

  set menuMatchRule(value: MenuMatchRule) {
    this._menuMatchRule = value;
    if (this.menuMatchRuleChanged) {
      this.menuMatchRuleChanged.emit(this._menuMatchRule);
    }
  }
  /**
   * 个性化菜单匹配规则
   */
  @Input()
  get menuMatchRule(): MenuMatchRule {
    return this._menuMatchRule;
  }

  /**
   * 个性化菜单匹配规则发生改变
   */
  @Output()
  menuMatchRuleChanged: EventEmitter<MenuMatchRule> = new EventEmitter<MenuMatchRule>();


  /**
   * 性别
   */
  gender: [
    { key: 'Male', value: 1 },
    { key: 'Female', value: 2 }
  ];



  constructor(
    injector: Injector
  ) {
    super(injector);
  }

  ngOnInit() {

  }

}
