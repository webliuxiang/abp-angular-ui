import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({ name: '_moment' })
export class MomentFormatPipe implements PipeTransform {
  transform(value: moment.MomentInput, format?: string) {
    if (!value) {
      return '';
    }
    if (!format) {
      format = 'YYYY-MM-DD HH:mm:ss';
    }

    return moment(value).format(format);
  }
}
