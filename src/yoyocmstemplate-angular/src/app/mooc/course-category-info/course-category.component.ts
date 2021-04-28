import { Component, Injector, OnInit, TemplateRef, EventEmitter, Output } from '@angular/core';
import * as _ from 'lodash';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  CourseCategoryServiceProxy,
  PagedResultDtoOfCourseCategoryListDto,
  CourseCategoryListDto,
  ListResultDtoOfCourseCategoryListDto,
  MoveCourseCategoryInput,
} from '@shared/service-proxies/service-proxies';
import { CreateOrEditCourseCategoryComponent } from './create-or-edit-course-category/create-or-edit-course-category.component';
import { AppConsts } from 'abpPro/AppConsts';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { finalize, map } from 'rxjs/operators';
import { NzTreeNode, NzFormatEmitEvent, NzFormatBeforeDropEvent } from 'ng-zorro-antd/tree';
import { NzDropdownMenuComponent, NzContextMenuService } from 'ng-zorro-antd/dropdown';
import { ArrayService } from '@delon/util';
import { Observable, of } from 'rxjs';

@Component({
  templateUrl: './course-category.component.html',
  styleUrls: ['./course-category.component.less'],
  animations: [appModuleAnimation()],
})
export class CourseCategoryComponent extends PagedListingComponentBase<CourseCategoryListDto> implements OnInit {
  constructor(
    injector: Injector,
    private _courseCategoryService: CourseCategoryServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private arrSrv: ArrayService,
    private nzContextMenuService: NzContextMenuService,
  ) {
    super(injector);
  }

  categoriesdata: NzTreeNode[] = [];
  item: CourseCategoryListDto;
  delDisabled = false;

  private menuEvent: NzFormatEmitEvent;
  @Output()
  selectedChange = new EventEmitter<NzTreeNode>();

  /**
   * 获取后端数据列表信息
   * @param request 请求的数据的dto 请求必需参数 skipCount: number; maxResultCount: number;
   * @param pageNumber 当前页码
   * @param finishedCallback 完成后回调函数
   */
  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._courseCategoryService
      .getAllCourseCategoriesList()
      .pipe()
      .subscribe((result: ListResultDtoOfCourseCategoryListDto) => {
        this.categoriesdata = this.arrSrv.arrToTreeNode(result.items, {
          titleMapName: 'name',
          parentIdMapName: 'parentId',
          cb: (item, parent, deep) => {
          },
        });
      });
  }

  ngOnInit(): void {
    // 初始化加载表格数据
    this.refresh();
  }

  /**
   * 新增或编辑DTO信息
   * @param id 当前DTO
   */
  createOrEdit(id?: number): void {
    // console.log(id);

    this.modalHelper.static(CreateOrEditCourseCategoryComponent,
      { id: id })
      .subscribe(result => {
        if (result) {
          this.refresh();
        }
      });
  }

  addSubCourseCategory(parentId): void {
    this.modalHelper.static(CreateOrEditCourseCategoryComponent,
      { parentId: parentId })
      .subscribe(result => {
        if (result) {
          this.refresh();
        }
      });
  }

  /**
   * 删除功能
   * @param entity 角色的实体信息
   */
  delete(entity: CourseCategoryListDto): void {
    this._courseCategoryService.delete(entity.id)
      .subscribe(() => {
        /**
         * 刷新表格数据并跳转到第一页（`pageNumber = 1`）
         */
        this.refreshGoFirstPage();
        this.notify.success(this.l('SuccessfullyDeleted'));
      });
  }

  /**
   * 批量删除
   */
  batchDelete(): void {
    const selectCount = this.selectedDataItems.length;
    if (selectCount <= 0) {
      abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));
      return;
    }

    abp.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined, res => {
      if (res) {
        const ids = _.map(this.selectedDataItems, 'id');
        this._courseCategoryService.batchDelete(ids).subscribe(() => {
          this.refreshGoFirstPage();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  // 移动分类功能
  move = (e: NzFormatBeforeDropEvent): Observable<boolean> => {
    const input = new MoveCourseCategoryInput();

    const item = e.dragNode.origin;
    const parentitem = e.node.origin;
    // if (e.pos !== 0) {
    //   abp.message.warn(`只支持分类在不同类目的移动，且无法移动至顶层`);
    //   return of(false);
    // }
    input.id = parseFloat(e.dragNode.key);
    input.newParentId = parseFloat(e.node.key);
    if (item.parentId === parentitem.id) {
      abp.notify.info('当前分类已经在' + parentitem.name + '分类中，无法继续移动。');
      return of(false);
    }

    this.message.confirm('请确认是否移动' + item.name + '到' + parentitem.name + '类目中？', undefined, isConfirmed => {
      if (isConfirmed) {
        this._courseCategoryService
          .move(input)
          .pipe(map(() => true))
          .subscribe(result => {
            this.refresh();
          });
      }
    });

    return of(false);
  }

  get delMsg(): string {
    const childrenLen = this.menuEvent.node.children.length;
    if (childrenLen === 0) {
      return `确认删除【${this.menuEvent.node.title}】吗？`;
    }
    return `确认删除【${this.menuEvent.node.title}】以及所有子分类吗？`;
  }

  show(e: NzFormatEmitEvent) {
    this.item = e.node.origin as any;
    this.menuEvent = e;
  }

  showContextMenu(event: any, menu: any) {
    this.nzContextMenuService.create(event, menu);
  }

  closeContextMenu() {
    this.nzContextMenuService.close();
  }
}
