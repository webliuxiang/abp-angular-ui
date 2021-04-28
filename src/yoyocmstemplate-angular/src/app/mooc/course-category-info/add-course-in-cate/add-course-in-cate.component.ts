import {
  CourseCategoryServiceProxy,
  FindCoursesInput,
  AddCourseToCategoryInput,
} from './../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Injector } from '@angular/core';
import { ModalPagedListingComponentBase, PagedRequestDto } from '@shared/component-base';
import { NameValueDto } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-add-course-in-cate',
  templateUrl: './add-course-in-cate.component.html',
  styles: [],
})
export class AddCourseInCateComponent extends ModalPagedListingComponentBase<NameValueDto> {
  // 分类Id
  categoryId: number;
  /**
   * 构造函数
   * @param injector 注入器
   * @param _categoryService 课程分类服务
   */
  constructor(injector: Injector, private _categoryService: CourseCategoryServiceProxy) {
    super(injector);
  }

  protected getDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    const input = new FindCoursesInput();
    input.categoryId = this.categoryId;
    input.filterText = this.filterText;
    input.skipCount = request.skipCount;
    input.maxResultCount = request.maxResultCount;

    this._categoryService
      .findCourses(input)
      .pipe(finalize(() => finishedCallback()))

      .subscribe(result => {
        this.dataList = result.items;

        this.showPaging(result);
      });
  }

  addCourseToCategory(): void {
    const selectCount = this.selectedDataItems.length;
    if (selectCount <= 0) {
      abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));
      return;
    }
    this.saving = true;
    const input = new AddCourseToCategoryInput();
    input.courseCategoryId = this.categoryId;
    input.courseIds = _.map(this.selectedDataItems, selectedMember => Number(selectedMember.value));

    this._categoryService
      .addCourse(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        abp.event.trigger('abp.courseCateRefreshEvents.refresh');

        this.success(input.courseIds);
      });
  }
}
