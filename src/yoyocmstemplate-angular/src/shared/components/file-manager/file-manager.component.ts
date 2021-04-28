import {
  Component,
  OnInit,
  Input,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Output,
  EventEmitter,
  TemplateRef,
  Injector,
} from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { UploadFile } from 'ng-zorro-antd/upload';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzFormatEmitEvent } from 'ng-zorro-antd/tree';
import { ArrayService, copy } from '@delon/util';
import { AppComponentBase, PagedListingComponentBase, PagedRequestDto } from '@shared/component-base';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from 'abpPro/AppConsts';
import { TokenService } from 'abp-ng2-module';
import {
  SysFileServiceProxy,
  SysFileListDto,
  PagedResultDtoOfSysFileListDto,
  SysFileEditDto,
} from '../../service-proxies/service-proxies';
import { finalize, filter } from 'rxjs/operators';
import { EntityDtoOfGuid, MoveSysFilesInput } from '../../service-proxies/service-proxies';
import { element } from 'protractor';

@Component({
  selector: 'file-manager',
  templateUrl: './file-manager.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FileManagerComponent extends PagedListingComponentBase<SysFileListDto> implements OnInit {
  constructor(
    injector: Injector,
    private _sysfileService: SysFileServiceProxy,
    public http: _HttpClient,
    private cdr: ChangeDetectorRef,
    private arrSrv: ArrayService,
    private msg: NzMessageService,
    private _tokenService: TokenService,
  ) {
    super(injector);
    // 设置头部信息
    this.uploadHeaders = {
      Authorization: 'Bearer ' + this._tokenService.getToken(),
    };
  }

  uploadHeaders: any;
  parentId = '';
  uploadFileUrl: string;

  filepath = AppConsts.remoteServiceBaseUrl + '/sysfiles/';

  showType: 'big' | 'small' = 'big';
  s: any = { orderby: 0, ps: 20, pi: 1, q: '' };
  loading = false;

  directoryLists: any[] = [];

  list: any[] = [];
  item: any;
  /** 路径的深度 */
  path: number[] = [0];
  total = 0;

  @Input()
  params: any;

  @Input()
  actions: TemplateRef<any>;

  @Input()
  multiple: boolean | number = false;

  @Output()
  selected = new EventEmitter<any>();

  // #endregion

  // #region rename

  /**文件夹模型 */
  directoryModel = false;
  /**文件夹名称 */
  directoryName = '';
  renameModel = false;
  renameTitle = '';

  // #endregion

  // #region move
  moveModel = false;
  moveId = '';
  folderNodes: any[] = [];

  // 模糊搜索
  filterText = '';

  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._sysfileService
      .getPaged(this.parentId, this.filterText, request.sorting, request.maxResultCount, request.skipCount)
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
      )
      .subscribe((result: any) => {
        this.list = result.items.map(res => {
          res.fileExt = res.fileExt.replace('.', '');
          res.selected = false;
          res.path = this.filepath + res.path;
          return res;
        });

        // console.log(this.list);
        this.showPaging(result);
        this.cdr.detectChanges();
      });
  }

  ngOnInit(): void {
    // 初始化加载表格数据
    this.refresh();
    this.getFileUploadUrl('');
  }

  getCode(mp: string, type: 'link' | 'code') {
    return type === 'link' ? mp : `<img src="${mp}">`;
  }

  getFileUploadUrl(parendId: string) {
    this.uploadFileUrl = AppConsts.remoteServiceBaseUrl + '/api/services/app/SysFile/Create?parentId=' + parendId;
  }

  // #region op

  back() {
    this.path.pop(); // 移除最后一个元素
    //  console.log(this.path);
    const length = this.path.length;
    const tempParentId = this.path[length - 1];
    if (tempParentId === 0) {
      this.parentId = '';
    } else {
      this.parentId = tempParentId.toString();
    }
    this.getFileUploadUrl(this.parentId);

    this.refresh();

    //   this.refreshGoFirstPage();
  }

  /** 进入文件夹 */
  next(i: any) {
    this.path.push(i.id);
    // console.log(i);
    // console.log(this.path);

    this.parentId = i.id;
    this.getFileUploadUrl(i.id);
    this.refresh();
  }

  /**
   * 如果是文件夹则显示文件夹中的内容
   * @param i 选中的项
   */
  cho(i: any) {
    if (i.dir) {
      this.next(i);
      return;
    }
    i.selected = !i.selected;
    this.selected.emit(i);
    this.cdr.detectChanges();
  }

  // #endregion

  // #region uplad

  uploadChange({ file }: { file: UploadFile }) {
    if (file.status === 'done') {
      this.refresh();
    }
  }

  openDirectory() {
    this.directoryModel = true;
  }

  createDirecotry() {
    const input = new SysFileEditDto();
    input.name = this.directoryName;
    input.parentId = this.parentId;
    this._sysfileService
      .createDirectory(input)
      .pipe()
      .subscribe((res: any) => {
        //  console.log(res);
        res.fileExt = res.fileExt.replace('.', '');
        res.selected = false;
        this.list.push(res);
        this.notify.success(this.l('SavedSuccessfully'));
        this.directoryName = null;
        //    this.refresh();
        this.directoryModel = false;

        this.cdr.detectChanges();
      });
  }

  /**
   * 获取待移动的文件夹列表
   * @param data 待移动的文件夹列表
   */
  getMoveDirectoryList(parentId: string): any[] {
    let directories: any[] = [];
    if (parentId == null) {
      directories = this.directoryLists.filter(item => {
        return item.parentId == null;
      });
    } else {
      directories = this.directoryLists.filter(item => {
        if (item.parentId == parentId) {
          item.disabled = true;
        }

        return item.parentId == parentId;
      });
    }

    console.log(this.directoryLists);
    return directories;
  }

  /**
   * 懒加载当前文件夹下的目录
   * @param e 选择的文件夹
   */
  onDirectoryExpandChange(e: NzFormatEmitEvent): void {
    // console.log(e.node);
    const node = e.node;
    if (node && node.getChildren().length === 0 && node.isExpanded) {
      const data = this.getMoveDirectoryList(e.node.key);
      // console.log(data);

      node.addChildren(data);

      // this.loadNode().then(data => {
      //   node.addChildren(data);
      // });
    }
  }

  /** 打开模态框，获取文件夹列表 */
  openMoveModal(i: any) {
    this.directoryLists = [];
    this.moveModel = true;
    this.item = i; // 当前文件夹
    console.log(i); // 003
    this.moveId = i.parentId;

    this._sysfileService
      .getDirectories()
      .pipe()
      .subscribe((res: any) => {
        res.splice(0, 0, { id: '', parentId: null, name: '根目录' }); // 往数组中添加一个对象

        res.forEach((element: { id: any; parentId: any; code: any; name: any }) => {
          this.directoryLists.push({
            key: element.id,
            parentId: element.parentId,
            code: element.code,
            title: element.name,
          });
        });

        const directories = this.getMoveDirectoryList(null);

        this.folderNodes = this.arrSrv.arrToTree(directories, {
          cb: e => {
            if (e.parentId === this.moveId) {
              e.disabled = true;
            }
          },
        });

        //  console.log(this.folderNodes);
        this.cdr.detectChanges();
      });
  }
  moveOk() {
    console.log(this.item);
    console.log(this.moveId);
    const input = new MoveSysFilesInput();
    input.id = this.item.id;
    input.newParentId = this.moveId;
    this._sysfileService
      .move(input)
      .pipe()
      .subscribe((res: any) => {
        this.refresh(); // 刷新表格数据并跳转到第一页（`pageNumber = 1`）
        this.notify.success(this.l('SavedSuccessfully'));
        this.moveModel = false;
        //     this.list.splice(
        //       this.list.findIndex(w => w.id === this.item.id),
        //       1,
        //     );

        this.cdr.detectChanges();
      });
  }
  // #endregion

  /** 打开重命名模态框 */
  rename(i: any) {
    this.renameModel = true;
    this.item = i;
    this.renameTitle = i.name;
  }
  /** 重命名方法 */
  renameOk() {
    const input = new SysFileEditDto();
    input.id = this.item.id;
    input.name = this.renameTitle;
    this._sysfileService.reFileName(input).subscribe(() => {
      this.refresh(); // 刷新表格数据并跳转到第一页（`pageNumber = 1`）
      this.notify.success(this.l('SuccessfullyDeleted'));
      this.renameModel = false;
      this.cdr.detectChanges();
    });
  }
  /**
   * 复制代码或者链接
   * @param mp url
   * @param type 代码或者链接
   */
  copyData(mp: string, type: 'link' | 'code') {
    copy(this.getCode(mp, type)).then(() => this.msg.success('Copy Success'));
  }

  // #endregion
  // #region copy
  /**
   * 复制文件
   * @param id 待copy文件id
   */
  copyImg(id: any) {
    const input = new SysFileEditDto();
    input.id = id;
    this._sysfileService
      .copyFile(input)
      .pipe()
      .subscribe((res: any) => {
        //    console.log(res);
        res.selected = false;
        res.path = this.filepath + res.path;

        this.list.push(res);
        this.notify.success(this.l('SavedSuccessfully'));

        //    this.refresh();

        this.cdr.detectChanges();
      });
  }

  // #region remove

  /**
   * 删除文件
   * @param id 文件id
   */
  remove(id: any) {
    this._sysfileService
      .delete(id)
      .pipe()
      .subscribe(() => {
        this.refresh();
        this.notify.success(this.l('SuccessfullyDeleted'));
      });
  }

  // #endregion
}
