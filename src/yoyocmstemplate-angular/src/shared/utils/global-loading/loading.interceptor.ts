import { Injectable } from '@angular/core';

import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';

import { Observable } from 'rxjs';
import { LoadingService } from './loading.service';
import { finalize } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  constructor(private loadingOverlayService: LoadingService) {

  }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.loadingOverlayService.show();
    return next.handle(req).pipe(finalize(() => {
      setTimeout(() => {
        this.loadingOverlayService.hide();
      }, 500);
    }));
    // return next.handle(req);
  }
}
