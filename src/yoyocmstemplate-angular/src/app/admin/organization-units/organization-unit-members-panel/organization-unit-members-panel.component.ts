import { Component, OnInit, Output, EventEmitter, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  OrganizationUnitUserListDto,
  OrganizationUnitServiceProxy,
  PagedResultDtoOfOrganizationUnitUserListDto,
} from '@shared/service-proxies/service-proxies';
import { NzTreeNode } from 'ng-zorro-antd/tree';
import { finalize } from 'rxjs/operators';
import * as _ from 'lodash';
import { AddMemberComponent } from '@app/admin/organization-units/add-member/add-member.component';

@Component({
  selector: 'app-organization-unit-members-panel',
  templateUrl: './organization-unit-members-panel.component.html',
  styles: [],
})
export class OrganizationUnitMembersPanelComponent extends PagedListingComponentBase<OrganizationUnitUserListDto>
  implements OnInit {
  @Output() memberRemoved = new EventEmitter<number[]>();
  @Output() membersAdded = new EventEmitter<number[]>();

  /**
   * 搜索文本
   */
  filterText = '';

  /**
   * 当前选中机构节点
   */
  private _organizationUnit: NzTreeNode = null;

  /**
   * 当前选中机构
   */
  get organizationUnit(): NzTreeNode {
    return this._organizationUnit;
  }

  set organizationUnit(ou: NzTreeNode) {
    if (this._organizationUnit === ou) {
      return;
    }
    this._organizationUnit = ou;
    if (ou) {
      this.refresh();
    }
  }

  /**
   * 构造函数
   * @param injector 注入器
   * @param _organizationUnitService 组织机构服务
   */
  constructor(injector: Injector, private _organizationUnitService: OrganizationUnitServiceProxy) {
    super(injector);
  }
  ngOnInit() {}

  /**
   * 获取数据列表
   * @param request 分页请求必须参数
   * @param pageNumber 当前页码
   * @param finishedCallback 完成后回调函数
   */
  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    if (!this._organizationUnit) {
      return;
    }

    this._organizationUnitService
      .getPagedOrganizationUnitUsers(
        // tslint:disable-next-line:radix
        parseInt(this._organizationUnit.key),
        this.filterText,
        request.sorting,

        request.maxResultCount,
        request.skipCount,
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
      )
      .subscribe((result: PagedResultDtoOfOrganizationUnitUserListDto) => {
        this.dataList = result.items;

        this.showPaging(result);
      });
  }

  /**
   * 移除用户
   * @param user 当前用户实体
   */
  removeMember(user: OrganizationUnitUserListDto): void {
    // tslint:disable-next-line:radix
    const _ouId = parseInt(this.organizationUnit.key);
    this._organizationUnitService.removeUser(user.id, _ouId).subscribe(() => {
      this.refreshGoFirstPage();
      this.notify.success(this.l('SuccessfullyRemoved'));
      this.memberRemoved.emit([user.id]);
    });
  }
  /**
   * 批量删除
   */
  batchDelete(): void {
    const selectCount = this.selectedDataItems.length;
    debugger;
    console.log(this.selectedDataItems);
    if (selectCount <= 0) {
      abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));
      return;
    }
    this.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined, res => {
      if (res) {
        // tslint:disable-next-line:radix
        const _ouId = parseInt(this.organizationUnit.key);
        const ids = _.map(this.selectedDataItems, 'id');
        this._organizationUnitService.batchRemoveUserFromOrganizationUnit(_ouId, ids).subscribe(() => {
          this.refreshGoFirstPage();
          this.notify.success(this.l('SuccessfullyDeleted'));
          this.memberRemoved.emit(ids);
        });
      }
    });
  }
  /**
   * 增加成员
   */
  addUser(): void {
    this.modalHelper
      .static(AddMemberComponent, {
        // tslint:disable-next-line:radix
        organizationUnitId: parseInt(this.organizationUnit.key),
      })
      .subscribe((res: number[]) => {
        if (res) {
          this.addMembers(res);
        }
      });
  }

  /**
   * 新增后广播事件
   * @param data 新增后回传数据
   */
  addMembers(userIds: number[]): void {
    this.membersAdded.emit(userIds);
    this.refresh();
  }
  /**
   * 清除条件并刷新
   */
  clearFilterAndRefresh(): void {
    this.filterText = '';
    this.refresh();
  }
}