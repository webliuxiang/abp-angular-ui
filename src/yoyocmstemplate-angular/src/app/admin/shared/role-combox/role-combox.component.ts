import {
  Component,
  OnInit,
  Injector,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import {
  RoleServiceProxy,
  RoleListDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/component-base/app-component-base';

@Component({
  selector: 'app-role-combox',
  templateUrl: './role-combox.component.html',
})
export class RoleComboxComponent extends AppComponentBase implements OnInit {
  /**
   * 角色数据
   */
  roles: RoleListDto[] = [];
  /**
   * 下拉框样式
   */
  @Input()
  dropDownStyle: any = null;

  /**
   * 选择模式，默认`multiple`
   */
  @Input()
  selectMode: 'multiple' | 'tags' | 'default' = 'multiple';
  /**
   * 已选项Value
   */
  @Input()
  selectedRole: any = undefined;
  /**
   * 选中回发父页面事件
   */
  @Output()
  selectedRoleChange: EventEmitter<any> = new EventEmitter<any>();

  constructor(private _roleService: RoleServiceProxy, injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    this._roleService.getAll(undefined).subscribe(result => {
      this.roles = result.items;
    });
  }

  /**
   * 选择事件
   * @param selectKey 选择Value
   */
  selectedChange(selectKey: any) {
    this.selectedRoleChange.emit(selectKey);
  }
}
