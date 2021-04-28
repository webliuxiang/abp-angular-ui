import { Injectable, Injector } from '@angular/core';
import { STColumn } from '@delon/abc';
import { ColumnItemDto, ColumnItemFixed, ColumnItemStatistical } from '@shared/service-proxies/service-proxies';
import { SampleComponentBase } from '@shared/component-base';
import * as _ from 'lodash';
import * as moment from 'moment';
import { SampleTableConsts } from '@shared/sample/table/sample-table-consts';


@Injectable()
export class SampleTableDataProcessorService extends SampleComponentBase {

  constructor(
    injector: Injector,
  ) {
    super(injector);

    // 初始化
    SampleTableConsts.init((key) => {
      return this.l(key);
    });
  }

  /** 处理列表配置 */
  processCols(cols: ColumnItemDto[]): STColumn[] {
    if (!cols) {
      return [];
    }

    const newCols = cols.sort((a, b) => a.order - b.order);
    const result: STColumn[] = [];
    for (const item of newCols) {
      const newItem: STColumn = {
        index: item.field.split('.'),
        title: this.l(item.title),
        render: item.render,
        width: item.width,
        sort: true,
      };
      if (item.type === 'yn-tag') {
        newItem.type = 'tag';
        newItem.tag = SampleTableConsts.ynTag;
      } else if (item.type === 'yn') {
        newItem.type = 'badge';
        newItem.badge = SampleTableConsts.ynBadge;
      } else if (item.type && item.type !== '') {
        newItem.type = item.type.toLowerCase() as any;
        if (newItem.type === 'no'
          || newItem.type === 'checkbox'
          || newItem.type === 'radio'
          || newItem.type === 'img') {
          newItem.sort = undefined;
        } else if (newItem.type === ('action' as any)) {
          newItem.type = undefined;
          newItem.sort = undefined;
          newItem.index = 'actions';
          newItem.render = 'actions';
        } else if (newItem.type === ('ss' as any)
          || newItem.type === ('ms' as any)
          || newItem.type === ('byte' as any)) {
          newItem.type = undefined;
        }
      }


      if (item.statistical && item.statistical !== ColumnItemStatistical.None) {
        newItem.statistical = item.statistical.toString().toLowerCase() as any;
      }
      if (item.fixed && item.fixed !== ColumnItemFixed.None) {
        newItem.fixed = item.fixed.toString().toLowerCase() as any;
      }
      result.push(newItem);
    }

    return result;
  }

  /** 处理列表数据 */
  processData<T>(data: T[], cols: ColumnItemDto[]): T[] {
    if (!data || !cols) {
      return [];
    }
    const result: T[] = [];

    let actions;
    // 遍历表格数据
    for (const item of data) {
      // 复制一份新数据
      const newItem = _.cloneDeep(item) as any;
      // 设置原始数据
      newItem.original = item;
      result.push(newItem);

      // 遍历列配置
      for (const col of cols) {
        // 字段描述
        const fields = col.field.split('.');

        // 判断类型,跳过字段处理
        if (!col.type
          || col.type === 'checkbox'
          || col.type === 'no') {
          continue;
        }

        if (col.type === 'action') {
          if (!actions) {
            actions = col.actions;
          }
          newItem.actions = actions;
          continue;
        }

        // 获取字段值,如果为空或NaN 则跳过格式化步骤
        let fieldValue = this.getFieldValue(newItem, fields);
        if (typeof (fieldValue) !== 'boolean') {
          if (isNaN(fieldValue)
            || typeof (fieldValue) === 'undefined'
            || (!fieldValue && fieldValue !== 0)) {
            continue;
          }
        }


        // 保留小小数点位数
        const numberDigits = typeof (col.numberDigits) === 'number' ? col.numberDigits : 2;

        // 根据类型格式化字段值,并处理根据执行结果决定需要重新给字段设置值
        let needSetValue = false;
        switch (col.type) {
          case 'datetime': // 时间类型
            if (col.dateFormat && col.dateFormat.trim() !== '') {
              if (fieldValue instanceof moment) {
                fieldValue = (fieldValue as moment.Moment).format(col.dateFormat);
              } else {
                fieldValue = moment(fieldValue).format(col.dateFormat);
              }
              needSetValue = true;
            }
            break;
          case 'number': // number类型列，处理小数点
            if (typeof (fieldValue) === 'number') {
              fieldValue = fieldValue.toFixed(numberDigits);
            } else {
              fieldValue = parseFloat(fieldValue).toFixed(numberDigits);
            }
            needSetValue = true;
            break;
          case 'yn':
          case 'yn-tag':
            if (typeof (fieldValue) === 'boolean') {
              fieldValue = fieldValue.toString();
            } else {
              fieldValue = '';
            }
            needSetValue = true;
            break;
          case 'byte': // 字节类型
            if (typeof (fieldValue) === 'number') {
              fieldValue = this.formatBytes(fieldValue);
            } else {
              fieldValue = this.formatBytes(parseFloat(fieldValue));
            }
            needSetValue = true;
            break;
          case 'ss': // 秒类型
            if (typeof (fieldValue) === 'number') {
              fieldValue = this.formatSeconds(fieldValue);
            } else {
              fieldValue = this.formatSeconds(parseFloat(fieldValue));
            }
            needSetValue = true;
            break;
          case 'ms': // 秒类型
            if (typeof (fieldValue) === 'number') {
              fieldValue = this.formatSeconds(fieldValue / 1000);
            } else {
              fieldValue = this.formatSeconds(parseFloat(fieldValue) / 1000);
            }
            needSetValue = true;
            break;
        }

        // 更新字段值
        if (needSetValue) {
          this.setFieldValue(newItem, fields, fieldValue);
        }
      }
    }

    return result;
  }

