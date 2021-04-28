import { ChangeDetectionStrategy, Component, forwardRef, Injector, OnInit, SimpleChange, SimpleChanges } from '@angular/core';
import { ControlComponentBase, SampleControlComponentBase } from '@shared/component-base';
import { AliyunVodCategoryServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { ArrayService } from '@delon/util';
import { NzTreeNodeOptions } from 'ng-zorro-antd/tree';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-video-category-select',
  templateUrl: './video-category-select.component.html',
  styles: [],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => VideoCategorySelectComponent),
      multi: true,
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class VideoCategorySelectComponent extends ControlComponentBase<number> {

  treeData: NzTreeNodeOptions[] = [];

  constructor(
    injector: Injector,
    private arraySrv: ArrayService,
    private aliyunVodeCategorySer: AliyunVodCategoryServiceProxy,
  ) {
    super(injector);
  }


  onInit(): void {
    this.fetchData();
  }

  onAfterViewInit(): void {
  }

  onDestroy(): void {
  }


  onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {

  }

  writeValue(obj: any): void {
    super.writeValue(obj);
  }

  onTreeModelChange(event: any) {
    this.emitValueChange(event);
  }

  private fetchData() {
    this.aliyunVodeCategorySer
      .getAllVodCategories()
      .pipe(finalize(() => {
      }))
      .subscribe(result => {
        result.filter(o => o.parentId === -1)
          .forEach(item => {
            item.parentId = 0;
          });

        const resultList = this.arraySrv.treeToArr(result, {
          parentMapName: 'parent_id',
          childrenMapName: 'children',
        });
        this.treeData = this.arraySrv.arrToTreeNode(resultList, {
          idMapName: 'cateId',
          titleMapName: 'cateName',
          parentIdMapName: 'parentId',
          cb: (item, parent, deep) => {
          },
        });

        this.cdr.detectChanges();
      });
  }

}
