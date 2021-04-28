import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { ProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from 'abpPro/AppConsts';

@Component({
  selector: 'app-friend-profile-picture',
  templateUrl: './friend-profile-picture.component.html',
  styles: [],
})
export class FriendProfilePictureComponent implements AfterViewInit {
  @Input() profilePictureId: string;
  @Input() userId: number;
  @Input() tenantId: number;
  @Input() cssClass = 'media-object';

  profilePicture = AppConsts.appBaseUrl + '/assets/common/images/default-profile-picture.png';

  constructor(private _profileService: ProfileServiceProxy) {}

  ngAfterViewInit(): void {
    this.setProfileImage();
  }

  private setProfileImage(): void {
    if (!this.profilePictureId) {
      this.profilePictureId = undefined;
    }

    if (!this.tenantId) {
      this.tenantId = undefined;
    }

    if (!this.profilePictureId) {
      return;
    }

    this._profileService
      .getFriendProfilePictureById(this.profilePictureId, this.userId, this.tenantId)
      .subscribe(result => {
        if (result && result.profilePicture) {
          this.profilePicture = 'data:image/jpeg;base64,' + result.profilePicture;
        }
      });
  }
}
