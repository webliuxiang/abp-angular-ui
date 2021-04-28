import { Component } from '@angular/core';
import { ControlWidget, SFSchema } from '@delon/form';

@Component({
    selector: 'sf-markdown',
    template: `
    <sf-item-wrap
      [id]="id"
      [schema]="schema"
      [ui]="ui"
      [showError]="showError"
      [error]="error"
      [showTitle]="schema.title"
    >
      <sf name="markdown" [schema]="schema" (formSubmit)="_change($event)"></sf>
    </sf-item-wrap>
  `,
    preserveWhitespaces: false,
})
// tslint:disable-next-line: component-class-suffix
export class MarkdownWidget extends ControlWidget {
    static readonly KEY = 'markdown';

    schema: SFSchema = {
        properties: {
            remark: {
                type: 'string',
                ui: {
                    widget: 'md',
                },
            },
        },
    };

    _change(value: string) {
        this.setValue(value);
    }
}
