import {
  Component,
  OnInit,
  Injector,
  ViewChild,
  AfterViewInit
} from '@angular/core';
import {
  TenantServiceProxy,
  EntityDto,
  UpdateTenantFeaturesInput
} from '@shared/service-proxies/service-proxies';
import { EditionFeatureTreeComponent } from '@app/admin/shared/edition-feature-tree/edition-feature-tree.component';
import { ModalComponentBase } from '@shared/component-base';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-edit-tenant-features',
  templateUrl: './edit-tenant-features.component.html',
  styles: []
})
export class EditTenantFeaturesComponent extends ModalComponentBase
  implements AfterViewInit {
  @ViewChild(EditionFeatureTreeComponent)
  featureTree: EditionFeatureTreeComponent;

  saving = false;

  resettingFeatures = false;
  tenantId: number;
  tenantName: string;
  featureEditData: any = null;

  constructor(injector: Injector, private _tenantService: TenantServiceProxy) {
    super(injector);
  }

  ngAfterViewInit(): void {
    this.show();
  }

  show(): void {
    this.loadFeatures();
  }

  loadFeatures(): void {
    this._tenantService
      .getTenantFeaturesForEdit(this.tenantId)
      .subscribe(result => {
        this.featureTree.editData = result;
      });
  }

  save(): void {
    if (!this.featureTree.areAllValuesValid()) {
      this.message.warn(this.l('InvalidFeaturesWarning'));
      return;
    }

    const input = new UpdateTenantFeaturesInput();
    input.id = this.tenantId;
    input.featureValues = this.featureTree.getGrantedFeatures();

    this.saving = true;
    this._tenantService
      .updateTenantFeatures(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success();
      });
  }

  resetFeatures(): void {
    const input = new EntityDto();
    input.id = this.tenantId;

    this.resettingFeatures = true;
    this._tenantService
      .resetTenantSpecificFeatures(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('ResetSuccessfully'));
        this.loadFeatures();
      });
  }
}
