import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { BlogComponent } from './blogs/blog.component';
import { PostComponent } from './posts/post.component';
import { CommentComponent } from './comments/comment.component';
import { TagComponent } from './tagging/tag.component';
import { CreateOrEditPostComponent } from './posts/create-or-edit-post/create-or-edit-post.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'blogs', component: BlogComponent, data: { permission: 'Pages.Blog' } },
      { path: 'posts', component: PostComponent, data: { permission: 'Pages.Post' } },
      { path: 'create-or-edit-post', component: CreateOrEditPostComponent },
      { path: 'comments', component: CommentComponent, data: { permission: 'Pages.Comment' } },
      { path: 'tagging', component: TagComponent, data: { permission: 'Pages.Tag' } },

      {
        path: '**',
        redirectTo: 'blogs',
      },
    ],
  },
];

// { path: '', component: BloggingComponent }
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BloggingRoutingModule {}
