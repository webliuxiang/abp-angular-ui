<template>
    <a-tree :checkStrictly="true" :multiple="false" :treeData="treeData">

        <template slot="nzTreeTemplate" slot-scope="node">
            <!-- 特性名称 -->
            <span style="margin-right: 5px;">{{node.title}}</span>

            <!-- 如果是输入框 -->
            <div class="ant-tree-checkbox" v-if="node.origin.inputType.name === 'SINGLE_LINE_STRING'">
                <a-input :defaultValue="node.origin.value"
                         :key="node.origin.name"
                         :name="node.origin.name"
                         @change="inputChange($event,node)"></a-input>
            </div>

            <!-- 如果是checkbox -->
            <span class="ant-tree-checkbox" v-if="node.origin.inputType.name==='CHECKBOX'">
                <a-checkbox :defaultChecked="node.origin.value"
                            :key="node.origin.name"
                            :name="node.origin.name"
                            @change="checkboxChange($event,node)">
                </a-checkbox>
            </span>

            <!-- 如果是下拉框,此功能有待校验 -->
            <select :key="node.origin.name"
                    :name="node.origin.name"
                    @change="node.origin.value=$event"
                    class="ant-tree-checkbox"
                    v-if="node.origin.inputType.name === 'COMBOBOX'"
                    v-model="node.origin.value">
                <option :key="index"
                        :value="item.value"
                        v-for="(item, key, index) in node.origin.itemSource.items">
                    {{item.displayName}}
                </option>
            </select>

        </template>

    </a-tree>
</template>

<script>
    import {AppComponentBase} from "@/shared/component-base";
    import {arrayService} from '@/shared/utils';
    import {NameValueDto} from '@/shared/service-proxies';

    export default {
        name: "edition-feature-tree",
        mixins: [AppComponentBase],
        props: ['editData'],
        data() {
            return {
                treeDataVal: [],
                featureSourceData: this.editData,
            }
        },
        mounted() {
            if (this.featureSourceData) {
                this.processsTreeData();
            }
        },
        computed: {
            treeData: {
                get() {
                    return this.treeDataVal;
                },
                set(val) {
                    this.treeDataVal = val;
                }
            }
        },
        watch: {
            editData(val) {
                this.featureSourceData = val;
                this.processsTreeData();
            }
        },
        methods: {
            processsTreeData() {
                let treeData = arrayService.arrToTreeNode(
                    this.featureSourceData.features,
                    {
                        idMapName: 'name',
                        parentIdMapName: 'parentName',
                        titleMapName: 'displayName',
                    });

                arrayService.visitTree(treeData, (item) => {
                    // 绑定模板
                    item.scopedSlots = {title: 'nzTreeTemplate'};

                    item.selectable = false;
                    item.class = "nzTreeTemplate";
                    this.fullTreeData(item, item.origin.name);
                });

                this.treeData = treeData;
            },
            fullTreeData(node, featureName) {
                let feature = this.featureSourceData.featureValues.find(item => item.name === featureName);
                // 默认值
                if (!feature) {
                    let defaultValue = this.convertValue(node.origin.inputType, node.origin.defaultValue);
                    if (typeof defaultValue === "boolean") {
                        node.isChecked = defaultValue;
                        node.origin.value = node.isChecked;
                    } else {
                        node.origin.value = defaultValue;
                    }
                    return;
                }

                let featureValue = this.convertValue(node.origin.inputType, feature.value);
                if (typeof featureValue === "boolean") {
                    node.isChecked = featureValue;
                    node.origin.value = node.isChecked;
                } else {
                    node.origin.value = featureValue;
                }

            },
            convertValue(inputType, value) {
                if (inputType.name === "CHECKBOX") {
                    return value === "true";
                }

                return value;
            },
            getGrantedFeatures() {
                if (!this.treeData) {
                    return [];
                }

                let features = [];
                arrayService.visitTree(this.treeData, (item) => {
                    let feature = new NameValueDto();
                    feature.name = item.origin.name;
                    feature.value = item.origin.value;
                    features.push(feature);
                });

                return features;
            },
            areAllValuesValid() {
                let result = true;

                arrayService.visitTree(this.treeData, item => {
                    if (!this.isFeatureValueValid(item.origin, item.origin.value)) {
                        result = false;
                    }
                });

                return result;
            },
            isLeaf(node) {
                return node.children && Array.isArray(node.children) && node.children.length > 0;
            },
            setNodeIsExpanded(node, value) {
                node.isExpanded = value;
            },
            isFeatureValueValid(feature, value) {
                if (!feature || !feature.inputType || !feature.inputType.validator) {
                    return true;
                }

                const validator = feature.inputType.validator;
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
                        return (new RegExp(validator.regularExpression)).test(value);
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
            },
            inputChange(e, node) {
                node.origin.value = e.srcElement.value;
            },
            checkboxChange(e, node) {
                node.origin.value = e.target.checked;
            },

        }
    }
</script>

<style scoped lang="less">
    @import "./edition-feature-tree.less";

    .nzTreeTemplate {
        .ant-tree-node-content-wrapper {
            &:hover {
                background-color: red !important;
            }

        }
    }
</style>
