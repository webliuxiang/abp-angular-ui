import { Directive, Input, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import { ImgComponent } from './img.component';

@Directive({ selector: '[dialog-img]' })
export class ImgDirective {
  @Input()
  multiple: boolean | number = false;

  @Input()
  field: string;

  @Output()
  selected = new EventEmitter<any>();

  constructor(private modalHelper: ModalHelper) {}

  @HostListener('click')
  _click() {
    this.modalHelper
      .create(
        ImgComponent,
        {
          multiple: this.multiple,
        },
        {
          size: 1000,
          modalOptions: {
            nzClosable: false,
          },
        },
      )
      .subscribe((res: any) => {
        console.log('指令');
        console.log(res);
        if (Array.isArray(res)) {
          let ret = res.length > 0 && !this.multiple ? res[0] : res;
          if (this.field && ret) { ret = ret[this.field]; }
          console.log(ret);
          this.selected.emit(ret);
        }
      });
  }
}
