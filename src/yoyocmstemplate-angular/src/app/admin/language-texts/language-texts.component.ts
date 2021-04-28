import { Component, OnInit, AfterViewInit, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import {
  LanguageTextListDto,
  UpdateLanguageTextInput,
  LanguageServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { AppConsts } from 'abpPro/AppConsts';
import { EditLanguageTextComponent } from '@app/admin/language-texts/edit-language-text/edit-language-text.component';
import { ReuseTabService } from '@delon/abc';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-language-texts',
  templateUrl: './language-texts.component.html',
  styles: [],
})
export class LanguageTextsComponent extends PagedListingComponentBase<LanguageTextListDto> implements AfterViewInit {
  languages: abp.localization.ILanguageInfo[] = [];
  baseLanguageName: string;
  targetLanguageName: string;
  sourceNames: string[] = [];
  sourceName: string;

  targetValueFilters: any[] = [
    {
      label: this.l('All'),
      value: 'ALL',
    },
    {
      label: this.l('EmptyOnes'),
      value: 'EMPTY',
    },
  ];
  targetValueFilter = 'ALL';

  filterText: string;

  constructor(
    injector: Injector,
    private _languageService: LanguageServiceProxy,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _reuseTabService: ReuseTabService,
  ) {
    super(injector);

    abp.localization.sources.forEach((item, index, array) => {
      if (item.type === 'MultiTenantLocalizationSource') {
        this.sourceNames.push(item.name);
      }
    });
    this.languages = abp.localization.languages;
    this.init();
  }

  ngAfterViewInit(): void {}

  protected fetchDataList(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this._languageService
      .getLanguageTexts(
        this.sourceName,
        this.baseLanguageName,
        this.targetLanguageName,
        this.targetValueFilter,
        this.filterText,
        undefined,
        request.maxResultCount,
        request.skipCount,
      )
      .pipe(finalize(() => finishedCallback()))
      .subscribe(result => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }

  init(): void {
    this.sourceName = AppConsts.localization.defaultLocalizationSourceName;
    this.baseLanguageName = abp.localization.currentLanguage.name;
    this._reuseTabService.title = this.l('LanguageTexts');

    this._activatedRoute.params.subscribe((params: Params) => {
      this.targetLanguageName = params.lang;
    });
  }

  onSearch(e?: any): void {
    this.pageNumber = 1;
    this.refresh();
  }

  truncateString(text): string {
    return abp.utils.truncateStringWithPostfix(text, 32, '...');
  }

  findIcon(name: string): string {
    let icon = '';
    for (let index = 0; index < this.languages.length; index++) {
      if (this.languages[index].name === name) {
        icon = this.languages[index].icon;
        break;
      }
    }
    return icon;
  }

  backLanguageList(): void {
    this._router.navigate(['app/admin/languages']);
  }

  edit(data: LanguageTextListDto): void {
    const pars = new UpdateLanguageTextInput();
    pars.sourceName = this.sourceName;
    pars.key = data.key;
    pars.languageName = this.targetLanguageName;
    pars.value = data.targetValue;

    this.modalHelper
      .open(EditLanguageTextComponent, {
        data: pars,
        baseText: data.baseValue,
        baseLanguageName: this.baseLanguageName,
        targetLanguageName: this.targetLanguageName,
      })
      .subscribe(res => {
        if (res) {
          this.refresh();
        }
      });
  }
}
