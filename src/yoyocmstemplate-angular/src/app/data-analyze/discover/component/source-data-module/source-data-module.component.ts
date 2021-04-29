import { Component, Input, OnInit } from '@angular/core';
import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';

@Component({
  selector: 'app-source-data-module',
  templateUrl: './source-data-module.component.html',
  styleUrls: ['./source-data-module.component.less'],
})
export class SourceDataModuleComponent implements OnInit {
  @Input() resData;
  @Input() keywordsTimeDate;

  selectedKeys = [];
  dataTotal = 0;

  constructor(private _DataAnalyzeService: DataAnalyzeService) {}

  ngOnInit() {
    console.log(this.resData);
    this.dataTotal = this.resData.length;
    this._DataAnalyzeService.getSelectKeysMessage().subscribe(result => {
      this.selectedKeys = result;
    });
  }

  // 格式化日期
  parseDate(d) {
    if (d) {
      const foo = new Date(d);
      return foo.toLocaleString();
    } else {
      return '-';
    }
  }
}
