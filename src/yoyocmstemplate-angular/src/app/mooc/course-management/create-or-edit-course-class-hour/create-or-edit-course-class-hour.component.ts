import { Component, Injector, OnInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base';
import { CourseClassHourServiceProxy, CourseClassHourDto, CourseClassHourResourcTypeEnum } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-create-or-edit-course-class-hour',
  templateUrl: './create-or-edit-course-class-hour.component.html',
  styles: [],
})
export class CreateOrEditCourseClassHourComponent extends ModalComponentBase
  implements OnInit {

  /** 课时id */
  id: number;

  /** 章节id */
  courseSectionId: number;

  /** 课时  */
  entity = new CourseClassHourDto();

  constructor(
    injector: Injector,
    private courseClassHourSer: CourseClassHourServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.entity.resourceType = CourseClassHourResourcTypeEnum.Video;
    this.entity.resourceId = undefined;
    this.loading = true;
    if (this.id) {
      this.courseClassHourSer.getById(this.id)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe((res) => {
          this.entity = res;
        });
    } else {
      this.courseClassHourSer.getClassHourCountBySectionId(this.courseSectionId)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe((res) => {
          this.entity.sortNumber = res + 1;
        });
    }
  }

  /** 视频分类发生改变 */
  onVideoCategoryChange(event: number) {
  }

  save() {
    this.loading = true;
    this.entity.courseSectionId = this.courseSectionId;
    this.courseClassHourSer.createOrUpdate(this.entity)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.notify.success(this.l('操作成功!'));
        this.entity.id = res;
        this.success(this.entity);
      });
  }

  /** 资源名称发生更改 */
  onResourceNameChange(e: string) {
    if (!this.entity.name
      || this.entity.name.trim() === '') {
      this.entity.name = e.substring(0, e.lastIndexOf('.'));
    }
  }

}
