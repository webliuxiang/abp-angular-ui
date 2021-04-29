import { Component, OnInit, ElementRef, NgZone, ViewChild, Input } from '@angular/core';
import { TransformDataService } from '@app/data-analyze/transform-data.service';
import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.less'],
})
export class PieChartComponent implements OnInit {
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
  };
  chartWidth = 500;
  chartCurrent = null;

  ngOnInit() {
    this._aggregationDataService.getChartMessage().subscribe(result => {
      this.chartData = this._transformDataService.transformData(result);
      this.chartConf.allAxis = this.chartData.tableHeader[1].allAxis;
      this.chartConf.xAxis = this.chartData.tableHeader[0].xAxis;
      this.displayData = true;
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
    const dv = ds.createView();

    // console.log(this.chartData);

    dv.source(this.chartData.tablerows).transform({
      type: 'percent',
      field: this.chartConf.xAxis,
      dimension: this.chartConf.allAxis[0].toString(),
      as: 'percent',
    });
    // dv.source(data).transform({
    //   type: 'percent',
    //   field: 'value',
    //   dimension: 'type',
    //   as: 'percent',
    // });
    const chart = new G2.Chart({
      container: el,
      forceFit: false,
      height: 700,
      width: this.chartWidth,
      padding: [20, 10, 200, 60],
    });
    chart.source(dv);
    chart.coord('theta', {
      radius: 0.8,
      innerRadius: 0.7 / 0.8,
    });
    chart
      .intervalStack()
      .position('percent')
      .color(this.chartConf.allAxis[0])
      .tooltip(`${this.chartConf.allAxis[0]}*percent`, (item, percent) => {
        percent = (percent * 100).toFixed(2) + '%';
        return {
          name: item,
          value: percent,
        };
      })
      .style({
        stroke: 'white',
        lineWidth: 1,
      });
    chart.tooltip({
      showTitle: false,
    });
    chart.legend({
      position: 'bottom',
      // offsetX: 10,
      useHtml: true, // 使用Html绘制图例
      // flipPage: true, //图例超出容器是否滚动
      containerTpl:
        '<div class="g2-legend" style="position:absolute;top:20px;width:auto;max-height:100px;">' +
        '<h4 class="g2-legend-title"></h4>' +
        '<ul class="g2-legend-list" style="max-height:100px;list-style-type:none;margin:0;padding:0;max-height:150px;min-width:100px;overflow:auto;"></ul>' +
        '</div>', // 图例容器
      itemTpl:
        '<li class="g2-legend-list-item item-{index} {checked}" style="white-space:nowrap;text-overflow:ellipsis; overflow:hidden;" data-color="{originColor}" data-value="{originValue}">' +
        '<i class="g2-legend-marker" style="background-color:{color};"></i>' +
        '<span class="g2-legend-text" title="{value}">{value}</span></li>', // 图例
    });

    for (let index = 1; index < this.chartConf.allAxis.length; index++) {
      const element = this.chartConf.allAxis[index];

      const outterView = chart.view();
      const ds2 = new DataSet();
      const dv2 = ds2.createView();
      dv2.source(this.chartData.tablerows).transform({
        type: 'percent',
        field: this.chartConf.xAxis,
        dimension: element,
        as: 'percent',
      });
      outterView.source(dv2);
      outterView.coord('theta', {
        radius: 0.9 + 0.1 * (index - 1),
        innerRadius: (0.8 + 0.1 * (index - 1)) / (0.9 + 0.1 * (index - 1)),
      });
      outterView
        .intervalStack()
        .position('percent')
        .color(element)
        .tooltip(`${element}*percent`, (item, percent) => {
          percent = (percent * 100).toFixed(2) + '%';
          return {
            name: item,
            value: percent,
          };
        })
        .style({
          stroke: 'white',
          lineWidth: 1,
        });
    }

    // const outterView3 = chart.view();
    // const ds3 = new DataSet();
    // const dv3 = ds3.createView();
    // dv3.source(data).transform({
    //   type: 'percent',
    //   field: 'value',
    //   dimension: 'test',
    //   as: 'percent',
    // });
    // outterView3.source(dv3);
    // outterView3.coord('theta', {
    //   radius: 1,
    //   innerRadius: 0.9 / 1,
    // });
    // outterView3
    //   .intervalStack()
    //   .position('percent')
    //   .color('test', ['#1890ff', '#13c2c2', '#ffc53d', '#73d13d'])
    //   .style({
    //     stroke: 'white',
    //     lineWidth: 1,
    //   });

    chart.render();
    this.chartCurrent = chart;
  }
}
