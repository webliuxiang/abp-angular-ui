import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { AbpModule } from 'abp-ng2-module';

import { CourseComponent } from './course-management/course.component';

import { VideoResourceComponent } from './video-resources/video-resource.component';
// tslint:disable-next-line:max-line-length
import { MoocRoutingModule } from './mooc-routing.module';
import { VideoCategoryComponent } from './video-category/video-category.component';
import { UploadVideoResourceComponent } from './video-resources/upload-video-resource/upload-video-resource.component';
import { CourseCategoryComponent } from './course-category-info/course-category.component';
import { AddCourseInCateComponent } from './course-category-info/add-course-in-cate/add-course-in-cate.component';
import { SimplemdeModule } from 'ngx-simplemde';
import { SampleComponentsModule } from '@shared/sample/components';
import { CoursecateCourseListPanelComponent } from './course-category-info/coursecate-course-list-panel';
import { CourseSectionsPanelComponent } from './course-management/course-sections-panel';

import { CreateOrEditCourseCategoryComponent } from './course-category-info/create-or-edit-course-category';
import { CreateOrEditCourseComponent } from './course-management/create-or-edit-course';
import { CreateOrEditCourseClassHourComponent } from './course-management/create-or-edit-course-class-hour';
import { CreateOrEditCourseSectionComponent } from './course-management/create-or-edit-course-section';
import { VideoCategorySelectComponent } from './video-category-select';
import { UploadVideoResoucePanelComponent } from './video-resources/upload-video-resouce-panel';
import { CourseClassHourResourceSelectComponent } from './course-management/course-class-hour-resource-select';
import { CourseClassHourResourceUploadPanelComponent } from './course-management/course-class-hour-resource-upload-panel';
import { CourseClassHourResourceSelectPanelComponent } from './course-management/course-class-hour-resource-select-panel';
import { DragDropModule } from '@angular/cdk/drag-drop';


const ENTRY_COMPONENTS = [

  UploadVideoResourceComponent,
  AddCourseInCateComponent,
  CoursecateCourseListPanelComponent,
  //

  CreateOrEditCourseCategoryComponent,
  CreateOrEditCourseComponent,
  CreateOrEditCourseSectionComponent,
  CreateOrEditCourseClassHourComponent,
];

const COMPONENTS = [

  VideoCategoryComponent,
  CourseComponent,
  VideoResourceComponent,
  UploadVideoResourceComponent,
  CourseCategoryComponent,
  AddCourseInCateComponent,
  CoursecateCourseListPanelComponent,
  CourseSectionsPanelComponent,
  VideoCategorySelectComponent,
  UploadVideoResoucePanelComponent,
  CourseClassHourResourceSelectComponent,
  CourseClassHourResourceUploadPanelComponent,
  CourseClassHourResourceSelectPanelComponent,
  ...ENTRY_COMPONENTS,
];


@NgModule({
  imports: [
    CommonModule,
    MoocRoutingModule,
    HttpClientModule,
    SharedModule,
    AbpModule,
    SampleComponentsModule,
    DragDropModule
  ],
  declarations: [
    ...COMPONENTS,
  ],
  entryComponents: [
    ...ENTRY_COMPONENTS,
  ],
  providers: [],
})
export class MoocModule {
}
