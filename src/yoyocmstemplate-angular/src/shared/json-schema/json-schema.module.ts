import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { DelonFormModule, WidgetRegistry } from '@delon/form';
import { AddressWidget } from './widgets/address/address.widget';
import { ImgWidget } from './widgets/img/img.widget';
import { SimplemdeWidget } from './widgets/simplemde/simplemde.widget';
import { MarkdownWidget } from './widgets/markdown/markdown.widget';

export const SCHEMA_THIRDS_COMPONENTS = [
  ImgWidget,
  AddressWidget,
  SimplemdeWidget,
  MarkdownWidget
];

@NgModule({
  declarations: SCHEMA_THIRDS_COMPONENTS,
  entryComponents: SCHEMA_THIRDS_COMPONENTS,
  imports: [SharedModule, DelonFormModule.forRoot()],
  exports: [...SCHEMA_THIRDS_COMPONENTS],
})
export class JsonSchemaModule {
  constructor(widgetRegistry: WidgetRegistry) {
    widgetRegistry.register(ImgWidget.KEY, ImgWidget);
    widgetRegistry.register(AddressWidget.KEY, AddressWidget);
    widgetRegistry.register(SimplemdeWidget.KEY, SimplemdeWidget);
    widgetRegistry.register(MarkdownWidget.KEY, MarkdownWidget);
  }
}