  /** 获取字段值 */
  private getFieldValue(data: any, fields: string | string[]): any {
    if (!data) {
      console.debug('getFieldValue 数据为空');
      return undefined;
    }
    if (!fields) {
      console.debug('getFieldValue 列表配置字段为空');
      return undefined;
    }
    if (!Array.isArray(fields)) {
      return data[fields];
    }
    let resultData = data;
    for (const field of fields) {
      resultData = this.getFieldValue(resultData, field);
      if (!data) {
        return undefined;
      }
    }
    return resultData;
  }

  /** 给字段设置值 */
  private setFieldValue(data: any, fields: string | string[], val: any): boolean {
    if (!data) {
      console.debug('setFieldValue 数据为空');
      return undefined;
    }
    if (!fields) {
      console.debug('setFieldValue 列表配置字段为空');
      return undefined;
    }

    if (!Array.isArray(fields)) {
      data[fields] = val;
      return false;
    }

    if (fields.length === 1) {
      this.setFieldValue(data, fields[0], val);
      return false;
    }


    const max = fields.length - 1;
    const lastField = fields[max];
    let resultData = data;

    for (let i = 0; i < max; i++) {
      if (!resultData) {
        console.debug(`给字段设置值失败，调用链存在空数据，异常调用链: ${fields[i - 1]}  原始调用链：`);
        console.debug(fields);
        return false;
      }
      const field = fields[i];
      resultData = resultData[field];
    }

    resultData[lastField] = val;
    return true;
  }

  /** 转换秒数 */
  private formatSeconds(value) {
    let theTime = parseInt(value); // 秒
    let middle = 0; // 分
    let hour = 0; // 小时

    if (theTime > 60) {
      middle = parseInt((theTime / 60) + '');
      theTime = parseInt((theTime % 60) + '');
      if (middle > 60) {
        hour = parseInt((middle / 60) + '');
        middle = parseInt((middle % 60) + '');
      }
    }
    let result = '' + parseInt((theTime) + '') + this.l('Second');
    if (middle > 0) {
      result = '' + parseInt((middle) + '') + this.l('Minute') + result;
    }
    if (hour > 0) {
      result = '' + parseInt((hour) + '') + this.l('Hour') + result;
    }
    return result;
  }

  /** 转换字节 */
  private formatBytes(limit) {
    let size = '';
    if (limit < 0.1 * 1024) { // 如果小于0.1KB转化成B
      size = limit.toFixed(2) + 'B';
    } else if (limit < 0.1 * 1024 * 1024) {// 如果小于0.1MB转化成KB
      size = (limit / 1024).toFixed(2) + 'KB';
    } else if (limit < 0.1 * 1024 * 1024 * 1024) { // 如果小于0.1GB转化成MB
      size = (limit / (1024 * 1024)).toFixed(2) + 'MB';
    } else { // 其他转化成GB
      size = (limit / (1024 * 1024 * 1024)).toFixed(2) + 'GB';
    }

    const sizestr = size + '';
    const len = sizestr.indexOf('\.');
    const dec = sizestr.substr(len + 1, 2);
    if (dec == '00') {// 当小数点后为00时 去掉小数部分
      return sizestr.substring(0, len) + sizestr.substr(len + 3, 2);
    }
    return sizestr;
  }
}
