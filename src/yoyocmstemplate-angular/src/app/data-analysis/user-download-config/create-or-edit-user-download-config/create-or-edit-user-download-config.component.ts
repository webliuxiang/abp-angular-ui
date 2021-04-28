import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
  CreateOrUpdateUserDownloadConfigInput,
  UserDownloadConfigEditDto,
  UserDownloadConfigServiceProxy,
  ProductServiceProxy,
  KeyValuePairOfStringString,
  NameValueDto,
  CommonLookupServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { zip, BehaviorSubject } from 'rxjs';
import * as moment from 'moment';
import { debounceTime, map, switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-user-download-config',
  templateUrl: './create-or-edit-user-download-config.component.html',
  styleUrls: ['create-or-edit-user-download-config.component.less'],
})
export class CreateOrEditUserDownloadConfigComponent extends ModalComponentBase implements OnInit {
  /**
   * 编辑时DTO的id
   */
  id: any;

  entity: UserDownloadConfigEditDto = new UserDownloadConfigEditDto();
  products: KeyValuePairOfStringString[];

  startData: Date;
  searchChange$ = new BehaviorSubject('');
  userList: NameValueDto[] = [];
  userLoding = false;

  /**
   * 初始化的构造函数
   */
  constructor(
    injector: Injector,
    private _userDownloadConfigService: UserDownloadConfigServiceProxy,
    private _productService: ProductServiceProxy,
    private _commonLookupService: CommonLookupServiceProxy,
  ) {
    super(injector);
    this.startData = new Date();
  }

  ngOnInit(): void {
    this.init();
  }

  /**
   * 初始化方法
   */
  init(): void {
    zip(this._productService.getProducts(), this._userDownloadConfigService.getForEdit(this.id)).subscribe(
      ([productTypeResult, entityEditResult]) => {
        this.products = productTypeResult;
        this.entity = entityEditResult.userDownloadConfig;

        if (this.entity.startTime) { this.startData = this.entity.startTime.toDate(); }
      },
    );

    const getUserList = (name: string) =>
      this._commonLookupService
        .findUsersSetUserNameToValue({
          tenantId: undefined,
          filter: name,
          maxResultCount: 10,
          skipCount: 0,
        } as any)
        .pipe(
          map(list => {
            return list.items;
          }),
        );

    const optionList$ = this.searchChange$
      .asObservable()
      .pipe(debounceTime(500))
      .pipe(switchMap(getUserList));

    optionList$.subscribe(data => {
      this.userList = data;
      this.userLoding = false;
    });
  }

  /**
   * 搜索
   * @param value
   */
  onSearch(value: string): void {
    this.userLoding = true;
    this.searchChange$.next(value);
  }

  /**
   * 保存方法,提交form表单
   */
  submitForm(): void {
    const input = new CreateOrUpdateUserDownloadConfigInput();
    this.entity.startTime = moment(this.startData).utc(true);
    debugger;
    input.userDownloadConfig = this.entity;

    this.saving = true;

    this._userDownloadConfigService
      .createOrUpdate(input)
      .finally(() => (this.saving = false))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
      });
  }

  getProductTypeLocStr(label: string): string {
    return this.l(`ProductType${label}`);
  }
}
