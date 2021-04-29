import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzModalRef } from 'ng-zorro-antd/modal';

// import { queryRulesArrBase, queryRulesArrBetween, queryRulesArrOneOf } from './query-rule-option';

@Component({
  selector: 'app-create-query-tag-modal',
  templateUrl: './create-query-tag-modal.component.html',
  styleUrls: ['./create-query-tag-modal.component.less'],
})
export class CreateQueryTagModalComponent implements OnInit {
  @Input() filterTerm;

  // 配置字段、筛选类型、筛选规则 表单
  baseForm: FormGroup;
  // 单值匹配 表单
  formPhraseFilter: FormGroup;
  // 多值匹配 表单
  formPhrasesFilter: FormGroup;
  // 范围匹配 表单
  formRangeFilter: FormGroup;
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
  // 选择的筛选规则
  selectFilterRule = '';

  constructor(private formBuilder: FormBuilder, private nzModalRef: NzModalRef) {}

  ngOnInit() {
    // 初始化表单控件
    this.baseForm = this.formBuilder.group({
      filterField: ['', Validators.required],
      queryType: ['must', Validators.required],
      queryRule: ['', Validators.required],
    });
    this.formPhraseFilter = this.formBuilder.group({
      filterValue: ['', Validators.required],
    });
    this.formPhrasesFilter = this.formBuilder.group({
      filterValue: [[], Validators.required],
    });
    this.formRangeFilter = this.formBuilder.group({
      filterStartValue: ['', Validators.required],
      filterEndValue: ['', Validators.required],
    });

    // 编辑状态
    if (this.filterTerm.is_editor) {
      this.baseForm.setValue({
        filterField: this.filterTerm.fields.find(item => {
          return JSON.stringify(this.filterTerm.queryField) === JSON.stringify(item);
        }),
        queryType: this.filterTerm.relation,
        queryRule: this.queryRules.find((item, key) => {
          return JSON.stringify(this.filterTerm.queryMethod) === JSON.stringify(item);
        }),
      });
      this.changeFilterRules(this.filterTerm.queryMethod);
      if (this.selectFilterRule === 'numeric_range') {
        this.formRangeFilter.patchValue({
          filterStartValue: this.filterTerm.queryContent[0],
          filterEndValue: this.filterTerm.queryContent[1],
        });
      } else if (this.selectFilterRule === 'phrase') {
        this.formPhraseFilter.patchValue({
          filterValue: this.filterTerm.queryContent,
        });
      } else {
        this.formPhraseFilter.patchValue({
          filterValue: '',
        });
      }
    }
  }

  /**
   * 选择查询方式后确定输入框类型
   * @param 查询方式
   */
  changeFilterRules(e) {
    if (e.value === 'exists') {
      this.selectFilterRule = 'exists';
    } else if (e.value === 'numeric_range' || e.value === 'date_range') {
      this.selectFilterRule = 'numeric_range';
    } else {
      this.selectFilterRule = 'phrase';
    }
  }

  ok() {
    const resultData = {
      queryField: this.baseForm.value.filterField,
      queryMethod: this.baseForm.value.queryRule,
      relation: this.baseForm.value.queryType,
      originRelation: this.baseForm.value.queryType,
      queryContent: null,
    };
    this.selectFilterRule === 'numeric_range'
      ? (resultData.queryContent = [
          this.formRangeFilter.value.filterStartValue,
          this.formRangeFilter.value.filterEndValue,
        ])
      : this.selectFilterRule === 'exists'
      ? (resultData.queryContent = '')
      : (resultData.queryContent = this.formPhraseFilter.value.filterValue);
    this.nzModalRef.close(resultData);
  }
  cancel() {
    this.nzModalRef.destroy();
  }
}
