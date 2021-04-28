import { Component, OnInit, Injector } from '@angular/core';
import { AuditLogListDto, AuditLogServiceProxy } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { AuditLogsDetailComponent } from '@app/admin/audit-logs/audit-logs-detail/audit-logs-detail.component';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-audit-logs',
  templateUrl: './audit-logs.component.html',
  styles: [],
})
export class AuditLogsComponent extends PagedListingComponentBase<AuditLogListDto> {
  //  <nz-range-picker [(ngModel)]="dateRange" (ngModelChange)="onChange($event)" nzShowTime></nz-range-picker>
  startToEndDate = []; // [ new Date(), addDays(new Date(), 3) ];
  advancedFiltersVisible = false;
  username: string;
  serviceName: string;
  methodName: string;
  browserInfo: string;
  hasException: boolean = undefined;
  minExecutionDuration: number;
  maxExecutionDuration: number;

  errorState = [
    { label: this.l('All'), value: '' },
    { label: this.l('Success'), value: 'false' },
    { label: this.l('HasError'), value: 'true' },
  ];

  constructor(injector: Injector, private auditLogService: AuditLogServiceProxy) {
    super(injector);
  }

  fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    let startData;
    let endData;
    if (this.startToEndDate.length === 2) {
      startData = this.startToEndDate[0] === null ? undefined : this.startToEndDate[0];
      endData = this.startToEndDate[1] === null ? undefined : this.startToEndDate[1];
    }

    this.auditLogService
      .getPagedAuditLogs(
        startData,
        endData,
        this.username,
        this.serviceName,
        this.methodName,
        this.browserInfo,
        this.hasException,
        this.minExecutionDuration,
        this.maxExecutionDuration,
        this.sorting,
        request.maxResultCount,
        request.skipCount,
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
      )
      .subscribe(result => {
        this.dataList = result.items;
        this.showPaging(result);
      });
  }

  protected delete(entity: AuditLogListDto): void {}

  //
  showDetails(item: AuditLogListDto): void {
    this.modalHelper.open(AuditLogsDetailComponent, { auditLog: item }).subscribe(result => {});
  }

  truncateStringWithPostfix(text: string, length: number): string {
    return abp.utils.truncateStringWithPostfix(text, length);
  }
}
