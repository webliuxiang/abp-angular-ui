import { Component, HostListener, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import * as screenfull from 'screenfull';
import { Screenfull } from 'screenfull';

@Component({
  selector: 'header-fullscreen',
  templateUrl: './header-full-screen.component.html',
  styles: [],
  host: {
    '[class.d-block]': 'true',
  },
})
export class HeaderFullScreenComponent extends AppComponentBase {
  status = false;

  constructor(injector: Injector) {
    super(injector);
  }

  @HostListener('window:resize')
  _resize() {
    this.status = (screenfull as Screenfull).isFullscreen;
  }

  @HostListener('click')
  _click() {
    if ((screenfull as Screenfull).isEnabled) {
      (screenfull as Screenfull).toggle();
    }
  }
}
