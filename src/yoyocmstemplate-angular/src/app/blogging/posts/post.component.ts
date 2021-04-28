import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Injector, NgZone, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { STChange, STColumn } from '@delon/abc';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { PostListDto, PostServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';
import { CreateOrEditPostComponent } from './create-or-edit-post/create-or-edit-post.component';
import { CreateOrEditCommentComponent } from '../comments/create-or-edit-comment/create-or-edit-comment.component';

@Component({
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.less'],
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PostComponent extends PagedListingComponentBase<PostListDto> implements OnInit {
  constructor(
    injector: Injector,
    private _postService: PostServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private _router: Router,
    public _zone: NgZone,
    private _cdr: ChangeDetectorRef,
  ) {
    super(injector);
  }

  columns: STColumn[] = [
    { title: '编号', index: 'id', type: 'checkbox' },

    {
      title: this.l('BlogName'),
      index: 'blogName',
    },
    {
      title: this.l('PostTitle'),
      index: 'title',
    },
    {
      title: this.l('PostCoverImage'),
      type: 'img',
      width: 60,
      index: 'coverImage',
      // click: (record: PostListDto) => this.delete(record),
    },
    { title: this.l('PostUrl'), index: 'url' },

    { title: this.l('ReadCount'), index: 'readCount' },

    { title: this.l('PostType'), index: 'postTypeDescirption' },

    { title: this.l('Tags'), index: 'tags' },

    {
      title: this.l('Actions'),
      buttons: [
        {
          text: this.l('Edit'),
          icon: 'edit',
          type: 'modal',
          click: record => this._router.navigate(['/app/blogging/create-or-edit-post', { id: record.id }]),

          //  click: record => this.createOrEdit(record.id),
          iif: () => this.isGranted('Pages.Post.Edit'),
        },
        {
          text: this.l('NewPost'),
          icon: 'plus',
          type: 'link',
          click: record => this._router.navigate(['/app/blogging/create-or-edit-post', { blogId: record.c }]),
        },
        {
          text: this.l('More'),
          iif: () => this.isGrantedAny('Pages.Post.Create', 'Pages.Post.Delete'),
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
              click: (record: PostListDto) => this.delete(record),
            },
            {
              text: this.l('CreateComment'),
              icon: 'plus',
              click: (record: PostListDto) => this.createComment(record),
            },
          ],
        },
      ],
    },
  ];

  STChange(e: STChange) {
    console.log(e);
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
    this._postService
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
    this.registerPostRefreshEvents();
  }

  /**
   * 新增或编辑DTO信息
   * @param id 当前DTO的Id
   */
  createOrEdit(id?: number): void {
    this.modalHelper.static(CreateOrEditPostComponent, { id: id }).subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }

  /**
   * 删除功能
   * @param entity 角色的实体信息
   */
  delete(entity: PostListDto): void {
    this._postService.delete(entity.id).subscribe(() => {
      /**
       * 刷新表格数据并跳转到第一页（`pageNumber = 1`）
       */
      this.refreshGoFirstPage();
      this.notify.success(this.l('SuccessfullyDeleted'));
    });
  }

  /**添加评论 */
  createComment(entity: PostListDto): void {
    this.modalHelper.static(CreateOrEditCommentComponent, { id: undefined, postId: entity.id }).subscribe(result => {
      if (result) {
        this.refresh();
      }
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
        this._postService.batchDelete(ids).subscribe(() => {
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
    this._postService.getToExcelFile().subscribe(result => {
      this._fileDownloadService.downloadTempFile(result);
    });
  }

  registerPostRefreshEvents() {
    abp.event.on('abp.posts.refresh', () => {
      this._zone.run(() => {
        this.refresh();
      });
    });
  }
}
