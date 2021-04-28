import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CourseComponent } from './course-management/course.component';
import { VideoResourceComponent } from './video-resources/video-resource.component';
import { CreateOrEditCourseComponent } from './course-management/create-or-edit-course/create-or-edit-course.component';
import { VideoCategoryComponent } from './video-category/video-category.component';
import { CourseCategoryComponent } from './course-category-info/course-category.component';

const routes: Routes = [

  {
    path: 'course',
    component: CourseComponent,
    data: { permission: 'Pages.CourseManage' },
  },
  { path: 'course-edit', component: CreateOrEditCourseComponent },
  { path: 'coursecategoryinfo', component: CourseCategoryComponent, data: { permission: 'Pages.CourseCategory' } },

  {
    path: 'video-resource',
    component: VideoResourceComponent,
    data: { permission: 'Pages.VideoResource' },
  },
  {
    path: 'video-category',
    component: VideoCategoryComponent,
    data: { permission: '' },
  },

  {
    path: '**',
    redirectTo: 'course',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MoocRoutingModule { }
