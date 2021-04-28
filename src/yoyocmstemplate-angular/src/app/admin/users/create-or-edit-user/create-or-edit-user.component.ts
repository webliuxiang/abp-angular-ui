import { Component, OnInit, Injector, ViewChild, AfterViewInit } from '@angular/core';

import { AppConsts } from 'abpPro/AppConsts';

import * as _ from 'lodash';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
// tslint:disable-next-line:max-line-length
import {
  IOrganizationUnitsTreeComponentData,
  OrganizationUnitsTreeComponent,
} from '@app/admin/shared/organization-unit-tree/organization-unit-tree.component';
import {
  UserRoleDto,
  OrganizationUnitListDto,
  UserEditDto,
  UserServiceProxy,
  CreateOrUpdateUserInput,
  ProfileServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { TokenService } from 'abp-ng2-module';

@Component({
  selector: 'app-create-or-edit-user',
  templateUrl: './create-or-edit-user.component.html',
  styleUrls: ['./create-or-edit-user.component.less'],
})
export class CreateOrEditUserComponent extends ModalComponentBase implements OnInit, AfterViewInit {
  @ViewChild(OrganizationUnitsTreeComponent)
  organizationUnitTree: OrganizationUnitsTreeComponent;

  /**
   * 修改时用户Id
   */
  id?: number;
  /**
   * 发送激活邮件
   */
  sendActivationEmail = true;
  sendActivationEmailDisabled = false;
  /**
   * 角色数据列表
   */
  roles: UserRoleDto[];
  /**
   * 所有机构数据
   */
  allOrganizationUnits: OrganizationUnitListDto[];
  /**
   * 随机密码
   */
  setRandomPassword = false;
  /**
   * 当前用户所属机构代码列表
   */
  memberedOrganizationUnits: string[];
  /**
   * 是否启用双重因素身份验证
   */
  isTwoFactorEnabled: boolean = this.setting.getBoolean('Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled');
  /**
   * 是否启用锁定
   */
  isLockoutEnabled: boolean = this.setting.getBoolean('Abp.Zero.UserManagement.UserLockOut.IsEnabled');

  /** 用户实体信息 */
  user: UserEditDto = new UserEditDto();

  /**
   * 是否为管理员
   */
  isAdmin = false;

  /**
   * 图片上传后台处理地址
   */
  public uploadPictureUrl: string = AppConsts.remoteServiceBaseUrl + '/Profile/UploadProfilePictureReturnFileId';

  // public UploadPictureUrl = this._fileUploadService.downloadTemplateFile;


  passwordRepeatStr: string;

  //#endregion
  constructor(
    injector: Injector,
    private _userService: UserServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if (!this.id) {
      this.setRandomPassword = true;
      this.sendActivationEmail = true;
    } // 初始化数据
    this.init();
  }

  ngAfterViewInit(): void {
  }

  /**
   * 获取数据
   */
  init(): void {
    this._userService.getForEditTree(this.id).subscribe(result => {
      this.user = result.user;
      // 是否为管理员
      this.isAdmin = result.user.userName === AppConsts.userManagement.defaultAdminUserName;
      // 角色
      this.roles = result.roles;
      // 组织机构树

      this.allOrganizationUnits = result.allOrganizationUnits;
      this.memberedOrganizationUnits = result.memberedOrganizationUnits;

      this.user.profilePictureId = result.user.profilePictureId;
      if (this.id) {
        setTimeout(() => {
          this.setRandomPassword = false;
        }, 0);
        this.sendActivationEmail = false;
      }
      // 设置组织机构树
      this.setOrganizationUnitTreeData();
      // console.log(this.uploadPictureUrl);
      this.onActiveChange(this.user.isActive);
    });
  }

  setOrganizationUnitTreeData(): any {
    this.organizationUnitTree.data = ({
      allOrganizationUnits: this.allOrganizationUnits,
      selectedOrganizationUnits: this.memberedOrganizationUnits,
    } as IOrganizationUnitsTreeComponentData);
  }

  // 提交保存信息
  save(): void {
    const input = new CreateOrUpdateUserInput();
    input.user = this.user;
    // 后端生成随机密码 和 给用户发送邮件通知激活
    input.setRandomPassword = this.setRandomPassword;
    input.sendActivationEmail = this.sendActivationEmail;
    input.assignedRoleNames = _.map(_.filter(this.roles, { isAssigned: true }), role => role.roleName);
    // 组织机构
    input.organizationUnits = this.organizationUnitTree.getSelectedOrganizations();

    this._userService
      .createOrUpdate(input)
      .finally(() => {
        this.saving = false;
      })
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success();
      });
  }

  /**
   * 是否为编辑状态
   */
  isEdit(): boolean {
    return this.id !== -1;
  }

  /**
   * 获取选中角色的数量
   */
  getAssignedRoleCount(): number {
    return _.filter(this.roles, { isAssigned: true }).length;
  }

  onActiveChange(event) {
    if (event === true) {
      this.sendActivationEmail = true;
      this.sendActivationEmailDisabled = true;
    } else {
      this.sendActivationEmailDisabled = false;
    }
  }

  //#region 头像功能

  upLoadProfilePictureSuccess(event) {
    this.user.profilePictureId = event;
  }
  removeProfilePicture() {
    this.user.profilePictureId = '';
  }

  //#endregion
}
