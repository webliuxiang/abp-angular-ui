import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  AliyunVodCategoryServiceProxy,
  ComboboxItemDtoTOfString,
  CommonLookupServiceProxy,
  ListResultDtoOfComboboxItemDtoTOfString, VodCategoryEditDto,
} from '@shared/service-proxies/service-proxies';

@Injectable()
export class SampleDataSourceService {

  constructor(
    private commonSer: CommonLookupServiceProxy,
    private aliyunVodeCategorySer: AliyunVodCategoryServiceProxy,
  ) {

  }


  /** 根据数据源名称和参数返回对应的数据 */
  fetchData<TData>(dataSourceName: string, args?: any): Observable<TData> {
    switch (dataSourceName) {
      case 'yesOrNo':
        return this.yesOrNo();
      case 'enum':
        return this.enumCombobox(args.enumType) as Observable<TData>;
      case 'video-category':
        return this.videoCategory() as Observable<TData>;
    }

    return undefined;
  }


  /** 是否 */
  private yesOrNo(): Observable<any> {
    return new Observable<any>((obs) => {
      obs.next([
        { label: 'label.yes', value: true },
        { label: 'label.no', value: false },
      ]);
      obs.complete();
    });
  }

  /** 枚举 */
  private enumCombobox(enumType: string): Observable<any> {
    return new Observable<any>((obs) => {
      this.commonSer.getEnumForCombobox(enumType)
        .subscribe((res) => {
          if (!res.items) {
            obs.next([]);
          } else {
            obs.next(res.items.map(o => {
              return {
                label: o.displayText,
                value: o.value,
              };
            }));
          }
          obs.complete();
        });
    });
  }


  /** 视频分类 */
  private videoCategory(): Observable<any> {
    return new Observable<any>((obs) => {
      this.aliyunVodeCategorySer
        .getAllVodCategories()
        .subscribe(result => {
          const vodCateList = this.processVideoCategoryTree<VodCategoryEditDto, any>(result, 'children', (item, hasChildren) => {
            return {
              ...item,
              key: item.cateId,
              title: item.cateName,
              isLeft: !hasChildren,
            };
          });

          obs.next(vodCateList);
          obs.complete();
        });
    });
  }

  /** 转换视频资源树 */
  private processVideoCategoryTree<TIn, TOut>(
    sourceData: TIn[],
    childrenMapKey: string,
    mapFunc: (source: TIn, hasChildren: boolean) => TOut,
  ): TOut[] {
    if (!Array.isArray(sourceData)) {
      return undefined;
    }

    const result: TOut[] = [];

    for (const item of sourceData) {

      const oldChildren = item[childrenMapKey];
      const hasChildren = Array.isArray(oldChildren) && oldChildren.length > 0;

      const newObj = mapFunc(item, hasChildren);

      if (hasChildren) {
        const newChildren = this.processVideoCategoryTree(oldChildren, childrenMapKey, mapFunc);
        newObj[childrenMapKey] = newChildren;
      }

      result.push(newObj);
    }

    return result;
  }

}
