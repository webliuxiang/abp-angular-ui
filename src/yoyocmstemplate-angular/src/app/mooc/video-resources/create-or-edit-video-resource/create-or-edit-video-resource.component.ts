import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
  CreateOrUpdateVideoResourceInput,
  VideoResourceEditDto,
  VideoResourceServiceProxy,
  AliyunVodCategoryServiceProxy,
  VodCategoryEditDto,
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-video-resource',
  templateUrl: './create-or-edit-video-resource.component.html',
  styleUrls: ['create-or-edit-video-resource.component.less'],
})
export class CreateOrEditVideoResourceComponent extends ModalComponentBase implements OnInit {
  /**
   * 编辑时DTO的id
   */
  id: any;

  entity: VideoResourceEditDto = new VideoResourceEditDto();
  vodCateList = [];

  /**
   * 初始化的构造函数
   */
  constructor(
    injector: Injector,
    private _videoResourceService: VideoResourceServiceProxy,
    private _aliyunVodeCategoryservice: AliyunVodCategoryServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.init();
  }

  /**
   * 初始化方法
   */
  init(): void {
    this._videoResourceService.getForEdit(this.id).subscribe(result => {
      this.entity = result.videoResource;
    });

    this.getVodCategoryDataList();
  }

  /**
   * 保存方法,提交form表单
   */
  submitForm(): void {
    const input = new CreateOrUpdateVideoResourceInput();
    input.videoResource = new VideoResourceEditDto(this.entity);

    this.saving = true;

    this._videoResourceService
      .createOrUpdate(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
      });
  }

  // 分类功能

  getVodCategoryDataList(): void {
    this._aliyunVodeCategoryservice
      .getAllVodCategories()
      .pipe()
      .subscribe(result => {
        this.vodCateList = this.processTree<VodCategoryEditDto, any>(result, 'children', (item, hasChildren) => {
          return {
            ...item,
            key: item.cateId,
            title: item.cateName,
            isLeft: !hasChildren,
          };
        });

        // this.vodCateList = result;

        // result.map(a => a.cateId);

        // console.log(this.vodCateList);
      });
  }

  processTree<TIn, TOut>(
    sourceData: TIn[],
    childrenMapKey: string,
    mapFunc: (source: TIn, hasChildren: boolean) => TOut,
  ): TOut[] {
    if (!Array.isArray(sourceData)) {
      return undefined;
    }

    const result: TOut[] = [];

    for (let i = 0; i < sourceData.length; i++) {
      const item = sourceData[i];

      const oldChildren = item[childrenMapKey];
      const hasChildren = Array.isArray(oldChildren) && oldChildren.length > 0;

      const newObj = mapFunc(item, hasChildren);

      if (hasChildren) {
        const newChildren = this.processTree(oldChildren, childrenMapKey, mapFunc);
        newObj[childrenMapKey] = newChildren;
      }

      result.push(newObj);
    }

    return result;
  }
}
