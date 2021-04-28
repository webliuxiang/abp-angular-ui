import { Component, OnInit, Injector } from '@angular/core';
import {
  CurrentUserProfileEditDto,
  ProfileServiceProxy
} from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';
import { AppConsts } from 'abpPro/AppConsts';
import { Validators } from '@angular/forms';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';

@Component({
  selector: 'app-my-settings-modal',
  templateUrl: './my-settings-modal.component.html',
  styles: []
})
export class MySettingsModalComponent extends ModalComponentBase
  implements OnInit {
  user: CurrentUserProfileEditDto = new CurrentUserProfileEditDto();
  isAdmin = true;

  constructor(injector: Injector, private profileService: ProfileServiceProxy) {
    super(injector);
  }

  ngOnInit() {
    this.profileService.getCurrentUserProfileForEdit().subscribe(result => {
      this.user = result;
      this.isAdmin =
        this.user.userName === AppConsts.userManagement.defaultAdminUserName;
    });
  }

  save(): void {
    this.saving = true;
    this.profileService
      .updateCurrentUserProfile(this.user)
      .finally(() => {
        this.saving = false;
      })
      .subscribe(() => {
        this.appSession.user.userName = this.user.userName;
        this.appSession.user.emailAddress = this.user.emailAddress;
        this.notify.success(this.l('SavedSuccessfully'));
        this.success();
      });
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
