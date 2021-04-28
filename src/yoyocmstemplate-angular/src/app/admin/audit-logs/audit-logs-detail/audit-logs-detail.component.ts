import { Component, OnInit, Input, Injector } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { AuditLogListDto } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';


@Component({
  selector: 'app-audit-logs-detail',
  templateUrl: './audit-logs-detail.component.html',
  styles: []
})
export class AuditLogsDetailComponent extends ModalComponentBase {

  @Input() auditLog: AuditLogListDto;

  constructor(
    injector: Injector
  ) {
    super(injector);
  }

  getExecutionTime(): string {
    const self = this;
    return moment(self.auditLog.executionTime).fromNow() + ' (' + moment(self.auditLog.executionTime).format('YYYY-MM-DD HH:mm:ss') + ')';
  }

  getDurationAsMs(): string {
    const self = this;
    return self.l('Xms', self.auditLog.executionDuration);
  }

  getFormattedParameters(): string {
    const self = this;
    try {
      const json = JSON.parse(self.auditLog.parameters);
      return JSON.stringify(json, null, 4);
    } catch (e) {
      return self.auditLog.parameters;
    }
  }

}
