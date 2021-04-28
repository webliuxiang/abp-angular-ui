import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ChatComponent } from './chat/chat.component';
import { ProjectsComponent } from './projects/projects.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'chat', component: ChatComponent },
      { path: 'project', component: ProjectsComponent }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OtherRoutingModule { }
