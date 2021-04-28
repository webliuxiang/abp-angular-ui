import { CommonLookupServiceProxy, PagedResultDtoOfNameValueDto, NameValueDto, ProductSecretKeyBindToUserInput } from '@shared/service-proxies/service-proxies';
import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { ProductSecretKeyServiceProxy, BatchCreateProductSecretKeyInput, ProductServiceProxy } from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { debounceTime, map, switchMap, finalize } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';

@Component({
  selector: 'app-bind-product-secret-key-to-user',
  templateUrl: './bind-product-secret-key-to-user.component.html',
  styleUrls: [
    './bind-product-secret-key-to-user.component.less'
  ]
})
export class BindProductSecretKeyToUserComponent extends ModalComponentBase
  implements OnInit {

  @Input() secretKey: string;
  searchChange$ = new BehaviorSubject('');
  userList: NameValueDto[] = [];
  userLoding = false;
  input: ProductSecretKeyBindToUserInput = new ProductSecretKeyBindToUserInput();

  /**
  * 初始化的构造函数
  */
  constructor(
    injector: Injector,
    private _productSecretKeyService: ProductSecretKeyServiceProxy,
    private _commonLookupService: CommonLookupServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.init();
  }




  /**
  * 初始化方法
  */
  init(): void {
    const getUserList = (name: string) =>
      this._commonLookupService.findUsersSetUserNameToValue({
        tenantId: undefined,
        filter: name,
        maxResultCount: 10,
        skipCount: 0,
      } as any).pipe(map((list) => {
        return list.items;
      }));

    const optionList$ = this.searchChange$.asObservable()
      .pipe(debounceTime(500))
      .pipe(switchMap(getUserList));

    optionList$.subscribe(data => {
      this.userList = data;
      this.userLoding = false;
    });

    // 设置当前的卡密
    this.input.secretKey = this.secretKey;
    this.input.money = 0;
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
    this.saving = true;
    const self = this;

    setTimeout(() => {
      self._productSecretKeyService.bindToUser(self.input)
        .pipe(finalize(() => (self.saving = false)))
        .subscribe(() => {
          self.notify.success(self.l('SavedSuccessfully'));
          self.success(true);
        });
    }, 50);


  }
}
