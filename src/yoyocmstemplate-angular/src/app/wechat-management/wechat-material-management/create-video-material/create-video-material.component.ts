import { Component, OnInit, Injector } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base';

@Component({
  selector: 'app-create-video-material',
  templateUrl: './create-video-material.component.html',
  styles: []
})
export class CreateVideoMaterialComponent extends ModalComponentBase implements OnInit {

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  ngOnInit() {
  }

}
