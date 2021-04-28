import {
  AfterViewInit,
  ChangeDetectionStrategy, ChangeDetectorRef,
  Component,
  EventEmitter,
  Injector, Input,
  OnChanges,
  OnDestroy, OnInit, Output,
  SimpleChange,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { STChange, STColumn, STComponent, STMultiSort, STPage } from '@delon/abc';
import { ColumnItemDto, SortCondition, SortType } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/component-base';
import * as _ from 'lodash';
import { Subject } from 'rxjs';
import { SampleTableDataProcessorService } from './sample-table-data-processor.service';
import { ISampleTableAction } from './interfaces';

@Component({
  selector: 'sample-table',
  templateUrl: './sample-table.component.html',
  styleUrls: ['./sample-table.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SampleTableComponent extends AppComponentBase
  implements OnInit, AfterViewInit, OnChanges, OnDestroy {

  @Input() loading: boolean;

  /** 列表列配置 */
  @Input() columns: ColumnItemDto[] = [];

  /** 数据 */
  @Input() data: any[] = [];

  /** 列表配置 */
  @Input() page: STPage = {
    front: false,
    show: true,
    showQuickJumper: true,
    pageSizes: [10, 20, 30, 40, 50],
    showSize: true,
  };

  /** 虚拟滚动 */
  @Input() virtual = true;

  /** 滚动宽高 */
  @Input() scroll: { y?: string; x?: string; } = { x: '1800px', y: '240px' };

  /** 项高度 */
  @Input() virtualItemSize = 31;

  /** 页码 */
  @Input() pageIndex = 1;

  /** 页面数据量 */
  @Input() pageSize = 20;

  /** 页面数据分页条选项 */
  @Input() pageSizes = [10, 20, 30, 40, 50];

  /** 数据总量 */
  @Input() total: number;

  /** 边框 */
  @Input() bordered = true;

  /** 列被触发 */
  @Output() action = new EventEmitter<ISampleTableAction>();

  /** 列表排序事件 */
  @Output() sort = new EventEmitter<SortCondition[]>();

  /** 当check多选 */
  @Output() check = new EventEmitter<any[]>();

  /** 页面数据大小发生改变 */
  @Output() pageSizeChange = new EventEmitter<number>();

  /** 页码发生改变 */
  @Output() pageIndexChange = new EventEmitter<number>();

  /** 列表数据 */
  tableData: any[] = [];

  /** 列表配置 */
  tableColumns: STColumn[] = [
    {
      index: '',
      title: 'No',
      type: 'no',
      width: 40,
      fixed: 'left',
    },
  ];

  /** 排序配置 */
  sortData: STMultiSort = {
    key: 'sort',
    separator: '-',
    nameSeparator: '.',
    keepEmptyKey: true,
    global: true,
  };

  private destroy$ = new Subject();
  @ViewChild('st') stRef: STComponent;

  constructor(
    injector: Injector,
    private cdr: ChangeDetectorRef,
    private tableDataProcessor: SampleTableDataProcessorService,
  ) {
    super(injector);
  }

  ngOnInit(): void {

  }

  ngAfterViewInit(): void {

  }

  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
    if (changes.columns && changes.columns.currentValue) {
      this.processColumns(changes.columns.currentValue);
    }
    if (changes.data && changes.data.currentValue) {
      this.processDatas(changes.data.currentValue);
    }
    if (changes.scroll && changes.scroll.currentValue && this.stRef) {
      // 重新计算表格渲染内容
      const y = parseInt(changes.scroll.currentValue.y.toString().replace('px'));
      const timer = setInterval(() => {
        const tmpViewportSize = this.stRef.cdkVirtualScrollViewport.getViewportSize();
        if (tmpViewportSize === y) {
          this.cdr.detectChanges();
          clearInterval(timer);
        } else {
          this.stRef.cdkVirtualScrollViewport.checkViewportSize();
        }
      }, 800);
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /** 当 action 项被点击 */
  onActionClick(action: string, record: any) {
    this.action.emit({
      name: action,
      // tslint:disable-next-line: object-literal-shorthand
      record: record,
    });
  }

  /** 表格事件 */
  onTableChange(evnet: STChange) {
    switch (evnet.type) {
      case 'checkbox': // 多选
        const checked = evnet.checkbox;
        this.check.emit(checked);
        break;
      case 'radio': // 单选
        this.check.emit([evnet.radio]);
        break;
      case 'sort': // 排序
        this.updateSort(evnet);
        break;
      case 'pi': // page index 修改
        this.pageIndexChange.emit(evnet.pi);
        break;
      case 'ps': // page size 修改
        this.pageSizeChange.emit(evnet.ps);
        break;
    }
  }

  /** 更新排序 */
  protected updateSort(evnet: STChange) {
    if (evnet.type !== 'sort') {
      return;
    }

    const sortConditions: SortCondition[] = [];
    const sorts = evnet.sort.map.sort.split('-');
    let sortField: string;
    let sortType: SortType = SortType.None;
    let index = 0;
    for (const sort of sorts) {
      const lastIndex = sort.lastIndexOf('.');
      sortField = sort.substring(0, lastIndex);
      if (sortField.trim() === '') {
        continue;
      }

      if (sort.endsWith('.descend')) {
        sortType = SortType.Desc;
      } else if (sort.endsWith('.ascend')) {
        sortType = SortType.Asc;
      }

      sortConditions.push(
        new SortCondition({
          field: sortField,
          order: index++,
          type: sortType,
        }),
      );
    }

    this.sort.emit(sortConditions);
  }

  /** 处理列表信息 */
  protected processColumns(input: ColumnItemDto[]) {
    // {
    //   "field": "", // 字段,支持嵌套 aa.bb
    //   "title": "No", // 列名
    //   "type": "no", // 类型
    //   "render": null, // 要使用的渲染器名称
    //   "width": 40, // 宽度
    //   "order": 0, // 排序号
    //   "numberDigits": 2, // 列类型为number时保留小数位
    //   "dateFormat": null, // 列类型为datetime类型格式化规则
    //   "statistical": null, // 统计类型
    //   "fixed": "left" // 固定列 left或right,必须指定width
    // },
    this.tableColumns = [{
      index: '',
      title: 'No',
      type: 'no',
      width: 40,
      fixed: 'left',
    }];
    if (!input || input.length === 0) {
      this.cdr.detectChanges();
      return;
    }

    this.tableColumns = this.tableDataProcessor.processCols(input);
    this.cdr.detectChanges();
  }

  /*** 处理数据 */
  protected processDatas(input: any[]) {
    this.tableData = [];
    if (!input || input.length === 0) {
      this.cdr.detectChanges();
      return;
    }
    this.tableData = this.tableDataProcessor.processData<any>(input, this.columns);
    this.cdr.detectChanges();
  }
}
