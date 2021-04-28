import { Injectable } from '@angular/core';
import { AppConsts } from 'abpPro/AppConsts';

@Injectable()
export class AppAuthService {
  logout(reload?: boolean): void {
    abp.auth.clearToken();
    if (reload !== false) {
      location.href = AppConsts.appBaseUrl;
    }
  }
}
