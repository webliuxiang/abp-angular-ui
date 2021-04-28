import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OtherRoutingModule } from './other-routing.module';
import { ChatComponent } from './chat/chat.component';
import { SharedModule } from '@shared/shared.module';
import { AbpModule } from 'abp-ng2-module';
import { ProjectsComponent } from './projects/projects.component';
import { CreateOrEditProjectComponent } from './projects/create-or-edit-project/create-or-edit-project.component';

@NgModule({
  imports: [
    SharedModule,
    AbpModule,
    CommonModule,
    OtherRoutingModule,
  ],
  declarations: [
    ProjectsComponent,
    ChatComponent,
    CreateOrEditProjectComponent,
  ],
})
export class OtherModule {
}
