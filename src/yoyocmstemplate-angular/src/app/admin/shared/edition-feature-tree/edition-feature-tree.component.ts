import { Component, Injector } from '@angular/core';
import { FlatFeatureDto, NameValueDto, FeatureInputTypeDto } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';
import { NzTreeNode } from 'ng-zorro-antd/tree';
import { ArrayService } from '@delon/util';
import { AppComponentBase } from '@shared/component-base';
import { TreeDataHelperService, ArrayToTreeConverterService } from '@shared/utils';
import { FeatureTreeEditModel } from './types';

@Component({
  selector: 'edition-feature-tree',
  templateUrl: './edition-feature-tree.component.html',
  styleUrls: ['./edition-feature-tree.component.less'],
})
export class EditionFeatureTreeComponent extends AppComponentBase {
  /**
   * 功能源数据
   */
  _featureSourceData: FeatureTreeEditModel;

  /**
   * 树数据
   */
  treeData: NzTreeNode[] = [];

  /**
   * 编辑用的数据
   */
  set editData(val: FeatureTreeEditModel) {
    this._featureSourceData = val;
    this.processsTreeData();
  }

  constructor(private arrayService: ArrayService, injector: Injector) {
    super(injector);
  }

  /**
   * 将数据转换成树
   */
  processsTreeData() {
    this.treeData = this.arrayService.arrToTreeNode(this._featureSourceData.features, {
      idMapName: 'name',
      parentIdMapName: 'parentName',
      titleMapName: 'displayName',
    });

    this.arrayService.visitTree(this.treeData, item => {
      item.isLeaf = true;
      this.fullTreeData(item, item.origin.name);
    });
  }

  /**
   * 填充数据到节点
   * @param node
   * @param featureName
   */
  fullTreeData(node: NzTreeNode, featureName: string) {
    const feature = this._featureSourceData.featureValues.find(item => item.name === featureName);
    // 默认值
    if (!feature) {
      const defaultValue = this.convertValue(node.origin.inputType, node.origin.defaultValue);
      if (typeof defaultValue === 'boolean') {
        node.isChecked = defaultValue;
        node.origin.value = node.isChecked;
      } else {
        node.origin.value = defaultValue;
      }
      return;
    }

    const featureValue = this.convertValue(node.origin.inputType, feature.value);
    if (typeof featureValue === 'boolean') {
      node.isChecked = featureValue;
      node.origin.value = node.isChecked;
    } else {
      node.origin.value = featureValue;
    }
  }

  /**
   * 根据功能绑定控件类型做转换值
   * @param inputType
   * @param value
   */
  convertValue(inputType: FeatureInputTypeDto, value: any): any {
    if (inputType.name === 'CHECKBOX') {
      return value === 'true';
    }

    return value;
  }

  /**
   * 所有的功能和值
   */
  getGrantedFeatures(): NameValueDto[] {
    if (!this.treeData) {
      return [];
    }

    const features: NameValueDto[] = [];
    this.arrayService.visitTree(this.treeData, item => {
      const feature = new NameValueDto();
      feature.name = item.origin.name;
      feature.value = item.origin.value;
      features.push(feature);
    });

    return features;
  }

  /**
   * 遍历树校验数据
   */
  areAllValuesValid(): boolean {
    let result = true;

    this.arrayService.visitTree(this.treeData, item => {
      if (!this.isFeatureValueValid(item.origin, item.origin.value)) {
        result = false;
      }
    });

    return result;
  }

  /**
   * 是否为子节点
   * @param node
   */
  isLeaf(node: NzTreeNode): boolean {
    return node.children && Array.isArray(node.children) && node.children.length > 0;
  }

  /**
   * 是否展开
   * @param node
   * @param value
   */
  setNodeIsExpanded(node: NzTreeNode, value: boolean) {
    node.isExpanded = value;
  }

  /**
   * 功能的数据校验
   * @param feature 功能
   * @param value 值
   */
  isFeatureValueValid(feature: FlatFeatureDto, value: any): boolean {
    if (!feature || !feature.inputType || !feature.inputType.validator) {
      return true;
    }

    const validator = feature.inputType.validator as any;
    if (validator.name === 'STRING') {
      if (value === undefined || value === null) {
        return validator.allowNull;
      }

      if (typeof value !== 'string') {
        return false;
      }

      if (validator.minLength > 0 && value.length < validator.minLength) {
        return false;
      }

      if (validator.maxLength > 0 && value.length > validator.maxLength) {
        return false;
      }

      if (validator.regularExpression) {
        return new RegExp(validator.regularExpression).test(value);
      }
    } else if (validator.name === 'NUMERIC') {
      const numValue = parseInt(value);

      if (isNaN(numValue)) {
        return false;
      }

      const minValue = validator.minValue;
      if (minValue > numValue) {
        return false;
      }

      const maxValue = validator.maxValue;
      if (maxValue > 0 && numValue > maxValue) {
        return false;
      }
    }

    return true;
  }
}
