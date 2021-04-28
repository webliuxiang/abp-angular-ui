import { Component, OnInit, Injector, Input } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
  LanguageServiceProxy,
  LanguageEditDto,
  ComboboxItemDto,
  CreateOrUpdateLanguageInput,
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-create-or-edit-language',
  templateUrl: './create-or-edit-language.component.html',
  styles: [],
})
export class CreateOrEditLanguageComponent extends ModalComponentBase implements OnInit {
  @Input()
  languageId: number = undefined;
  saving = false;
  language: LanguageEditDto = new LanguageEditDto();
  selectedLanguage: string;
  languageNames: ComboboxItemDto[] = [];
  flags: ComboboxItemDto[] = [];

  constructor(injector: Injector, private languageService: LanguageServiceProxy) {
    super(injector);
  }

  ngOnInit() {
    this.init();
  }

  init(): void {
    this.languageService
      .getLanguageForEdit(this.languageId)
      .pipe()

      .subscribe(result => {
        this.language = result.language;
        this.languageNames = result.languageNames;
        this.flags = result.flags;

        this.selectedLanguage = result.language.name || '';

        if (!this.languageId) {
          this.language.isEnabled = true;
        }
      });
  }

  save(): void {
    this.saving = true;

    if (!this.selectedLanguage || this.selectedLanguage === '' || !this.language.icon || this.language.icon === '') {
      this.saving = false;
      this.message.warn('值不能为空');
      return;
    }

    this.language.name = this.selectedLanguage;
    const input = new CreateOrUpdateLanguageInput();
    input.language = this.language;

    this.languageService
      .createOrUpdateLanguage(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success();
      });
  }
}
