import { OrganizationUnitListDto } from '@shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { NzTreeNode } from 'ng-zorro-antd/tree';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/component-base/app-component-base';
import { ArrayService } from '@delon/util';

export interface IOrganizationUnitsTreeComponentData {
  allOrganizationUnits: OrganizationUnitListDto[];
  selectedOrganizationUnits: string[];
}

@Component({
  selector: 'organization-unit-tree',
  templateUrl: './organization-unit-tree.component.html',
})
export class OrganizationUnitsTreeComponent extends AppComponentBase implements OnInit {
  /**
   * 源数据
   */
  private _sourceData: IOrganizationUnitsTreeComponentData;

  /**
   * 默认需要选中的机构键值集合
   */
  defaultCheckedOrganizationUnits: number[] = [];

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
  set data(data: IOrganizationUnitsTreeComponentData) {
    this._sourceData = data;
    this.arrToTreeNode();
  }

  /**
   * 过滤文本
   */
  filterText: string;

  /**
   * NzTree数据
   */
  _treeData: NzTreeNode[] = [];

  /**
   * 构造函数
   * @param injector 注入器
   * @param _arrayService 数组常用工具
   */
  constructor(injector: Injector, private _arrayService: ArrayService) {
    super(injector);
  }

  ngOnInit(): void {}

  /**
   * 重组Tree数据
   */
  arrToTreeNode(): void {
    this.loading = true;

    this._treeData = this._arrayService.arrToTreeNode(this._sourceData.allOrganizationUnits, {
      idMapName: 'id',
      parentIdMapName: 'parentId',
      titleMapName: 'displayName',
    });

    this._arrayService.visitTree(this._treeData, item => {
      item.isChecked = this._sourceData.selectedOrganizationUnits.find(p => p === item.origin.code) ? true : false;
    });

    this.loading = false;
  }

  /**
   * 重新加载
   */
  reload(): void {
    this.arrToTreeNode();
    this.filterText = '';
  }

  /**
   * 获取已选项`key`集合
   */
  getSelectedOrganizations(): number[] {
    const organizationIds: number[] = this._arrayService.getKeysByTreeNode(this._treeData);
    return organizationIds;
  }

  /**
   * 过滤文本为空时，刷新树
   */
  filterTextEmptyChange() {
    if (!this.filterText) {
      this.reload();
    }
  }
}
