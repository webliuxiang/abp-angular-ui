import { Component } from '@angular/core';
import { ControlWidget } from '@delon/form';

@Component({
    selector: 'sf-editor',
    template: `
    <sf-item-wrap
      [id]="id"
      [schema]="schema"
      [ui]="ui"
      [showError]="showError"
      [error]="error"
      [showTitle]="schema.title"
    >
      <simplemde
        [ngModel]="value"
        (ngModelChange)="_change($event)"
        [options]="ui.options"
        [delay]="ui.delay || 300"
      ></simplemde>
    </sf-item-wrap>
  `,
    preserveWhitespaces: false,
})
// tslint:disable-next-line: component-class-suffix
export class SimplemdeWidget extends ControlWidget {
    static readonly KEY = 'md';

    _change(value: string) {
        this.setValue(value);
        if (this.ui.contentChanged) { this.ui.contentChanged(value); }
    }
}
