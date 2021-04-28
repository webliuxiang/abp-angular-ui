import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DemoUiComponent } from './demo-ui/demoui.component';

const routes: Routes = [{ path: 'demoui', component: DemoUiComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DemoManagementRoutingModule {}
