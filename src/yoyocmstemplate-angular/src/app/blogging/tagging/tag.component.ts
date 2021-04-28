import { Component, Injector, OnInit, ChangeDetectorRef } from '@angular/core';
import * as _ from 'lodash';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { TagServiceProxy, PagedResultDtoOfTagListDto, TagListDto } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTagComponent } from './create-or-edit-tag/create-or-edit-tag.component';
import { AppConsts } from 'abpPro/AppConsts';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { finalize } from 'rxjs/operators';
import { STColumn, STChange } from '@delon/abc';

@Component({
  templateUrl: './tag.component.html',
  styleUrls: ['./tag.component.less'],
  animations: [appModuleAnimation()],
})
export class TagComponent extends PagedListingComponentBase<TagListDto> implements OnInit {
  constructor(
    injector: Injector,
    private _tagService: TagServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private _cdr: ChangeDetectorRef,
  ) {
    super(injector);
  }

  columns: STColumn[] = [
    { title: '编号', index: 'id', type: 'checkbox' },
    {
      title: this.l('TagName'),
      index: 'name',
    },
    { title: this.l('UsageCount'), index: 'usageCount' },
    { title: this.l('TagDescription'), index: 'description' },

    {
      title: this.l('Actions'),
      buttons: [
        {
          text: this.l('Edit'),
          icon: 'edit',
          type: 'modal',
          //   click: record => this._router.navigate(['/app/blogging/create-or-edit-post', { id: record.id }]),

          click: record => this.createOrEdit(record.id),
          iif: () => this.isGranted('Pages.Tag.Edit'),
        },
        {
          text: this.l('More'),
          iif: () => this.isGrantedAny('Pages.Tag.Create', 'Pages.Tag.Delete'),
          children: [
            // {
            //   text: record => (record.id === 1 ? `过期` : `正常`),
            //   click: record => this.message.error(`${record.id === 1 ? `过期` : `正常`}【${record.name}】`),
            // },
            // {
            //   text: `审核`,
            //   click: record => this.message.info(`check-${record.name}`),
            //   iif: record => record.id % 2 === 0,
            //   iifBehavior: 'disabled',
            //   tooltip: 'This is tooltip',
            // },
            // {
            //   type: 'divider',
            // },
            {
              text: this.l('Delete'),
              icon: 'delete',
              pop: true,
              popTitle: this.l('ConfirmDeleteWarningMessage'),
              click: (record: TagListDto) => this.delete(record),
            },
          ],
        },
      ],
    },
  ];

  STChange(e: STChange) {
    // console.log(e);
    switch (e.type) {
      case 'checkbox':
        //     console.log(e.checkbox);
        this.refreshCheckStatus(e.checkbox);
        this._cdr.detectChanges();
        break;
      case 'filter':
        //  this.getData();
        break;
    }
  }

  /**
   * 获取后端数据列表信息
   * @param request 请求的数据的dto 请求必需参数 skipCount: number; maxResultCount: number;
   * @param pageNumber 当前页码
   * @param finishedCallback 完成后回调函数
   */
  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._tagService
      .getPaged(this.filterText, request.sorting, request.maxResultCount, request.skipCount)
      .pipe(finalize(() => finishedCallback()))
      .subscribe(result => {
        this.dataList = result.items;
        this.showPaging(result);
        this._cdr.detectChanges();
      });
  }

  ngOnInit(): void {
    // 初始化加载表格数据
    this.refresh();
  }

  /**
   * 新增或编辑DTO信息
   * @param id 当前DTO的Id
   */
  createOrEdit(id?: number): void {
    this.modalHelper.static(CreateOrEditTagComponent, { id: id }).subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }

  /**
   * 删除功能
   * @param entity 角色的实体信息
   */
  delete(entity: TagListDto): void {
    this._tagService.delete(entity.id).subscribe(() => {
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
        this._tagService.batchDelete(ids).subscribe(() => {
          this.refreshGoFirstPage();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  /**
   * 导出为Excel表
   */
  exportToExcel(): void {
    // abp.message.error('已经开发完成测试通过！！！！');
    this._tagService.getToExcelFile().subscribe(result => {
      this._fileDownloadService.downloadTempFile(result);
    });
  }
}
