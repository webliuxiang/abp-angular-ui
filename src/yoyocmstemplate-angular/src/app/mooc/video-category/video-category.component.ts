import { AliyunVodCategoryServiceProxy, VodCategoryEditDto } from './../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Injector, TemplateRef } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/component-base';
import { finalize } from 'rxjs/operators';
import { ArrayService } from '@delon/util';
import { NzTreeNode, NzTreeNodeOptions, NzFormatEmitEvent } from 'ng-zorro-antd/tree';
import { NzDropdownMenuComponent, NzContextMenuService } from 'ng-zorro-antd/dropdown';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'app-video-category',
  templateUrl: './video-category.component.html',
  styleUrls: ['./video-category.component.less'],
  animations: [appModuleAnimation()],
})
export class VideoCategoryComponent extends AppComponentBase implements OnInit {
  constructor(
    injector: Injector,
    private _aliyunVodeCategoryservice: AliyunVodCategoryServiceProxy,
    private arrSrv: ArrayService,
    private _nzContextMenuService: NzContextMenuService,
  ) {
    super(injector);
  }

  private menuEvent: NzFormatEmitEvent;
  data: NzTreeNodeOptions[] = [];
  item: VodCategoryEditDto;
  op: string;
  delDisabled = false;

  schema: SFSchema = {
    properties: {
      cateName: { type: 'string', title: '名称' },
    },
    required: ['cateName'],

    ui: { grid: { md: 24, lg: 12 }, spanLabelFixed: 100 },
  };

  ngOnInit() {
    this.fetchData();
  }

  // 获取树形数据
  fetchData(): void {
    this._aliyunVodeCategoryservice
      .getAllVodCategories()
      .pipe(finalize(() => {
      }))
      .subscribe(result => {
        result
          .filter(o => o.parentId === -1)
          .forEach(item => {
            item.parentId = 0;
          });

        const resultList = this.arrSrv.treeToArr(result, {
          parentMapName: 'parent_id',
          childrenMapName: 'children',
        });
        //  debugger;

        this.data = this.arrSrv.arrToTreeNode(resultList, {
          idMapName: 'cateId',
          titleMapName: 'cateName',
          parentIdMapName: 'parentId',
          cb: (item, parent, deep) => {
          },
        });
        console.log(result);
      });
  }

  add(item: any) {
    this.op = 'edit';
    this.closeContextMenu();

    this.item = new VodCategoryEditDto();

    if (item != null) {
      this.item.parentId = item.cateId;
    }
    // console.log(this.item);
  }

  del() {
    this.closeContextMenu();
    this._aliyunVodeCategoryservice.deleteVodCategory(this.item.cateId).subscribe(() => {
      this.fetchData();
      this.op = '';
    });
  }

  edit() {
    this.closeContextMenu();
    this.op = 'edit';
  }

  save(item: VodCategoryEditDto) {
    if (item.cateId > 0) {
      this._aliyunVodeCategoryservice
        .updateVodCategory(item)
        .pipe()
        .subscribe(result => {
          this.fetchData();
          this.op = '';
        });
    } else {
      this._aliyunVodeCategoryservice
        .createVodCategory(item)
        .pipe()
        .subscribe(result => {
          this.fetchData();
          this.op = '';
        });
    }

    //  console.log(item);
  }

  show(e: NzFormatEmitEvent) {
    this.op = e.node.isSelected ? 'view' : '';
    this.item = e.node.origin as any;
    this.menuEvent = e;
  }

  get delMsg(): string {
    const childrenLen = this.menuEvent.node.children.length;
    if (childrenLen === 0) {
      return `确认删除【${this.menuEvent.node.title}】吗？`;
    }
    return `确认删除【${this.menuEvent.node.title}】以及所有子分类吗？`;
  }

  showContextMenu($event: any, template: any, node?: NzTreeNode) {
    // this.menuEvent = e;
    //  this.delDisabled = e.node.children.length !== 0;
    this._nzContextMenuService.create($event, template);
  }

  closeContextMenu() {
    this._nzContextMenuService.close();
  }

  // processTree(sourceData: any[], keyMap: string, titleMap: string, parentIdMap: string, childrenMap: string): any[] {

  // }
}
