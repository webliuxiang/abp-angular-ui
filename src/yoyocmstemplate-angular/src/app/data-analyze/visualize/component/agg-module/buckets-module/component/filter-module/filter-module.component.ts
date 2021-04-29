import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-filter-module',
  templateUrl: './filter-module.component.html',
  styleUrls: ['./filter-module.component.less'],
})
export class FilterModuleComponent implements OnInit {
  @Input() bucketData;
  filtersForm: FormGroup;

  listOfControl: Array<{ id: number; name: string; filterValue: string; filterLabel: string; showLabel: boolean }> = [];

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.filtersForm = this.fb.group({});
    // console.log(this.isArray(this.bucketData.bucketsSetting));

    if (this.bucketData.isEdit && this.isArray(this.bucketData.bucketsSetting)) {
      // console.log(this.bucketData);
      this.listOfControl = this.bucketData.listOfControl;
      this.listOfControl.map((item, key) => {
        // console.log(Object.keys(this.bucketData.bucketsSetting[key])[0]);
        if (item.showLabel) {
          this.filtersForm.addControl(
            item.filterLabel,
            new FormControl(Object.keys(this.bucketData.bucketsSetting[key])[0]),
          );
        } else {
          this.filtersForm.addControl(item.filterLabel, new FormControl(null));
        }
        this.filtersForm.addControl(
          item.filterValue,
          new FormControl(this.bucketData.bucketsSetting[key][Object.keys(this.bucketData.bucketsSetting[key])[0]]),
        );
      });
    } else {
      this.addField();
    }
    // console.log(this.filtersForm);
  }
  // 判断是否是数组
  isArray(o) {
    return Object.prototype.toString.call(o) == '[object Array]';
  }
  // 切换别名输入框显示
  checkShowLabel(
    i: { id: number; name: string; filterValue: string; filterLabel: string; showLabel: boolean },
    e: MouseEvent,
  ) {
    e.preventDefault();
    i.showLabel = !i.showLabel;
    // console.log(i);
  }
  // 添加
  addField(e?: MouseEvent): void {
    if (e) {
      e.preventDefault();
    }
    const id = this.listOfControl.length > 0 ? this.listOfControl[this.listOfControl.length - 1].id + 1 : 0;

    const control = {
      id,
      name: `条件 ${id + 1}`,
      label: `条件 ${id + 1} 的标签`,
      filterValue: `F_V${id + 1}`,
      filterLabel: `F_L${id + 1}`,
      showLabel: false,
    };
    const index = this.listOfControl.push(control);
    // console.log(this.listOfControl[this.listOfControl.length - 1]);
    this.filtersForm.addControl(this.listOfControl[index - 1].filterLabel, new FormControl(null));
    this.filtersForm.addControl(this.listOfControl[index - 1].filterValue, new FormControl(null, Validators.required));
  }
  // 删除
  removeField(
    i: { id: number; name: string; filterValue: string; filterLabel: string; showLabel: boolean },
    e: MouseEvent,
  ): void {
    e.preventDefault();
    if (this.listOfControl.length > 1) {
      const index = this.listOfControl.indexOf(i);
      this.listOfControl.splice(index, 1);
      // console.log(this.listOfControl);
      this.filtersForm.removeControl(i.filterValue);
      this.filtersForm.removeControl(i.filterLabel);
    }
  }
  // 构建
  buildBucketTermsOption() {
    for (const i in this.filtersForm.controls) {
      this.filtersForm.controls[i].markAsDirty();
      this.filtersForm.controls[i].updateValueAndValidity();
    }
    if (this.filtersForm.valid) {
      const arrTemp = [];
      let tempObj;
      this.bucketData.keywordsMapTransFormLabelAndValue = [];
      // { name: 'query_string' }
      this.listOfControl.map(item => {
        const objTemp = {};
        objTemp[
          item.showLabel && this.filtersForm.value[item.filterLabel] !== null
            ? this.filtersForm.value[item.filterLabel]
            : item.filterLabel
        ] = this.filtersForm.value[item.filterValue];
        arrTemp.push(objTemp);
      });
      // console.log(arrTemp);

      if (this.bucketData.isEdit) {
        this.bucketData.bucketsSetting = arrTemp;
        this.bucketData.listOfControl = this.listOfControl;
        tempObj = { ...this.bucketData };
      } else {
        tempObj = {
          bucketsSetting: arrTemp,
          listOfControl: this.listOfControl,
          ...this.bucketData,
        };
      }

      return tempObj;
    }
  }
}
