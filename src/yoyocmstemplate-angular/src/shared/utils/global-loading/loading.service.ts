import { Overlay, OverlayRef, OverlayConfig } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import {
  Injectable,
  ViewContainerRef,
  ApplicationRef,
  FactoryProvider,
  Type,
  Injector
} from '@angular/core';
import { OverlayComponent } from './overlay/overlay.component';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  static appliedComponents: any;

  constructor(public overlay: Overlay, private injector: Injector) {}
  showLoading = false;
  overlayRef: OverlayRef;
  static include(componentType: string | Type<any>): any {
    if (!LoadingService.appliedComponents.includes(componentType)) {
      LoadingService.appliedComponents.push(componentType);
    }
  }

  public show(message = '', target?: ViewContainerRef) {
    if (!this.showLoading) {
      const config = new OverlayConfig();
      if (target) {
        config.positionStrategy = this.overlay
          .position()
          .flexibleConnectedTo(target.element)
          .withPositions([
            {
              originY: 'center',
              originX: 'center',
              overlayX: 'start',
              overlayY: 'top'
            }
          ]);
      } else {
        config.positionStrategy = this.overlay
          .position()
          .global()
          .centerHorizontally() // 水平居中
          .centerVertically();
      }
      config.hasBackdrop = true;
      this.overlayRef = this.overlay.create(config); // OverlayRef, overlay层
      this.overlayRef.attach(new ComponentPortal(OverlayComponent, target));
      this.showLoading = true;
    }
  }

  public hide() {
    if (this.showLoading) {
      this.overlayRef.dispose();
      this.showLoading = false;
    }
  }
}

// export const LoadingServiceProvider: FactoryProvider = {
//   provide: LoadingService,
//   useFactory: (overlay) => new LoadingService(overlay),
//   deps: [Overlay]
// }
