import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';

@Component({
  selector: 'app-file-manager',
  templateUrl: './file-manager.component.html',
})
export class SysFileManagerComponent extends AppComponentBase implements OnInit {
  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    //   throw new Error('Method not implemented.');
  }
}
