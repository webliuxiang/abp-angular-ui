import { Component, HostListener, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';

@Component({
  selector: 'header-storage',
  templateUrl: './header-storage.component.html',
  styles: [],
  host: {
    '[class.d-block]': 'true',
  },
})
export class HeaderStorageComponent extends AppComponentBase {
  clicked = true;

  constructor(injector: Injector) {
    super(injector);
  }

  @HostListener('click')
  _click() {
    this.message.confirm(this.l('MakeSureClearAllLocalStorage'), undefined, res => {
      if (res) {
        localStorage.clear();
        this.message.success(this.l('ClearFinished'));
      }
    });
  }
}
