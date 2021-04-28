import { Component, OnInit, Output, EventEmitter, Injector, Input } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';

@Component({
  selector: 'table-checkbox-panel',
  templateUrl: './table-checkbox-panel.component.html',
  styles: [],
})
export class TableCheckboxPanelComponent extends AppComponentBase implements OnInit {
  @Output()
  onClear = new EventEmitter<void>();

  @Output()
  onRefresh = new EventEmitter<void>();

  @Input()
  selectLength = 0;

  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit() {}

  onClearClick() {
    this.onClear.emit();
  }
  // 触发刷新方法，调用绑定的onRefresh事件
  onRefreshClick() {
    this.onRefresh.emit();
  }
}
