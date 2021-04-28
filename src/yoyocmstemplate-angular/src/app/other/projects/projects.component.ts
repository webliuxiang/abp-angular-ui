import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { STData, STColumn, STChange } from '@delon/abc';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base';
import { ProjectListDto, ProjectServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { CreateOrEditProjectComponent } from './create-or-edit-project/create-or-edit-project.component';
import * as _ from 'lodash';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styles: [],
})
export class ProjectsComponent extends PagedListingComponentBase<ProjectListDto> implements OnInit {


  /**选中的行 */
  selectedRows: STData[] = [];

  columns: STColumn[] = [
    { title: '编号', index: 'id', type: 'checkbox' },
    {
      title: this.l('CoverImg'),
      index: 'imgUrl',
      type: 'img',
    },
    {
      title: this.l('DisplayName'),
      index: 'name',
    },
    {
      title: this.l('ShortName'),
      index: 'shortName',
    },
    {
      title: this.l('Format'),
      index: 'format',
    },
    { title: this.l('DefaultDocumentName'), index: 'defaultDocumentName' },
    { title: this.l('NavigationDocumentName'), index: 'navigationDocumentName' },
    { title: this.l('DocumentStoreType'), index: 'documentStoreType' },
    { title: this.l('LatestVersionBranchName'), index: 'latestVersionBranchName' },
    { title: this.l('MinimumVersion'), index: 'minimumVersion' },
    { title: this.l('Enabled'), index: 'enabled' },
    { title: this.l('Sort'), index: 'sort' },
    {
      title: this.l('Actions'),
      buttons: [
        {
          text: this.l('Edit'),
          icon: 'edit',
          type: 'modal',
          click: record => this.createOrEdit(record.id),
        },
        {
          text: this.l('More'),
          children: [
            {
              text: this.l('Delete'),
              icon: 'delete',
              pop: true,
              popTitle: this.l('ConfirmDeleteWarningMessage'),
              click: record => this.delete(record.id),
            },
          ],
        },
      ],
    },
  ];

  constructor(
    injector: Injector,
    private _cdr: ChangeDetectorRef,
    private _router: Router,
    private _projectService: ProjectServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    // 初始化加载表格数据
    this.refresh();
  }




  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._projectService
      .getPaged(this.filterText, request.sorting, request.maxResultCount, request.skipCount)
      .pipe(finalize(() => finishedCallback()))
      .subscribe(result => {
        this.dataList = result.items;
        this.showPaging(result);
        this._cdr.detectChanges();
      });
  }

  createOrEdit(id?: number): void {
    this.modalHelper.static(CreateOrEditProjectComponent, { id: id }).subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }

  exportToExcel() {

  }

  delete(id: any) {
    this._projectService.delete(id).pipe().subscribe(() => {
      this.refreshGoFirstPage();
      this.notify.success(this.l('SuccessfullyDeleted'));
    });
  }

  batchDelete() {
    const selectCount = this.selectedDataItems.length;
    if (selectCount <= 0) {
      abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));
      return;
    }

    abp.message.confirm(this.l('ConfirmDeleteXItemsWarningMessage', selectCount), undefined, res => {
      if (res) {
        const ids = _.map(this.selectedDataItems, 'id');
        this._projectService.batchDelete(ids).subscribe(() => {
          this.refreshGoFirstPage();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  STChange(e: STChange) {
    // console.log(e);
    switch (e.type) {
      case 'checkbox':
        console.log(e.checkbox);
        this.refreshCheckStatus(e.checkbox);

        // this.selectedRows = e.checkbox!;
        this._cdr.detectChanges();
        break;
      case 'filter':
        //  this.getData();
        break;
    }
  }
}
