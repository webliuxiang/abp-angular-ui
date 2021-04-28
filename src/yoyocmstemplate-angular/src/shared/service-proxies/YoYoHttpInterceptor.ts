import { AbpHttpInterceptor } from 'abp-ng2-module';
import { HttpRequest, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class YoYoHttpInterceptor extends AbpHttpInterceptor {
  protected addAspNetCoreCultureHeader(headers: HttpHeaders): HttpHeaders {
    const cookieLangValue = abp.localization.currentLanguage.name;

    if (cookieLangValue && headers && !headers.has('.AspNetCore.Culture')) {
      headers = headers.set('.AspNetCore.Culture', cookieLangValue);
    }

    return headers;
  }
}
