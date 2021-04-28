import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { EditionFeatureTreeComponent } from '@app/admin/shared/edition-feature-tree/edition-feature-tree.component';
import {
  EditionEditDto,
  ComboboxItemDto,
  EditionServiceProxy,
  CommonLookupServiceProxy,
  SubscribableEditionComboboxItemDto,
  CreateOrUpdateEditionDto
} from '@shared/service-proxies/service-proxies';
import { AppEditionExpireAction } from 'abpPro/AppEnums';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-create-or-edit-edition',
  templateUrl: './create-or-edit-edition.component.html',
  styles: []
})
export class CreateOrEditEditionComponent extends ModalComponentBase
  implements OnInit {
  @ViewChild(EditionFeatureTreeComponent)
  featureTree: EditionFeatureTreeComponent;

  saving = false;

  editionId?: number;
  edition: EditionEditDto = new EditionEditDto();
  expiringEditions: SubscribableEditionComboboxItemDto[] = [];

  expireAction: AppEditionExpireAction = AppEditionExpireAction.DeactiveTenant;
  expireActionEnum: typeof AppEditionExpireAction = AppEditionExpireAction;
  isFree = 'true';
  isTrialActive = false;
  isWaitingDayActive = false;

  constructor(
    injector: Injector,
    private _editionService: EditionServiceProxy,
    private _commonLookupService: CommonLookupServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.show(this.editionId);
  }

  show(editionId?: number): void {
    this._commonLookupService
      .getEditionsForCombobox(true)
      .subscribe(editionsResult => {
        this.expiringEditions = editionsResult.items;

        this._editionService
          .getEditionForEdit(editionId)
          .subscribe(editionResult => {
            this.edition = editionResult.edition;
            this.featureTree.editData = editionResult;

            this.expireAction =
              this.edition.expiringEditionId > 0
                ? AppEditionExpireAction.AssignToAnotherEdition
                : AppEditionExpireAction.DeactiveTenant;

            this.isFree =
              !editionResult.edition.monthlyPrice &&
              !editionResult.edition.annualPrice
                ? 'true'
                : 'false';
            this.isTrialActive = editionResult.edition.trialDayCount > 0;
            this.isWaitingDayActive =
              editionResult.edition.waitingDayAfterExpire > 0;
          });
      });
  }

  updateAnnualPrice(value): void {
    this.edition.annualPrice = value;
  }

  updateMonthlyPrice(value): void {
    this.edition.monthlyPrice = value;
  }

  resetPrices(isFree) {
    this.edition.annualPrice = undefined;
    this.edition.monthlyPrice = undefined;
  }

  removeExpiringEdition(isDeactivateTenant) {
    this.edition.expiringEditionId = null;
  }

  save(): void {
    const input = new CreateOrUpdateEditionDto();
    input.edition = this.edition;
    input.featureValues = this.featureTree.getGrantedFeatures();

    this.saving = true;
    this._editionService
      .createOrUpdateEdition(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.saving = false;
        this.success();
      });
  }
}
