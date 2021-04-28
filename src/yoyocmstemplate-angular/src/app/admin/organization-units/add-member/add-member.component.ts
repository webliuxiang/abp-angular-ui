import { Component, OnInit, Injector, Input } from '@angular/core';
import {
  NameValueDto,
  OrganizationUnitServiceProxy,
  FindUsersInput,
  UsersToOrganizationUnitInput,
  PagedResultDtoOfNameValueDto,
} from '@shared/service-proxies/service-proxies';
import { PagedRequestDto, PagedListingComponentBase } from '@shared/component-base/paged-listing-component-base';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { finalize } from 'rxjs/operators';
import * as _ from 'lodash';
import { ModalPagedListingComponentBase } from '@shared/component-base/modal-paged-listing-component-base';

@Component({
  selector: 'app-add-member',
  templateUrl: './add-member.component.html',
  styles: [],
})
export class AddMemberComponent extends ModalPagedListingComponentBase<NameValueDto> {
  /**
   * 机构Id
   */
  organizationUnitId: number;
  filterText = '';

  /**
   * 构造函数
   * @param injector 注入器
   * @param _organizationUnitService 组织机构服务
   */
  constructor(injector: Injector, private _organizationUnitService: OrganizationUnitServiceProxy) {
    super(injector);
  }

  /**
   * 获取数据列表
   * @param request 分页请求必须参数
   * @param pageNumber 当前页码
   * @param finishedCallback 完成后回调函数
   */
  protected getDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    // 设置查询数据
    const input = new FindUsersInput();
    input.organizationUnitId = this.organizationUnitId;
    input.filterText = this.filterText;
    input.skipCount = request.skipCount;
    input.maxResultCount = request.maxResultCount;

    this._organizationUnitService
      .findUsers(input)
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
      )
      .subscribe((result: PagedResultDtoOfNameValueDto) => {
        this.dataList = result.items;

        this.showPaging(result);
      });
  }

  protected delete(entity: NameValueDto): void {}
  /**
   * 添加用户到当前组织
   */
  addUsersToOrganizationUnit(): void {
    const selectCount = this.selectedDataItems.length;
    if (selectCount <= 0) {
      abp.message.warn(this.l('PleaseSelectAtLeastOneItem'));
      return;
    }
    this.saving = true;
    const input = new UsersToOrganizationUnitInput();
    input.organizationUnitId = this.organizationUnitId;
    input.userIds = _.map(this.selectedDataItems, selectedMember => Number(selectedMember.value));

    this._organizationUnitService
      .addUsers(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));

        this.success(input.userIds);
      });
  }
}
