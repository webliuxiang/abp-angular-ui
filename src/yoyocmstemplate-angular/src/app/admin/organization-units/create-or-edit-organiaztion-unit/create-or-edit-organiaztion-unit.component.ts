import { Component, OnInit, Injector, Input } from '@angular/core';
// tslint:disable-next-line:max-line-length
import {
  OrganizationUnitServiceProxy,
  CreateOrganizationUnitInput,
  UpdateOrganizationUnitInput,
  OrganizationUnitListDto,
} from '@shared/service-proxies/service-proxies';
import { Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';

export interface IOrganizationUnitOnEdit {
  id?: number;
  parentId?: number;
  displayName?: string;
  parentDisplayName?: string;
}

@Component({
  selector: 'app-create-or-edit-organiaztion-unit',
  templateUrl: './create-or-edit-organiaztion-unit.component.html',
  styles: [],
})
export class CreateOrEditOrganiaztionUnitComponent extends ModalComponentBase
  implements OnInit {
  @Input()
  organizationUnit: IOrganizationUnitOnEdit = {};

  constructor(
    injector: Injector,
    private organizationUnitService: OrganizationUnitServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit() {}

  save(): void {
    if (this.organizationUnit.id) {
      this.updateUnit();
    } else {
      // 创建
      this.createUnit();
    }
  }

  updateUnit(): any {
    // 编辑
    const updateInput = new UpdateOrganizationUnitInput();
    updateInput.id = this.organizationUnit.id;
    updateInput.displayName = this.organizationUnit.displayName;
    this.saving = true;
    this.organizationUnitService
      .update(updateInput)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(result => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(result);
      });
  }
  createUnit(): any {
    // 创建
    const input = new CreateOrganizationUnitInput();
    input.parentId = this.organizationUnit.parentId;
    input.displayName = this.organizationUnit.displayName;
    this.saving = true;

    this.organizationUnitService
      .create(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe((result: OrganizationUnitListDto) => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(result);
      });
  }
}
