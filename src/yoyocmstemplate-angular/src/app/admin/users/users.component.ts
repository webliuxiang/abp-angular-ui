import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  UserListDto,
  UserServiceProxy,
  EntityDtoOfInt64,
  PagedResultDtoOfUserListDto,
} from '@shared/service-proxies/service-proxies';
import { CreateOrEditUserComponent } from '@app/admin/users/create-or-edit-user/create-or-edit-user.component';
import { EditUserPermissionsComponent } from '@app/admin/users/edit-user-permissions/edit-user-permissions.component';
import { ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';

// import _ = require('../@types/lodash');
import * as _ from 'lodash';
import { AppConsts } from 'abpPro/AppConsts';
import { ImpersonationService } from '@shared/auth';
import { getBaseHref } from '../../../root.module';
import { FileDownloadService } from '@shared/utils';
import { TokenService } from 'abp-ng2-module';
import { UploadFile } from 'ng-zorro-antd/upload';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styles: [],
})
export class UsersComponent extends PagedListingComponentBase<UserListDto> implements OnInit {
  /**
   * 从Excel导入用户后台处理地址
   */
  importFromExcelUrl: string = AppConsts.remoteServiceBaseUrl + '/UserImport/ImportFromExcel';

  importFromExcelTemplateUrl: string =
    AppConsts.remoteServiceBaseUrl + '/yoyosoft/SampleFiles/ImportUsersSampleFile.xlsx';

  uploadHeaders: any;

  // 模糊搜索
  filterText = '';
  advancedFiltersVisible = false; // 是否显示高级过滤
  /**
   * 选中的权限过滤
   */
  selectedPermission: string[] = [];

  /**
   * 是否激活过滤
   */
  isActive: boolean = undefined;
  /**
   * 是否已验证邮箱过滤
   */
  isEmailConfirmed: boolean = undefined;
  /**
   * 选中的角色Ids过滤
   */
  role: number[] = undefined;

  /**
   * 是否显示解锁按钮
   */
  get enabledUnlock(): boolean {
    return (
      this.isGranted('Pages.Administration.Users.Edit') &&
      this.setting.getBoolean('Abp.Zero.UserManagement.UserLockOut.IsEnabled')
    );
  }

  constructor(
    injector: Injector,
    private _userService: UserServiceProxy,
    private _activatedRoute: ActivatedRoute,
    public _impersonationService: ImpersonationService,
    private _fileDownloadService: FileDownloadService,
    private _tokenService: TokenService,
  ) {
    super(injector);
    this.filterText = this._activatedRoute.snapshot.queryParams.filterText || '';
    // 设置头部信息
    this.uploadHeaders = {
      Authorization: 'Bearer ' + this._tokenService.getToken(),
    };
    console.log(
      this.isGrantedAny(
        'Pages.Administration.Users.Impersonation',
        'Pages.Administration.Users.ChangePermissions',
        'Pages.Administration.Users.Unlock',
      ),
    );
  }

  // 搜索
  onSearch(): void {
    this.refresh();
  }

  /**
   * 激活过滤
   * @param event 值
   */
  isActiveFilter(event: any): void {
    if (event === null || event === 'All') {
      this.isActive = undefined;
    } else {
      this.isActive = event;
    }
    this.refreshGoFirstPage();
  }

  /**
   * 邮件确认过滤
   * @param event 值
   */
  isEmailConfirmedFilter(event: any): void {
    if (event === null || event === 'All') {
      this.isEmailConfirmed = undefined;
    } else {
      this.isEmailConfirmed = event;
    }
    this.refreshGoFirstPage();
  }

  /**
   * 根据角色列表进行数据展示
   * @param roles 角色列表信息
   */
  getRolesAsString(roles): string {
    let roleNames = '';
    for (let j = 0; j < roles.length; j++) {
      if (roleNames.length) {
        roleNames = roleNames + ', ';
      }
      roleNames = roleNames + roles[j].roleName;
    }
    return roleNames;
  }

