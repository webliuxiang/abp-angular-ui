import { ChangeDetectionStrategy, Component, forwardRef, Injector, OnInit, SimpleChange, SimpleChanges, ViewChild } from '@angular/core';
import { PageFilterItemComponentBase } from '@shared/sample/common';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { Moment } from 'moment';
import * as moment from 'moment';

const defaultFormat = 'YYYY-MM-DD';

@Component({
  selector: 'sample-date',
  templateUrl: './sample-date.component.html',
  styleUrls: ['./sample-date.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SampleDateComponent),
    multi: true,
  }],
})
export class SampleDateComponent extends PageFilterItemComponentBase<string> {

  dateRange = [];

  defaultRanges: { [key: string]: Date[] } = {};

  argsObject = {
    type: undefined,
    placeholder: undefined,
    defaultRanges: [],
    default: undefined,
    allowClear: false,
    format: undefined
  };

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  onAfterViewInit(): void {
  }

  onArgsChange(args: any) {
    this.argsObject = args;
    if (this.argsObject.placeholder) {
      this.placeholder = this.l(this.argsObject.placeholder);
    } else {
      this.placeholder = this.l('PleaseSelect');
    }
    if (typeof (this.argsObject.allowClear) !== 'boolean') {
      this.argsObject.allowClear = true;
    }
    if (!this.argsObject.format) {
      this.argsObject.format = defaultFormat;
    }
    this.defaultRanges = {};
    if (Array.isArray(this.argsObject.defaultRanges)) {
      for (const item of this.argsObject.defaultRanges) {
        if (typeof (item) !== 'number') {
          continue;
        }
        const key = this.l('最近{0}天', item);
        this.defaultRanges[key] = [
          new Date(moment().subtract(item, 'days').startOf('day').format(defaultFormat) + ' 00:00:00'),
          new Date(moment().endOf('day').format(defaultFormat) + ' 23:59:59'),
        ];
      }
    }
    if (typeof (this.argsObject.default) === 'number') {
      const key = this.l('最近{0}天', this.argsObject.default);
      const range = this.defaultRanges[key];
      if (Array.isArray(range) && range.length === 2) {
        this.dateRange = [
          moment(range[0]),
          moment(range[1]),
        ];
        setTimeout(() => {
          this.onDateRangeChange(this.dateRange);
          this.cdr.detectChanges();
          this.imReady();
        }, 1000);
        return;
      }
    }

    this.cdr.detectChanges();
    this.imReady();
  }

  onExternalArgsChange(externalArgs: any) {

  }


  onDestroy(): void {
  }

  onInit(): void {
  }

  onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges) {
  }

  onDateRangeChange(e: Moment[]) {
    debugger;
    if (!Array.isArray(e) || e.length !== 2) {
      this.emitValueChange(undefined);
      return;
    }

    this.emitValueChange(
      this.processMoment(e),
    );
  }

  protected processMoment(input: Moment[]): string {
    return input[0].format(this.argsObject.format) + '|' + input[1].format(this.argsObject.format);
  }

}

