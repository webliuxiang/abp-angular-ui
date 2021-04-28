import { Component, EventEmitter, forwardRef, Injector, Input, OnInit, Output, SimpleChange, SimpleChanges } from '@angular/core';
import { ControlComponentBase } from '@shared/component-base';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { CourseClassHourResourcTypeEnum } from '@shared/service-proxies/service-proxies';


/** 课时资源选择组件 */
@Component({
  selector: 'course-class-hour-resource-select',
  templateUrl: './course-class-hour-resource-select.component.html',
  styleUrls: ['./course-class-hour-resource-select.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CourseClassHourResourceSelectComponent),
      multi: true,
    },
  ],
})
export class CourseClassHourResourceSelectComponent extends ControlComponentBase<string> {

  /** 课时资源大分类 */
  resourceTypes = [
    { label: this.l('Video'), value: CourseClassHourResourcTypeEnum.Video },
  ];

  /** 课程资源大类 */
  @Input() resourceType;

  /** 课时资源类型 */
  resourceSource: string;

  /** 课时资源类型列表 */
  resourceSources: any = {};

  /** 课时资源类型列表 */
  resourceSourcesList = [];

  /** 状态更改 */
  @Output() successChange = new EventEmitter<void>();

  /** 资源名称更改 */
  @Output() resourceNameChange = new EventEmitter<string>();

  /** 资源大类发生改变 */
  @Output() resourceTypeChange = new EventEmitter<CourseClassHourResourcTypeEnum>();

  /** 是否存在资源 */
  get noResource(): boolean {
    if (!this.value
      || this.value.trim() === '') {
      return true;
    }
    return false;
  }

  constructor(
    injector: Injector,
  ) {
    super(injector);

    // 初始化资源
    this.initResouceSouce();
  }

  onInit(): void {
  }


  onAfterViewInit(): void {
  }

  onDestroy(): void {
  }


  onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges) {
    if (changes.resourceType) {
      this.onResourceTypeChange(changes.resourceType.currentValue);
    }
  }

  writeValue(obj: any): void {
    this.value = obj;
    // 处理现有数据，因为最早是数值类型字段
    if (obj === '0') {
      this.emitValueChange(undefined);
    }
  }

  /** 资源类型选中修改 */
  onResourceTypeChange(e: any) {
    this.resourceType = e;
    this.resourceTypeChange.emit(e);

    this.resourceSourcesList = this.resourceSources[this.resourceType];
    if (!Array.isArray(this.resourceSourcesList)) {
      this.resourceSourcesList = [];
    }
    if (this.resourceSourcesList.length > 0) {
      this.resourceSource = this.resourceSourcesList[0].value;
    }
  }

  /** 资源来源选中修改 */
  onResourceSourceChange(e: any) {

  }

  /** 选中的视频信息更改 */
  onSelectVideoInfoChange(video: any) {
    this.emitValueChange(video.videoId);
    this.resourceNameChange.emit(video.title);
  }

  /** 重新选择资源 */
  onReSelectResouce(event) {
    this.emitValueChange(undefined);
    this.onResourceTypeChange(CourseClassHourResourcTypeEnum.Video);
    event.preventDefault();
    return false;
  }


  /** 初始化资源来源 */
  private initResouceSouce() {
    if (this.resourceSources.inited) {
      return;
    }
    this.resourceSources.inited = true;
    this.resourceSources[CourseClassHourResourcTypeEnum.Video] = [
      { label: this.l('Upload'), value: 'upload' },
      { label: this.l('VideoLibrary'), value: 'storage' },
    ];
  }

}
