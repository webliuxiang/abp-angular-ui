import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, OnInit, ViewChild, Injector, Input } from '@angular/core';
import { PermissionTreeComponent } from '@app/admin/shared/permission-tree/permission-tree.component';
import {
  UserServiceProxy,
  UpdateUserPermissionsInput,
  EntityDtoOfInt64
} from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-edit-user-permissions',
  templateUrl: './edit-user-permissions.component.html',
  styles: []
})
export class EditUserPermissionsComponent extends ModalComponentBase
  implements OnInit {
  @ViewChild(PermissionTreeComponent)
  permissionTree: PermissionTreeComponent;

  /**
   * 用户Id
   */
  userId: number;
  /**
   * 用户名
   */
  userName?: string;

  saving = false;
  resettingPermissions = false;

  constructor(injector: Injector, private userService: UserServiceProxy) {
    super(injector);
  }

  ngOnInit() {
    const self = this;

    // self._roleService.getForEdit(self.id).subscribe(result => {
    //   self.role = result.role;

    //   self.permissionTree.editData = result;
    //   self.setFormValues(this.role);
    // });

    self.userService
      .getPermissionsTreeForEdit(self.userId)
      .subscribe(result => {
        this.permissionTree.editData = result;
      });
  }

  /**
   * 保存
   */
  save(): void {
    this.saving = true;
    this.saving = true;
    const input = new UpdateUserPermissionsInput();
    input.id = this.userId;
    input.grantedPermissionNames = this.permissionTree.getGrantedPermissionNames();

    this.userService
      .updatePermissions(input)
      .finally(() => {})
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success();
      });
  }

  /**
   * 重置权限
   */
  resetPermissions(): void {
    const input = new EntityDtoOfInt64();
    input.id = this.userId;

    this.resettingPermissions = true;
    this.userService
      .resetSpecificPermissions(input)
      .finally(() => {
        this.resettingPermissions = false;
      })
      .subscribe(() => {
        this.notify.success(this.l('ResetSuccessfully'));
        this.userService
          .getPermissionsTreeForEdit(this.userId)
          .subscribe(result => {
            this.permissionTree.editData = result;
          });
      });
  }
}