  /**
   * 添加或者编辑实体信息模态框
   * @param id 需要修改实体信息的ID
   */
  createOrEdit(id?: number): void {
    this.modalHelper.static(CreateOrEditUserComponent, { id: id }).subscribe(res => {
      if (res) {
        this.refresh();
      }
    });
  }

  editUserPermissions(user: UserListDto): void {
    this.modalHelper
      .open(EditUserPermissionsComponent, {
        userId: user.id,
        userName: user.userName,
      })
      .subscribe(result => {
      });
  }

  unlockUser(user: UserListDto): void {
    const data = new EntityDtoOfInt64();
    data.id = user.id;
    this._userService
      .unlock(data)
      .finally(() => {
      })
      .subscribe(() => {
        this.refresh();
        this.notify.success(this.l('SuccessfullyUnlock'));
      });
  }

  /**
   * 强制刷新
   */
  forceRefresh() {
    this.filterText = '';
    this.isEmailConfirmed = undefined;
    this.isActive = undefined;
    this.selectedPermission = undefined;
    this.role = undefined;
    this.refreshGoFirstPage();
  }

  /**
   * 获取远端数据
   * @param request
   * @param pageNumber
   * @param finishedCallback
   */
  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._userService
      .getPaged(
        this.selectedPermission,
        this.role,
        this.isEmailConfirmed,
        this.isActive,
        undefined,
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
      .subscribe((result: PagedResultDtoOfUserListDto) => {
        this.dataList = result.items;
        this.showPaging(result);
      });
  }

  /**
   * 批量删除
   */
  batchDelete(): void {
    const selectCount = this.selectedDataItems.length;
    if (selectCount <= 0) {
      abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));
      // this.miniMessage.warning(this.l('PleaseSelectAtLeastOneItem'));
      return;
    }
    this.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined, res => {
      if (res) {
        const ids = _.map(this.selectedDataItems, 'id');
        this._userService.batchDelete(ids).subscribe(() => {
          this.refreshGoFirstPage();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  /**
   * 下载user的模板
   */
  ImportUsersSampleFile(): void {
    this._fileDownloadService.downloadTemplateFile('ImportUsersSampleFile.xlsx');
  }

  exportToExcel(): void {
    this._userService.getUsersToExcel().subscribe(result => {
      console.log(result);
      this._fileDownloadService.downloadTempFile(result);
    });
    // 调用后端的到处方法
  }

  /**
   * 删除功能
   * @param entity 实体信息：User
   */
  delete(entity: UserListDto): void {
    if (entity.userName === AppConsts.userManagement.defaultAdminUserName) {
      abp.message.warn(this.l('XUserCannotBeDeleted', AppConsts.userManagement.defaultAdminUserName));
      return;
    }

    this._userService.delete(entity.id).subscribe(() => {
      this.refreshGoFirstPage();
      this.notify.success(this.l('SuccessfullyDeleted'));
    });
  }

  isAdmin(item: UserListDto): boolean {
    return item.userName === AppConsts.userManagement.defaultAdminUserName;
  }

  refreshCheckStatus(entityList: any[]): void {
    entityList.forEach(item => {
      if (item.userName === AppConsts.userManagement.defaultAdminUserName) {
        item.checked = false;
      }
    });

    // 是否全部选中
    const allChecked = entityList.every(value => value.checked === true);
    // 是否全部未选中
    const allUnChecked = entityList.every(value => !value.checked);
    // 是否全选
    this.allChecked = allChecked;
    // 全选框样式控制
    this.checkboxIndeterminate = !allChecked && !allUnChecked;
    // 已选中数据
    this.selectedDataItems = entityList.filter(value => value.checked);
  }

  /**
   * 选择Excel上传事件
   * @param info 反馈信息
   */
  uploadPictureChange(info: any) {
    if (info.type === 'success') {
      this.refreshGoFirstPage();
    }
  }
}
