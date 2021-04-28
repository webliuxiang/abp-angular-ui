import { Component, OnInit, Input, OnChanges, SimpleChanges, SimpleChange, Injector, NgZone } from '@angular/core';
import {
  CourseCategoryListDto,
  CourseListDto,
  CourseServiceProxy,
  TreeMemberListDto,
} from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base';
import { finalize } from 'rxjs/operators';
import { CourseCategoryServiceProxy } from '../../../../shared/service-proxies/service-proxies';
import { AddCourseInCateComponent } from '../add-course-in-cate/add-course-in-cate.component';
import * as _ from 'lodash';

@Component({
  selector: 'app-coursecate-course-list-panel',
  templateUrl: './coursecate-course-list-panel.component.html',
  styles: [],
})
export class CoursecateCourseListPanelComponent extends PagedListingComponentBase<TreeMemberListDto>
  implements OnInit, OnChanges {
  @Input()
  courseCatedto: CourseCategoryListDto = null;

  constructor(injector: Injector, private _categoryService: CourseCategoryServiceProxy) {
    super(injector);
  }

  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    if (!this.courseCatedto) {
      return;
    }

    this._categoryService
      .getPagedCourseListInCategory(
        this.courseCatedto.id,
        this.filterText,
        request.sorting,
        request.maxResultCount,
        request.skipCount,
      )
      .pipe(finalize(() => finishedCallback()))
      .subscribe(result => {
        this.dataList = result.items;
        this.showPaging(result);
      });
  }

  ngOnInit(): void {
    this.refresh();

    this.courseCateRefreshEvents();
  }

  addCourse(): void {
    this.modalHelper
      .static(AddCourseInCateComponent, {
        categoryId: this.courseCatedto.id,
      })
      .subscribe((res: number[]) => {
        if (res) {
          // this.addMembers(res);
        }
      });
  }

  removeCourse(item: TreeMemberListDto): void {
    const _categoryId = this.courseCatedto.id;
    const ids = [];
    ids.push(item.id);
    this._categoryService
      .removeCourse(ids, _categoryId)
      .pipe()
      .subscribe(result => {
        this.refreshGoFirstPage();
        this.notify.success(this.l('SuccessfullyDeleted'));
      });
  }

  batchDelete(): void {
    const selectCount = this.selectedDataItems.length;
    if (selectCount <= 0) {
      abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));
      return;
    }
    this.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined, res => {
      if (res) {
        // tslint:disable-next-line:radix
        const _categoryId = this.courseCatedto.id;
        const ids = _.map(this.selectedDataItems, 'id');

        this._categoryService
          .removeCourse(ids, _categoryId)
          .pipe()
          .subscribe(result => {
            this.refreshGoFirstPage();
            this.notify.success(this.l('SuccessfullyDeleted'));
          });
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    let change: SimpleChange;
    this.refresh();
  }

  courseCateRefreshEvents() {
    abp.event.on('abp.courseCateRefreshEvents.refresh', () => {
      this.refresh();
    });
  }
}
