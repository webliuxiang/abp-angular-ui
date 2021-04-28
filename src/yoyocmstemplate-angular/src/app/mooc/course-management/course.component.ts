import { Component, Injector, OnInit, NgZone } from '@angular/core';
import * as _ from 'lodash';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
// tslint:disable-next-line:max-line-length
import {
  CourseServiceProxy,
  PagedResultDtoOfCourseListDto,
  CourseListDto,
  EntityDtoOfInt64, QueryInput, CourseQueryInput,
} from '@shared/service-proxies/service-proxies';
import { AppConsts } from 'abpPro/AppConsts';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DynamicListViewComponentBase, IFetchData } from '@shared/sample/common';
import { Router } from '@angular/router';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { ReuseTabService } from '@delon/abc';

@Component({
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.less'],
  animations: [appModuleAnimation()],
})
export class CourseComponent extends DynamicListViewComponentBase<CourseListDto> implements OnInit {
  portalBaseUrl = AppConsts.portalBaseUrl;

  constructor(
    injector: Injector,
    private router: Router,
    private reuseTabSer: ReuseTabService,
    private courseSer: CourseServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    super.ngOnInit();

    this.pageName = 'mooc.course';
    this.reuseTabSer.title = this.l('课程');
    setTimeout(() => {
      this.reuseTabSer.refresh();
    }, 300);
  }

  fetchData(arg: IFetchData) {
    const input = new CourseQueryInput();
    input.maxResultCount = arg.pageSize;
    input.skipCount = arg.skipCount;
    input.queryConditions = arg.queryConditions;
    input.sortConditions = arg.sortConditions;


    this.courseSer.getPaged(input)
      .pipe(finalize(() => {
        if (arg.finishedCallback) {
          arg.finishedCallback();
        }
      }))
      .subscribe((res) => {
        if (res && Array.isArray(res.items)) {
          for (let i = 0; i < res.items.length; i++) {
            const item = res.items[i];
            item.imgUrl = UrlHelper.processImgUrl(item.imgUrl);
          }
        }

        arg.successCallback(res);
      });
  }

  /** 创建 */
  create() {
    setTimeout(() => {
      this.router.navigate(['/app/mooc/course-edit']);
    }, 500);
  }

  /** 编辑 */
  edit(input: CourseListDto) {
    setTimeout(() => {
      // this.router.navigate(['/app/mooc/course-edit', { id: input.id }], { skipLocationChange: true });
      this.router.navigate(['/app/mooc/course-edit', { id: input.id }]);
    }, 500);
  }

  /** 删除 */
  delete(input: CourseListDto) {
    this.message.confirm(this.l('确认删除？'), undefined, (res) => {
      if (!res) {
        return;
      }
      this.loading = true;

      this.courseSer.delete(input.id)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(() => {
          this.notify.success(this.l('删除成功!'));
        });
    });
  }

}
