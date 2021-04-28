import { Component, Injector, OnInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base';
import { CourseSectionDto, CourseSectionServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-create-or-edit-course-section',
  templateUrl: './create-or-edit-course-section.component.html',
  styles: [],
})
export class CreateOrEditCourseSectionComponent extends ModalComponentBase
  implements OnInit {


  /** 章节id */
  id: number;

  /** 课程id */
  courseId: number;

  /** 章节 */
  entity = new CourseSectionDto();

  constructor(
    injector: Injector,
    private courseSectionSer: CourseSectionServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.loading = true;
    if (this.id) {
      this.courseSectionSer.getById(this.id)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe((res) => {
          this.entity = res;
        });
    } else {
      this.courseSectionSer.getSectionCountByCourseId(this.courseId)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe((res) => {
          this.entity.coursesId = res;
        });
    }
  }


  save() {
    this.loading = true;
    this.entity.coursesId = this.courseId;
    this.courseSectionSer.createOrUpdate(this.entity)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.message.success(this.l('操作成功!'));
        this.entity.id = res;
        this.success(this.entity);
      });
  }

}
