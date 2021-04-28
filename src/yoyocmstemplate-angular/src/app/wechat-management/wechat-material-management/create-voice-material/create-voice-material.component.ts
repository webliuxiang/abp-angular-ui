import { Component, OnInit, Injector } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base';

@Component({
  selector: 'app-create-voice-material',
  templateUrl: './create-voice-material.component.html',
  styles: []
})
export class CreateVoiceMaterialComponent extends ModalComponentBase implements OnInit {

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  ngOnInit() {
  }

}
