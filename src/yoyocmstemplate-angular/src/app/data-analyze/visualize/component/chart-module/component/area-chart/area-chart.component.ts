import { Component, OnInit, ElementRef, NgZone, ViewChild, Input } from '@angular/core';
import { TransformDataService } from '@app/data-analyze/transform-data.service';
import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';

@Component({
  selector: 'app-area-chart',
  templateUrl: './area-chart.component.html',
  styleUrls: ['./area-chart.component.less'],
})
export class AreaChartComponent implements OnInit {
  constructor(
    private _transformDataService: TransformDataService,
    private _aggregationDataService: DataAnalyzeService,
    public el1: ElementRef,
    private ngZone: NgZone,
  ) {}

  displayData = false;
  chartData = {
    tablerows: [],
    tableHeader: [],
  };
  chartConf = {
    allAxis: [],
    xAxis: '',
    type: '',
  };
  chartWidth = 500;
  chartCurrent = null;

  ngOnInit() {
    this._aggregationDataService.getChartMessage().subscribe(result => {
      this.chartData = this._transformDataService.transformData(result);
      this.chartConf.allAxis = this.chartData.tableHeader[1].allAxis;
      this.chartConf.xAxis = this.chartData.tableHeader[0].xAxis;
      this.chartConf.type = this.chartData.tableHeader[0].type;
      this.displayData = true;
      // console.log(this.chartData);
      if (this.chartCurrent !== null) {
        this.chartCurrent.destroy();
        this.render(this.el1);
      }
    });
  }

  render(el: ElementRef<HTMLDivElement>): void {
    setTimeout(() => {
      const styles = getComputedStyle(this.el1.nativeElement);
      this.chartWidth = parseFloat(styles.width.split('px')[0]);
      this.ngZone.runOutsideAngular(() => this.init(el.nativeElement));
    }, 200);
  }

  private init(el: HTMLElement) {
    // 在一行中保存多个城市的数据，需要将数据转换成
    // { month: 'Jan', city: 'Tokyo', temperature: 7.0 },
    // { month: 'Jan', Tokyo: 7.0, London: 3.9 } 该格式需要转换,
    // const dv = ds.createView().source(data);
    // fold 方式完成了行列转换，如果不想使用 DataSet 直接手工转换数据即可
    // dv.transform({
    //   type: 'fold',
    //   fields: ['Tokyo', 'London'], // 展开字段集
    //   key: 'city', // key字段
    //   value: 'temperature', // value字段
    // });

    const ds = new DataSet();
    // console.log(this.chartData);

    const dv = ds.createView().source(this.chartData.tablerows);
    // dv.transform({
    //   type: 'fold',
    //   fields: this.chartConf.allAxis, // 展开字段集
    //   key: 'key', // key字段
    //   value: 'value', // value字段
    // });

    const chart = new G2.Chart({
      container: el,
      forceFit: false,
      height: 700,
      width: this.chartWidth,
      padding: [20, 10, 200, 60],
    });
    chart.source(dv, {
      xAxis: {
        range: [0.02, 0.98],
        type: this.chartConf.type === 'date' ? 'time' : '',
        min: this.chartConf.type === 'noXAxis' ? 0 : null,
        max: this.chartConf.type === 'noXAxis' ? 1 : null,
        alias: this.chartConf.type === 'noXAxis' ? 'all_docs' : null,
        tickInterval: this.chartConf.type === 'noXAxis' ? 0.5 : null,
        formatter: (() => {
          return val => {
            if (this.chartConf.type === 'date') {
              const date = new Date(val * 1);
              return (
                date.getFullYear() +
                '-' +
                (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) +
                '-' +
                (date.getDate() < 10 ? '0' + date.getDate() : date.getDate()) +
                ' ' +
                (date.getHours() < 10 ? '0' + date.getHours() : date.getHours()) +
                ':' +
                (date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes()) +
                ':' +
                (date.getSeconds() < 10 ? '0' + date.getSeconds() : date.getSeconds())
              );
            } else {
              return val;
            }
          };
        })(),
      },
    });
    if (this.chartConf.type === 'noXAxis') {
      chart.axis('xAxis', {
        label: null,
        title: {
          offset: 20,
          textStyle: {
            fill: '#404040',
          },
        },
      });
    }
    chart.axis('value', {
      label: {
        formatter(text, item, index) {
          if (text / 1000 > 1000) {
            if (text / 1000 / 1000 > 1000) {
              return text / 1000 / 1000 / 1000 + 'B';
            } else {
              return text / 1000 / 1000 + 'M';
            }
          } else {
            return text / 1000 + 'K';
          }
        },
      },
    });
    chart
      .areaStack()
      .position('xAxis*value')
      .color('type');
    chart
      .lineStack()
      .position('xAxis*value')
      .color('type')
      .size(2);
    chart.tooltip({
      // offset: -10, // tooltip 距离鼠标的偏移量
      containerTpl:
        '<div class="g2-tooltip">' +
        '<div class="g2-tooltip-title" style="margin:10px 0;"></div>' +
        '<ul class="g2-tooltip-list" style="max-height:300px;overflow:auto;"></ul></div>', // tooltip 容器模板
      itemTpl:
        '<li data-index={index}><span style="background-color:{color};width:8px;height:8px;border-radius:50%;display:inline-block;margin-right:8px;"></span>{name}: {value}</li>', // tooltip 每项记录的默认模板
      inPlot: true,
      follow: true,
      // triggerOn: 'click',
      // enterable: true,
      // shared: true,
      // position: 'top',
    });
    chart.legend({
      position: 'bottom',
      offsetX: 10,
      useHtml: true, // 使用Html绘制图例
      flipPage: true, // 图例超出容器是否滚动
      containerTpl:
        '<div class="g2-legend" style="position:absolute;top:20px;width:auto;">' +
        '<h4 class="g2-legend-title"></h4>' +
        '<ul class="g2-legend-list" style="list-style-type:none;margin:0;padding:0;max-height:150px;overflow:auto;"></ul>' +
        '</div>', // 图例容器
      itemTpl:
        '<li class="g2-legend-list-item item-{index} {checked}" style="white-space:nowrap;text-overflow:ellipsis; overflow:hidden;" data-color="{originColor}" data-value="{originValue}">' +
        '<i class="g2-legend-marker" style="background-color:{color};"></i>' +
        '<span class="g2-legend-text" title="{value}">{value}</span></li>', // 图例
      // autoWrap: true,
    });
    chart.render();
    this.chartCurrent = chart;
  }
}
