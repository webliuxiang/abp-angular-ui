import { Component, Injector, Input, OnChanges, OnInit, SimpleChange, SimpleChanges } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import {
  CourseClassHourDto,
  CourseClassHourServiceProxy,
  CourseSectionDto,
  CourseSectionServiceProxy,
  CourseSectionExchangeIndexDto, CourseClassHourExchangeIndexDto, MoveCourseClassHourSectionDto,
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { CreateOrEditCourseSectionComponent } from '../create-or-edit-course-section';
import { CreateOrEditCourseClassHourComponent } from '../create-or-edit-course-class-hour';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-course-sections-panel',
  templateUrl: './course-sections-panel.component.html',
  styles: [],
})
export class CourseSectionsPanelComponent extends AppComponentBase
  implements OnInit, OnChanges {

  /** 加载效果 */
  loading = false;
  /** 启用 */
  enabled: boolean;
  /** 章节集合 */
  sectionList: CourseSectionDto[] = [];
  /** 章节课时 映射 */
  courseClassHourMap: { [P in keyof string]?: CourseClassHourDto[] } = {};
  /** 章节总数 */
  courseSectionCount: number;
  /** 课时总数 */
  courseClassHourCount: number;
  /** 课程id */
  @Input() courseId: number;

  /** 修改课时所在章节面板是否显示 */
  changeClassHourSectionVisibleMap: { [P in keyof string]?: boolean } = {};

  constructor(
    injector: Injector,
    private _courseSectionSer: CourseSectionServiceProxy,
    private _courseClassHourSer: CourseClassHourServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
  }

  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
    if (changes.courseId && changes.courseId.currentValue > 0) {
      this.fetchCourseSectionsByCourseId(changes.courseId.currentValue);
      this.enabled = true;
    }
  }

  /** 获取章节对应的课时集合 */
  getCourseClassHours(courseId: number): CourseClassHourDto[] {
    return this.courseClassHourMap[courseId];
  }

  /** 创建或编辑章节 */
  createOrEditSection(courseSectio?: CourseSectionDto) {
    this.modalHelper.createStatic(CreateOrEditCourseSectionComponent,
      {
        id: courseSectio ? courseSectio.id : undefined, // 章节id
        courseId: this.courseId, // 课程id
      })
      .subscribe((res) => {
        if (res) {
          this.fetchCourseSectionById(res.id);
        }
      });
  }

  /** 删除章节 */
  deleteSection(courseSection: CourseSectionDto) {
    this.loading = true;
    this._courseSectionSer.delete(courseSection.id)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(() => {
        this.message.success(this.l('操作成功'));
        this.sectionList = this.sectionList.filter(o => o.id !== courseSection.id)
          .sort((a, b) => {
            return a.index - b.index;
          });
        // 移除课时
        delete this.courseClassHourMap[courseSection.id];
        this.processSectionAndClassHourStatistics();
      });
  }


  /** 移动章节 */
  moveSection(index: number, up = true) {
    let a: CourseSectionDto;
    let b: CourseSectionDto;
    if (up) {
      a = this.sectionList[index - 1];
      b = this.sectionList[index];
    } else {
      a = this.sectionList[index];
      b = this.sectionList[index + 1];
    }

    const input = new CourseSectionExchangeIndexDto();
    input.aId = a.id;
    input.bId = b.id;
    this._courseSectionSer.exchangeIndex(input)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(() => {
        const aIndex = a.index;
        a.index = b.index;
        b.index = aIndex;

        this.sectionList.sort((a, b) => {
          return a.index - b.index;
        });
        this.sectionList = [
          ...this.sectionList,
        ];
      });
  }


  /** 添加或编辑课时 */
  createOrEditClassHour(courseSection: CourseSectionDto, courseClassHour?: CourseClassHourDto) {
    this.modalHelper.createStatic(CreateOrEditCourseClassHourComponent,
      {
        id: courseClassHour ? courseClassHour.id : undefined, // 课时id
        courseSectionId: courseSection.id, // 章节id
      })
      .subscribe((res) => {
        if (res) {
          this.fetchCourseClassHourById(res.id);
        }
      });
  }

  /** 删除课时 */
  deleteClassHour(classHour?: CourseClassHourDto) {
    this.loading = true;
    this._courseClassHourSer.delete(classHour.id)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(() => {
        this.message.success(this.l('操作成功'));
        const array = this.courseClassHourMap[classHour.courseSectionId];
        this.courseClassHourMap[classHour.courseSectionId] = array
          .filter(o => o.id !== classHour.id)
          .sort((a, b) => {
            return a.sortNumber - b.sortNumber;
          });
        this.processSectionAndClassHourStatistics();
      });
  }

  /** 移动章节 */
  moveClassHour(courseId: number, index: number, up = true) {
    let a: CourseClassHourDto;
    let b: CourseClassHourDto;
    if (up) {
      a = this.courseClassHourMap[courseId][index - 1];
      b = this.courseClassHourMap[courseId][index];
    } else {
      a = this.courseClassHourMap[courseId][index];
      b = this.courseClassHourMap[courseId][index + 1];
    }

    const input = new CourseClassHourExchangeIndexDto();
    input.aId = a.id;
    input.bId = b.id;
    this._courseClassHourSer.exchangeSortNum(input)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(() => {
        const aIndex = a.sortNumber;
        a.sortNumber = b.sortNumber;
        b.sortNumber = aIndex;

        this.courseClassHourMap[courseId].sort((a, b) => {
          return a.sortNumber - b.sortNumber;
        });
        this.courseClassHourMap[courseId] = [
          ...this.courseClassHourMap[courseId],
        ];
      });
  }

  /** 设置课时所属章节 */
  setClassHourSection(classHour: CourseClassHourDto, sectionId: number) {
    this.loading = true;
    const input = new MoveCourseClassHourSectionDto();
    input.id = classHour.id;
    input.courseSectionId = sectionId;
    this._courseClassHourSer.moveCourseClassHourSection(input)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(() => {
        this.changeClassHourSectionVisibleMap[classHour.id] = false;
        this.fetchCourseClassHourBySectionIdList(
          this.sectionList.map(o => o.id),
        );
      });
  }

  /** 查看课时 */
  viewClassHour(classHour: CourseClassHourDto) {

  }

  /** 根据课程id获取章节集合 */
  private fetchCourseSectionsByCourseId(courseId: number) {
    this.courseSectionCount = 0;
    this.loading = true;
    this._courseSectionSer.getSectionsByCourseId(courseId)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        if (!res || !res.items) {
          this.sectionList = [];
          return;
        }
        this.sectionList = res.items;
        this.courseSectionCount = this.sectionList.length;
        this.fetchCourseClassHourBySectionIdList(
          this.sectionList.map(o => o.id),
        );
      });
  }

  /** 根据章节id获取章节 */
  private fetchCourseSectionById(sectionId: number) {
    this.loading = true;
    this._courseSectionSer.getById(sectionId)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        const newArray = [];
        const index = this.sectionList.findIndex(o => o.id === res.id);
        if (index === -1) {
          newArray.push(...this.sectionList);
          newArray.push(res);
        } else {
          this.sectionList[index] = res;
          newArray.push(...this.sectionList);
        }

        this.sectionList = newArray.sort((a, b) => {
          return a.index - b.index;
        });
        if (index === -1) {
          this.processSectionAndClassHourStatistics();
        }
      });
  }

  /** 获取课程所有章节的所有课时数据 */
  private fetchCourseClassHourBySectionIdList(sectionIdList: number[]) {
    if (!sectionIdList || sectionIdList.length === 0) {
      this.courseClassHourCount = 0;
      this.courseClassHourMap = {};
      return;
    }

    this.loading = true;
    this._courseClassHourSer.getBySectionIdList(sectionIdList)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        if (!res || !res.items) {
          this.courseClassHourCount = 0;
          this.courseClassHourMap = {};
          return;
        }

        for (const classHour of res.items) {
          this.changeClassHourSectionVisibleMap[classHour.id] = false;
        }

        const courseClassHourList = res.items;
        this.courseClassHourCount = courseClassHourList.length;
        for (let i = 0; i < sectionIdList.length; i++) {
          const courseSectionId = sectionIdList[i];
          this.courseClassHourMap[courseSectionId] = courseClassHourList
            .filter(o => o.courseSectionId === courseSectionId);
        }
      });
  }

  /** 加载课时数据 */
  private fetchCourseClassHourById(classHourId: number) {
    this.loading = true;
    this._courseClassHourSer.getById(classHourId)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        const currentArray = this.courseClassHourMap[res.courseSectionId];
        if (!Array.isArray(currentArray)) {
          this.courseClassHourMap[res.courseSectionId] = [
            res,
          ];
          this.processSectionAndClassHourStatistics();
          return;
        }

        const isNew = !currentArray.find(o => o.id === res.id);
        const newArray = [];
        if (!isNew) {
          for (const item of currentArray) {
            if (item.id === res.id) {
              newArray.push(res);
            } else {
              newArray.push(item);
            }
          }
        } else {
          this.changeClassHourSectionVisibleMap[res.id] = false;
          newArray.push(...currentArray);
          newArray.push(res);
        }

        for (const classHour of newArray) {
          this.changeClassHourSectionVisibleMap[classHour.id] = false;
        }

        this.courseClassHourMap[res.courseSectionId] = newArray.sort((a, b) => {
          return a.sortNumber - b.sortNumber;
        });
        if (isNew) {
          this.processSectionAndClassHourStatistics();
        }
      });
  }


  /** 重新计算章节和课时数量 */
  private processSectionAndClassHourStatistics() {
    this.courseSectionCount = 0;
    this.courseClassHourCount = 0;
    this.courseSectionCount = this.sectionList.length;
    // tslint:disable-next-line:forin
    for (const key in this.courseClassHourMap) {
      this.courseClassHourCount += this.courseClassHourMap[key].length;
    }
  }

  /** 拖动面板 */
  drop(event: CdkDragDrop<string[]>, courseId: number) {
    const data = this.getCourseClassHours(courseId);
    moveItemInArray(data, event.previousIndex, event.currentIndex);



  }
}
