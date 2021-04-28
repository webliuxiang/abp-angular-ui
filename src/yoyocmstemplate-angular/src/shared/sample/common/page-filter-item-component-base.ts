import { ChangeDetectorRef, EventEmitter, Injector, Input, OnChanges, Output, SimpleChange, SimpleChanges, Directive } from '@angular/core';
import { ControlComponentBase } from '@shared/component-base/control-component-base';
import { SampleDataSourceService } from '@shared/sample/components/sample-data-source.service';

/** page filter 组件的基类 */
export abstract class PageFilterItemComponentBase<T> extends ControlComponentBase<T>
  implements OnChanges {

  /** 配置文件中的参数 */
  @Input() args: any;

  /** 外部组件传递的参数 */
  @Input() externalArgs: any = {};

  /** */
  cdr: ChangeDetectorRef;

  /** 数据源服务 */
  sampleDataSourceSer: SampleDataSourceService;

  /** 控件准备好 */
  protected _ready: boolean;

  /** 控件加载完成 */
  @Output() readyChange = new EventEmitter<string>();

  constructor(
    injector: Injector,
  ) {
    super(injector);

    this.cdr = injector.get(ChangeDetectorRef);
    this.sampleDataSourceSer = injector.get(SampleDataSourceService);
  }

  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
    super.ngOnChanges(changes);
    if (changes.args && changes.args.currentValue) {
      if (typeof (changes.args.currentValue) === 'string') {
        this.onArgsChange(JSON.parse(changes.args.currentValue));
      } else {
        this.onArgsChange(changes.args.currentValue);
      }
    }
    if (changes.externalArgs && changes.externalArgs.currentValue) {
      this.onExternalArgsChange(changes.externalArgs.currentValue);
    }
  }

  /** 表示控件已经准备就绪 */
  protected imReady() {
    if (this._ready) {
      return;
    }

    this._ready = true;
    this.readyChange.emit(this.name);
    this.emitValueChange(this.value);
  }

  /** 参数发生更改 */
  abstract onArgsChange(args: any);

  /** 外部组件传递的参数发生过更改 */
  abstract onExternalArgsChange(externalArgs: any);
}

