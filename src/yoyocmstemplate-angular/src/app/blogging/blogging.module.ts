import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BloggingRoutingModule } from './blogging-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { AbpModule } from 'abp-ng2-module';
import { CustomNgZorroModule } from '@shared/ng-zorro';
import { CreateOrEditBlogComponent } from './blogs/create-or-edit-blog/create-or-edit-blog.component';
import { BlogComponent } from './blogs/blog.component';
import { PostComponent } from './posts/post.component';
import { CreateOrEditPostComponent } from './posts/create-or-edit-post/create-or-edit-post.component';
import { CommentComponent } from './comments/comment.component';
import { CreateOrEditCommentComponent } from './comments/create-or-edit-comment/create-or-edit-comment.component';
import { TagComponent } from './tagging/tag.component';
import { CreateOrEditTagComponent } from './tagging/create-or-edit-tag/create-or-edit-tag.component';

@NgModule({
  imports: [CommonModule, HttpClientModule, SharedModule, AbpModule, CustomNgZorroModule, BloggingRoutingModule],
  declarations: [
    BlogComponent,
    CreateOrEditBlogComponent,
    PostComponent,
    CreateOrEditPostComponent,
    CommentComponent,
    CreateOrEditCommentComponent,
    TagComponent,
    CreateOrEditTagComponent,
  ],
  entryComponents: [
    CreateOrEditBlogComponent,
    CreateOrEditPostComponent,
    CreateOrEditCommentComponent,
    CreateOrEditTagComponent,
  ],
})
export class BloggingModule { }
