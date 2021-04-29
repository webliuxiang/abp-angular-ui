import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import { CreateQueryTagModalComponent } from './create-query-tag-modal/create-query-tag-modal.component';
import { DataAnalyzeService } from '@app/data-analyze/data-analyze.service';

@Component({
  selector: 'app-query-module',
  templateUrl: './query-module.component.html',
  styleUrls: ['./query-module.component.less'],
})
export class QueryModuleComponent implements OnInit {
  @Input() keywordsMap;
  @Input() chartSetting;
  @Input() queryData;
  @Output() querySearch = new EventEmitter();
  queryString = '';
  // filterTags
  filterTags = [];
  // 筛选规则字段
  queryRules = [
    {
      value: 'term',
      label: '按关键字匹配(term)',
    },
    {
      value: 'exists',
      label: '按字段存在匹配(exists)',
    },
    {
      value: 'wildcard',
      label: '按通配符匹配(wildcard)',
    },
    {
      value: 'numeric_range',
      label: '按数值区间匹配(numeric_range)',
    },
    {
      value: 'date_range',
      label: '按日期区间匹配(date_range)',
    },
    {
      value: 'match',
      label: '按文本内容匹配(match)',
    },
    {
      value: 'query_string',
      label: '按lucene表达式匹配(query_string)',
    },
    {
      value: 'scripted',
      label: '按脚本执行结果匹配(scripted)',
    },
  ];

  constructor(private _modal: ModalHelper, private _aggregationDataService: DataAnalyzeService) {}

  ngOnInit() {
    if (this.queryData.isEdit) {
      // console.log(this.queryData);
      if (this.queryData.data.must.length > 0) {
        this.queryData.data.must.map(item => {
          if (item.type === 'exists') {
            this.filterTags.push({
              queryField: this.keywordsMap.find(keywordsMapItem => {
                return keywordsMapItem.name === item.field;
              }),
              queryMethod: this.queryRules.find(queryRulesItem => {
                return queryRulesItem.value === item.type;
              }),
              relation: 'must',
              originRelation: 'must',
            });
          } else if (item.type === 'date_range' || item.type === 'numeric_range') {
            this.filterTags.push({
              queryField: this.keywordsMap.find(keywordsMapItem => {
                return keywordsMapItem.name === item.field;
              }),
              queryMethod: this.queryRules.find(queryRulesItem => {
                return queryRulesItem.value === item.type;
              }),
              relation: 'must',
              originRelation: 'must',
              queryContent: [item.gte, item.lte],
            });
          } else {
            this.filterTags.push({
              queryField: this.keywordsMap.find(keywordsMapItem => {
                return keywordsMapItem.name === item.field;
              }),
              queryMethod: this.queryRules.find(queryRulesItem => {
                return queryRulesItem.value === item.type;
              }),
              relation: 'must',
              originRelation: 'must',
              queryContent: item.query || item.value,
            });
          }
        });
      }
      if (this.queryData.data.must_not.length > 0) {
        this.queryData.data.must_not.map(item => {
          if (item.type === 'exists') {
            this.filterTags.push({
              queryField: this.keywordsMap.find(keywordsMapItem => {
                return keywordsMapItem.name === item.field;
              }),
              queryMethod: this.queryRules.find(queryRulesItem => {
                return queryRulesItem.value === item.type;
              }),
              relation: 'must_not',
              originRelation: 'must_not',
            });
          } else if (item.type === 'date_range' || item.type === 'numeric_range') {
            this.filterTags.push({
              queryField: this.keywordsMap.find(keywordsMapItem => {
                return keywordsMapItem.name === item.field;
              }),
              queryMethod: this.queryRules.find(queryRulesItem => {
                return queryRulesItem.value === item.type;
              }),
              relation: 'must_not',
              originRelation: 'must_not',
              queryContent: [item.gte, item.lte],
            });
          } else {
            this.filterTags.push({
              queryField: this.keywordsMap.find(keywordsMapItem => {
                return keywordsMapItem.name === item.field;
              }),
              queryMethod: this.queryRules.find(queryRulesItem => {
                return queryRulesItem.value === item.type;
              }),
              relation: 'must_not',
              originRelation: 'must_not',
              queryContent: item.query || item.value,
            });
          }
        });
      }
      if (this.queryData.data.should.length > 0) {
        this.queryData.data.should.map(item => {
          if (item.type === 'exists') {
            this.filterTags.push({
              queryField: this.keywordsMap.find(keywordsMapItem => {
                return keywordsMapItem.name === item.field;
              }),
              queryMethod: this.queryRules.find(queryRulesItem => {
                return queryRulesItem.value === item.type;
              }),
              relation: 'should',
              originRelation: 'should',
            });
          } else if (item.type === 'date_range' || item.type === 'numeric_range') {
            this.filterTags.push({
              queryField: this.keywordsMap.find(keywordsMapItem => {
                return keywordsMapItem.name === item.field;
              }),
              queryMethod: this.queryRules.find(queryRulesItem => {
                return queryRulesItem.value === item.type;
              }),
              relation: 'should',
              originRelation: 'should',
              queryContent: [item.gte, item.lte],
            });
          } else {
            this.filterTags.push({
              queryField: this.keywordsMap.find(keywordsMapItem => {
                return keywordsMapItem.name === item.field;
              }),
              queryMethod: this.queryRules.find(queryRulesItem => {
                return queryRulesItem.value === item.type;
              }),
              relation: 'should',
              originRelation: 'should',
              queryContent: item.query || item.value,
            });
          }
        });
      }
      // this.filterTags = [];
      // this.queryString = this.queryData.data.default_query;
    }
    // this._aggregationDataService.getQueryMessage().subscribe(result => {
    //   this.filterTags = result.filterTags;
    //   this.queryString = result.queryString;
    // });
  }
  // 显示弹出框
  showInput(haveSelectDatasource): void {
    if (haveSelectDatasource === null) {
      return;
    }
    const filterTerm = {
      is_editor: false,
      fields: this.keywordsMap,
    };
    this._modal
      .open(CreateQueryTagModalComponent, { filterTerm }, 'md', { nzMaskClosable: false })
      .subscribe((res: any) => {
        if (res) {
          // console.log(res);
          this.filterTags.push(res);
        }
      });
  }

  // 删除查询条件
  handleClose(removedTag: {}): void {
    this.filterTags = this.filterTags.filter(tag => tag !== removedTag);
  }
  // 编辑查询条件
  editorFilter(tag, index) {
    const filterTerm = {
      is_editor: true,
      fields: this.keywordsMap,
      ...tag,
    };
    this._modal
      .open(CreateQueryTagModalComponent, { filterTerm }, 'md', { nzMaskClosable: false })
      .subscribe((res: any) => {
        if (res) {
          // console.log(res);
          this.filterTags.splice(index, 1, res);
          // TODO: 数据交互
        }
      });
  }

  // 切换查询条件
  shiftRelation(tag, i) {
    if (tag.relation === 'must') {
      tag.relation = 'should';
    } else if (tag.relation === 'must_not') {
      tag.relation = 'must';
    } else if (tag.relation === 'should') {
      tag.relation = 'must_not';
    }
  }

  // 查询数据
  query() {
    const objTemp = {
      queryString: this.queryString,
      filterTags: this.filterTags,
    };
    this.querySearch.next(objTemp);
  }
  // 保存配置数据
  save() {
    const objTemp = {
      queryString: this.queryString,
      filterTags: this.filterTags,
    };
    return objTemp;
  }
}
