import { ArrayService } from '@delon/util';
import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { NzFormatEmitEvent, NzTreeComponent, NzTreeNode } from 'ng-zorro-antd/tree';
import { TreePermissionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/component-base/app-component-base';
import { PermissionTreeEditModel } from '@app/admin/shared/permission-tree/types';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'permission-tree',
  templateUrl: './permission-tree.component.html',
})
export class PermissionTreeComponent extends AppComponentBase
  implements OnInit {
  /**
   * 源数据
   */
  private _editData: PermissionTreeEditModel;

  /**
   * 默认需要选中的权限名称集合
   */
  defaultCheckedPermissionNames = [];

  /**
   * 父子节点选中状态不再关联
   */
  checkStrictly = true;

  /**
   * 加载中
   */
  loading = false;

  /**
   * 源数据
   */
  set editData(val: PermissionTreeEditModel) {
    this._editData = val;
    this.arrToTreeNode();
    this.defaultCheckedPermissionNames = val.grantedPermissionNames;
  }

  /**
   * 过滤文本
   */
  filterText: string;

  /**
   * NzTree数据
   */
  _treeData: NzTreeNode[] = [];

  ngOnInit(): void {
    //  throw new Error('Method not implemented.');
  }
  /**
   * 构造函数
   * @param injector 注入器
   * @param _arrayService 数组常用工具
   */
  constructor(injector: Injector, private _arrayService: ArrayService) {
    super(injector);
  }

  /**
   * 重组Tree数据
   */
  arrToTreeNode(): void {
    this.loading = true;

    this._treeData = this._arrayService.arrToTreeNode(
      this._editData.permissions,
      {
        idMapName: 'name',
        parentIdMapName: 'parentName',
        titleMapName: 'displayName'
      },
    );

    this._arrayService.visitTree(this._treeData, item => {
      item.isChecked = this._editData.grantedPermissionNames.find(p => p == item.key) ? true : false;
    });

    // 延时设置子父节点checkbox关联状态，否则有父节点选中则全部选中了
    setTimeout(() => {
      this.checkStrictly = false;
      this.loading = false;
    }, 500);
  }
  /**
   * 重新加载
   */
  reload(): void {
    this.checkStrictly = true;
    this.arrToTreeNode();
    this.filterText = '';
  }
  /**
   * 获取已授权项`key`集合
   */
  getGrantedPermissionNames(): string[] {
    const permissionNames: string[] = this._arrayService.getKeysByTreeNode(
      this._treeData,
    );
    return permissionNames;
  }
  /**
   * 过滤文本为空时，刷新树
   */
  filterTextEmptyChange() {
    if (!this.filterText) {
      this.reload();
    }
  }

  // TODO: 隔离地带
}
