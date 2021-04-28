import { ModalComponentBase } from '@shared/component-base';
import { Component, Injector } from '@angular/core';

@Component({
  selector: 'app-img-show',
  templateUrl: './img-show.component.html',
  styles: []
})
export class ImgShowComponent extends ModalComponentBase {
  imgUrl = '';
  constructor(injector: Injector) {
    super(injector);
  }
}
