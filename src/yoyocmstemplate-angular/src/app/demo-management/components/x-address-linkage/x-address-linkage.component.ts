import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { AddressLinkageServiceProxy, AddressEnum } from '@shared/service-proxies/service-proxies';

// 省
const provinces = [];
// 市
const cities = [];
// 县
const areas = [];
// 镇、乡
const street = [];

@Component({
  selector: 'x-address-linkage',
  templateUrl: './x-address-linkage.component.html',
  styles: []
})
/**
 * 省市县镇组件
 */
export class XAddressLinkageComponent extends AppComponentBase implements OnInit {

  constructor(
    injector: Injector,
    private _addressLinkageServiceProxy: AddressLinkageServiceProxy,
  ) {
    super(injector);
  }

  @Input() name: string;

  @Input()
  placeHolder = '所在地区';  // 文本框展示的内容

  @Input()
  xDisabled = false;   // 禁用

  @Input()
  isClickOnLoad = false; // ture 点击加载 ；false：鼠标移动加载

  @Output()
  selectCodeChange = new EventEmitter<string[] | number[]>();  // 选择后回调事件

  @Output()
  selectNameChange = new EventEmitter<string[]>(); // 选中的文本

  values: any[] | null = null;

  ngOnInit(): void {
    this.getData();
  }


  /**
   * 获取所有数据
   */
  getData() {
    this._addressLinkageServiceProxy.getAll()
      .subscribe(item => {
        item.provinces.forEach(element => {
          provinces.push({ value: element.code, label: element.name });
        });
        item.citys.forEach(element => {
          cities.push({ value: element.code, label: element.name, provinceCode: element.provinceCode });
        });
        item.areas.forEach(element => {
          areas.push({ value: element.code, label: element.name, provinceCode: element.provinceCode, cityCode: element.cityCode });
        });
        item.streets.forEach(element => {
          street.push({ value: element.code, label: element.name, provinceCode: element.provinceCode, cityCode: element.cityCode, areaCode: element.areaCode, isLeaf: true });
        });
      });

    // this._addressLinkageServiceProxy.get(AddressEnum._0, "")
    //   .subscribe(item => {
    //     item.forEach(element => {
    //       provinces.push({ value: element.code, label: element.name });
    //     });
    //   })
    // this._addressLinkageServiceProxy.getAllCity()
    //   .subscribe(item => {
    //     item.forEach(element => {
    //       cities.push({ value: element.code, label: element.name, provinceCode: element.provinceCode });
    //     });
    //   })
    // this._addressLinkageServiceProxy.getAllArea()
    //   .subscribe(item => {
    //     item.forEach(element => {
    //       areas.push({ value: element.code, label: element.name, provinceCode: element.provinceCode, cityCode: element.cityCode });
    //     });
    //   })
    // this._addressLinkageServiceProxy.getAllStreet()
    //   .subscribe(item => {
    //     item.forEach(element => {
    //       street.push({ value: element.code, label: element.name, provinceCode: element.provinceCode, cityCode: element.cityCode, areaCode: element.areaCode, isLeaf: true });
    //     });
    //   })
  }

  onChanges(values: any): void {
    this.selectCodeChange.emit(this.values);
  }

  onSelectionChange(selectedOptions: any[]): void {
    let ret: string[] = [];
    ret = selectedOptions.map(o => o.label);
    this.selectNameChange.emit(ret);
  }


  /** load data async execute by `nzLoadData` method */
  loadData(node: any, index: number): PromiseLike<any> {
    return new Promise(resolve => {
      setTimeout(() => {
        if (index < 0) {
          node.children = provinces;
        } else if (index === 0) {
          const selectCitys = cities.filter(x => x.provinceCode === node.value);
          node.children = selectCitys;
        } else if (index === 1) {
          const selectAreas = areas.filter(x => x.cityCode === node.value);
          node.children = selectAreas;
        } else {
          const selectStreet = street.filter(x => x.areaCode == node.value);
          node.children = selectStreet;
        }
        resolve();
      }, 500);
    });
  }
}
