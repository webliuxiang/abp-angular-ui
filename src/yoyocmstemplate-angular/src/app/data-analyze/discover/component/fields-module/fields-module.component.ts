import { Component, Input, OnInit } from '@angular/core';
import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';

@Component({
  selector: 'app-fields-module',
  templateUrl: './fields-module.component.html',
  styleUrls: ['./fields-module.component.less'],
})
export class FieldsModuleComponent implements OnInit {
  @Input() keywordsList;
  @Input() chartSetting;
  @Input() resData;

  selectedKeys = [];
  topArr = [];
  isCollapsed = false;

  constructor(private _DataAnalyzeService: DataAnalyzeService) {}

  ngOnInit() {}
  showPopover(e) {
    let countArr = [];
    this.resData.map(item => {
      const temp = item.originData[e.name];
      const tempTypeObj = item._source.find(i => {
        return i.key === e.name;
      });
      if (tempTypeObj !== undefined && tempTypeObj.type === 'object') {
        item.originData[tempTypeObj.parentName.split('.')[0]].map(itemObj => {
          if (itemObj[e.name.split(tempTypeObj.parentName)[1]]) {
            countArr.push(itemObj[e.name.split(tempTypeObj.parentName)[1]]);
          }
        });
      } else if (temp === undefined) {
        return;
      } else {
        countArr.push(temp);
      }
    });
    countArr = [].concat(...countArr);
    function countTimes(data) {
      return data.reduce(function(time, name) {
        if (name in time) {
          time[name]++;
        } else {
          time[name] = 1;
        }

        return time;
      }, {});
    }
    const tempObj = countTimes(countArr);
    const resultArr = [];
    for (const i in tempObj) {
      if (tempObj.hasOwnProperty(i)) {
        const element = tempObj[i];
        resultArr.push({
          key: i,
          value: element,
        });
      }
    }

    function compare(params: string) {
      return function(objA: object, objB: object) {
        const valA = objA[params];
        const valB = objB[params];
        return valB - valA;
      };
    }
    this.topArr = resultArr.sort(compare('value')).slice(0, 5);
  }
  onSelectKey(e, key) {
    if (e) {
      this.selectedKeys.push(key.name);
      this.selectedKeys.sort();
    } else {
      this.selectedKeys.splice(this.selectedKeys.indexOf(key.name), 1);
    }
    console.log(this.selectedKeys);
    this._DataAnalyzeService.sendSelectKeysMessage(this.selectedKeys);
  }
}
