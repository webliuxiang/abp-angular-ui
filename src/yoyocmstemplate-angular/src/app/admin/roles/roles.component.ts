import { Component, OnInit, Injector, ViewChild, NgZone } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { RoleListDto, RoleServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRoleComponent } from '@app/admin/roles/create-or-edit-role/create-or-edit-role.component';
import { PermissionComboxComponent } from '@app/admin/shared/permission-combox/permission-combox.component';
import * as _ from 'lodash';
import { AppConsts } from 'abpPro/AppConsts';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styles: [],
})
export class RolesComponent extends PagedListingComponentBase<RoleListDto> {
  advancedFiltersVisible = false; // 是否显示高级过滤

  /**
   * 选中的权限过滤
   */
  selectedPermission: string[] = [];

  constructor(injector: Injector, private _roleService: RoleServiceProxy, public _zone: NgZone) {
    super(injector);
  }

  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._roleService
      .getPaged(
        this.selectedPermission,

        this.filterText,
        request.sorting,
        request.maxResultCount,
        request.skipCount,
      )
      .pipe(finalize(() => finishedCallback()))
      .subscribe(result => {
        this.dataList = result.items;
        this.showPaging(result);
      });
  }

  /**
   * 强制刷新
   */
  forceRefresh() {
    this.filterText = '';
    this.refreshGoFirstPage();
  }

  ngOnInit(): void {
    // 初始化加载表格数据
    this.refresh();
  }

  /**
   * 删除功能
   * @param entity 角色的实体信息
   */
  delete(entity: RoleListDto): void {
    if (entity.isStatic) {
      abp.message.warn(this.l('XUserCannotBeDeleted', AppConsts.userManagement.defaultAdminUserName));
      return;
    }
    this._roleService.delete(entity.id).subscribe(() => {
      this.refreshGoFirstPage();
      this.notify.success(this.l('SuccessfullyDeleted'));
    });
  }

  /**
   * 新增或编辑角色
   * @param id 当前角色Id
   */
  createOrEdit(id?: number): void {
    this.modalHelper.static(CreateOrEditRoleComponent, { id: id }).subscribe(res => {
      if (res) {
        this.refresh();
      }
    });
  }

  /**
   * 批量删除
   */
  batchDelete(): void {
    const selectCount = this.selectedDataItems.length;
    if (selectCount <= 0) {
      abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));

      return;
    }
    this.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined, res => {
      if (res) {
        const ids = _.map(this.selectedDataItems, 'id');
        this._roleService.batchDelete(ids).subscribe(() => {
          this.refresh();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  refreshTheWindwosPageEvents() {
    window.location.reload();
  }

  refreshCheckStatus(entityList: any[]): void {
    // 是否全部选中
    const allChecked = entityList.every(value => value.checked === true);
    // 是否全部未选中
    const allUnChecked = entityList.every(value => !value.checked);
    // 是否全选
    this.allChecked = allChecked;
    // 全选框样式控制
    this.checkboxIndeterminate = !allChecked && !allUnChecked;
    // 已选中数据
    this.selectedDataItems = entityList.filter(value => value.checked && !value.isStatic);
  }
}
