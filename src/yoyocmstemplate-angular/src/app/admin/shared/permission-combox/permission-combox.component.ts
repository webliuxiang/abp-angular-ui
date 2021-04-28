import {
  OnInit,
  Component,
  Input,
  Output,
  EventEmitter,
  Injector,
} from '@angular/core';
import { AppComponentBase } from '@shared/component-base/app-component-base';
import {
  FlatPermissionWithLevelDto,
  PermissionServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { NzTreeNode } from 'ng-zorro-antd/tree';
import { ArrayService } from '@delon/util';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-permission-combox',
  templateUrl: './permission-combox.component.html',
})
export class PermissionComboxComponent extends AppComponentBase
  implements OnInit {
  /**
   * 源数据
   */
  permissions: FlatPermissionWithLevelDto[] = [];

  /**
   * 是否启用多选，默认`multiple=false`
   */
  @Input()
  multiple = false;

  /**
   * 下拉框样式
   */
  @Input()
  dropDownStyle: any = null;

  /**
   * 选中的权限
   */
  @Input()
  selectedPermission: any = undefined;

  /**
   * 选择后发射到父页面事件
   */
  @Output()
  selectedPermissionChange: EventEmitter<any> = new EventEmitter<any>();

  /**
   * 加载中
   */
  loading = false;

  /**
   * NzTree数据
   */
  _treeData: NzTreeNode[] = [];

  constructor(
    private _permissionService: PermissionServiceProxy,
    private _arrayService: ArrayService,
    injector: Injector,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._permissionService.getAllPermissions().subscribe(result => {
      this.permissions = result.items;
      this.arrToTreeNode();
    });
  }

  /**
   * 重组Tree数据
   */
  arrToTreeNode(): void {
    this.loading = true;
    this._treeData = this._arrayService.arrToTreeNode(this.permissions, {
      idMapName: 'name',
      parentIdMapName: 'parentName',
      titleMapName: 'displayName',
    });

    // 延时设置子父节点checkbox关联状态，否则有父节点选中则全部选中了
    setTimeout(() => {
      this.loading = false;
    }, 500);
  }

  /**
   * 选择事件
   * @param node 选择节点
   */
  selectedChange(selectKey: any) {
    this.selectedPermissionChange.emit(selectKey);
  }
}
