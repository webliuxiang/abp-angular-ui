import {
  Component,
  Input,
  Optional,
  Inject,
  AfterViewInit,
  Renderer2,
  ElementRef,
  ChangeDetectionStrategy,
} from '@angular/core';
import { InputBoolean } from '@delon/util';
import { TitleService } from '@delon/theme';
import { ReuseTabService } from '@delon/abc';

@Component({
  selector: 'app-page-grid',
  templateUrl: './page-grid.component.html',
  host: {
    '[class.alain-pro__page-grid]': 'true',
    '[class.alain-pro__page-grid-no-spacing]': 'noSpacing',
    '[class.alain-pro__page-grid-wide]': 'false',
  },
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageGridComponent implements AfterViewInit {
  @Input() @InputBoolean() loading = false;

  @Input() @InputBoolean() noSpacing = false;

  @Input() style: any;

  @Input()
  set title(value: string) {
    if (value) {
      if (this.titleSrv) {
        this.titleSrv.setTitle(value);
      }
      if (this.reuseSrv) {
        this.reuseSrv.title = value;
      }
    }
  }

  constructor(
    private el: ElementRef,
    private rend: Renderer2,
    @Optional() @Inject(TitleService) private titleSrv: TitleService,
    @Optional() @Inject(ReuseTabService) private reuseSrv: ReuseTabService,
  ) {}

  ngAfterViewInit() {
    if (this.style) {
      Object.keys(this.style).forEach((key: string) => {
        this.rend.setStyle(this.el.nativeElement, key, this.style[key]);
      });
    }
  }
}
