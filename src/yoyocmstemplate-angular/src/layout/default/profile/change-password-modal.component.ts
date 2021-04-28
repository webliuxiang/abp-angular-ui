import { Component, OnInit, Injector } from '@angular/core';
import {
  ProfileServiceProxy,
  ChangePasswordInput,
} from '@shared/service-proxies/service-proxies';
import { AbstractControl, Validators, FormControl } from '@angular/forms';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';

@Component({
  selector: 'app-change-password-modal',
  templateUrl: './change-password-modal.component.html',
  styles: [],
})
export class ChangePasswordModalComponent extends ModalComponentBase
  implements OnInit {
  input: ChangePasswordInput = new ChangePasswordInput();
  confirmNewPasswordStr = '';

  constructor(injector: Injector, private profileService: ProfileServiceProxy) {
    super(injector);
  }
  ngOnInit() { }

  save(): void {
    this.saving = true;
    this.profileService
      .changePassword(this.input)
      .finally(() => {
        this.saving = false;
      })
      .subscribe(() => {
        this.notify.success(this.l('YourPasswordHasChangedSuccessfully'));
        this.success();
      });
  }
}
