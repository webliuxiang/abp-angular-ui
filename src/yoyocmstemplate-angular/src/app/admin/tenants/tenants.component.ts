import { Component, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  TenantServiceProxy,
  TenantListDto,
  SubscribableEditionComboboxItemDto,
  EntityDtoOfInt64,
  NameValueDto,
  CommonLookupServiceProxy,
  CommonLookupFindUsersInput,
} from '@shared/service-proxies/service-proxies';
import { CreateTenantComponent } from '@app/admin/tenants/create-tenant/create-tenant.component';
import { EditTenantComponent } from './edit-tenant/edit-tenant.component';
import { CommonLookupComponent } from '../common/common-lookup/common-lookup.component';
import { EditTenantFeaturesComponent } from './edit-tenant-features/edit-tenant-features.component';
import { ImpersonationService } from '@shared/auth';
import { ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-tenants',
  templateUrl: './tenants.component.html',
  styles: [],
})
export class TenantsComponent extends PagedListingComponentBase<TenantListDto> {
  advancedFiltersVisible = false; // 是否显示高级过滤
  editionId: any = null; // 版本Id
  subscribableDateRange = [null, null]; // 订阅时间范围
  createDateRange = [null, null]; // 创建时间范围

  constructor(
    injector: Injector,
    private _activatedRoute: ActivatedRoute,
    private _tenantService: TenantServiceProxy,
    private _impersonationService: ImpersonationService,
    private _commonLookupService: CommonLookupServiceProxy,
  ) {
    super(injector);
    this.filterText = this._activatedRoute.snapshot.queryParams.filterText || '';
  }

  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._tenantService
      .getPaged(
        this.advancedFiltersVisible ? this.subscribableDateRange[0] || undefined : undefined, // 订阅结束时间开始
        this.advancedFiltersVisible ? this.subscribableDateRange[1] || undefined : undefined, // 订阅结束时间结束
        this.advancedFiltersVisible ? this.createDateRange[0] || undefined : undefined, // 创建时间开始
        this.advancedFiltersVisible ? this.createDateRange[1] || undefined : undefined, // 创建时间结束
        this.editionId || undefined, // 版本id
        this.filterText, // 名称过滤字符串
        this.sorting, // 排序字段
        request.maxResultCount, // 最大数据量
        request.skipCount, // 跳过数据量
      )
      .pipe(finalize(() => finishedCallback()))
      .subscribe(result => {
        this.dataList = result.items;
        this.showPaging(result);
      });
  }

  /**
   * 强制刷新
   */
  forceRefresh() {
    this.filterText = '';
    this.refreshGoFirstPage();
  }

  /**
   * 解锁此租户默认管理员
   * @param entity
   */
  unlockTenantAdminUser(entity: TenantListDto): void {
    this._tenantService.unlockTenantAdmin(new EntityDtoOfInt64({ id: entity.id })).subscribe(() => {
      this.notify.success(this.l('UnlockedTenandAdmin', entity.name));
    });
  }

  /**
   * 修改租户的功能
   * @param entity
   */
  changeTenantFeatures(entity: TenantListDto): void {
    this.modalHelper
      .createStatic(EditTenantFeaturesComponent, {
        tenantId: entity.id,
        tenantName: name,
      })
      .subscribe(res => {});
  }

  /**
   * 使用此租户模拟登陆
   * @param entity
   */
  tenantImpersonateLogin(entity: TenantListDto): void {
    this.modalHelper
      .createStatic(CommonLookupComponent, {
        tenantId: entity.id,
        options: {
          dataSource: (skipCount: number, maxResultCount: number, filter: string, tenantId?: number) => {
            const input = new CommonLookupFindUsersInput();
            input.filterText = filter;
            input.maxResultCount = maxResultCount;
            input.skipCount = skipCount;
            input.tenantId = tenantId;
            return this._commonLookupService.findUsers(input);
          },
          isFilterEnabled: true,
          canSelect: () => true,
        },
      })
      .subscribe(item => {
        if (item) {
          this.impersonateUser(item, entity.id);
        }
      });
  }

  edit(entity: TenantListDto): void {
    this.modalHelper.static(EditTenantComponent, { entityId: entity }).subscribe(res => {
      if (res) {
        this.refresh();
      }
    });
  }

  delete(entity: TenantListDto): void {
    this._tenantService.delete(entity.id).subscribe(() => this.refresh());
  }

  create(): void {
    this.modalHelper.open(CreateTenantComponent, null, 'md').subscribe(res => {
      if (res) {
        this.refresh();
      }
    });
  }

  batchDelete(): void {
    this._tenantService.batchDelete(this.selectedDataItems).subscribe(() => this.refresh());
  }

  selectedEditionChange(edition: SubscribableEditionComboboxItemDto) {
    this.editionId = edition ? edition.value : null;
    this.refresh();
  }

  impersonateUser(item: NameValueDto, id: number): void {
    this._impersonationService.impersonate(parseInt(item.value), id);
  }
}
