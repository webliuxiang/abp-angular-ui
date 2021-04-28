<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('LanguageTexts')"></page-header>
        <a-card :title="l('LanguagesHeaderInfo')" :bordered="false">
            <a-button slot="extra" @click="getData">
                <a-icon type="reload" />
                {{ l('Refresh') }}
            </a-button>
            <a-button slot="extra" type="primary" @click="backLanguageList">
                <a-icon type="rollback" />
                {{ l('BackLanguageList') }}
            </a-button>
            <a-form :form="form" :layout="'vertical'" @submit.prevent="getData">
                <a-row :gutter="8">
                    <!-- 搜索 -->
                    <a-col :sm="24">
                        <a-form-item>
                            <a-input-search
                                name="filterText"
                                :placeholder="l('SearchWithThreeDot')"
                                @search="getData"
                                enterButton
                                v-model="filterText"
                                v-decorator="['filterText']" />
                        </a-form-item>
                    </a-col>
                    <!-- 源语言 -->
                    <a-col :sm="6">
                        <a-form-item :label="l('BaseLanguage')">
                            <a-select :placeholder="l('BaseLanguage')" @change="getData" v-decorator="['baseLanguageName', {
                                    rules: [
                                        { required: true, message: l('ThisFieldIsRequired') },
                                    ]
                                }]">
                                <a-select-option v-for="item in ILanguageInfo" :key="item.name" :value="item.name">
                                    {{ item.displayName }}
                                </a-select-option>
                            </a-select>
                        </a-form-item>
                    </a-col>
                    <!-- 目标语言 -->
                    <a-col :sm="6">
                        <a-form-item :label="l('TargetLanguage')">
                            <a-select :placeholder="l('TargetLanguage')" @change="getData" v-decorator="['targetLanguageName', {
                                    rules: [
                                        { required: true, message: l('ThisFieldIsRequired') },
                                    ]
                                }]">
                                <a-select-option v-for="item in ILanguageInfo" :key="item.name" :value="item.name">
                                    {{ item.displayName }}
                                </a-select-option>
                            </a-select>
                        </a-form-item>
                    </a-col>
                    <!-- 源 -->
                    <a-col :sm="6">
                        <a-form-item :label="l('Source')">
                            <a-select :placeholder="l('Source')" @change="getData" v-decorator="['sourceName', {
                                    rules: [
                                        { required: true, message: l('ThisFieldIsRequired') },
                                    ]
                                }]">
                                <a-select-option v-for="item in sources" :key="item.name" :value="item.name">
                                    {{ item.name }}
                                </a-select-option>
                            </a-select>
                        </a-form-item>
                    </a-col>
                    <!-- 目标值 -->
                    <a-col :sm="6">
                        <a-form-item :label="l('TargetValue')">
                            <a-select :placeholder="l('TargetValue')" @change="getData" v-decorator="['targetValueFilter', {
                                    rules: [
                                        { required: true, message: l('ThisFieldIsRequired') },
                                    ]
                                }]">
                                <a-select-option value="ALL">
                                    {{ l('All') }}
                                </a-select-option>
                                <a-select-option value="EMPTY">
                                    {{ l('EmptyOnes') }}
                                </a-select-option>
                            </a-select>
                        </a-form-item>
                    </a-col>
                </a-row>
            </a-form>
            <!-- table -->
            <a-row>
                <a-table
                    class="list-table"
                    @change="getData"
                    :pagination="false"
                    :columns="columns"
                    :rowKey="tableDatas => tableDatas.id"
                    :dataSource="tableData">
                    <span slot="actions" slot-scope="text, record">
                        <!-- 修改 -->
                        <a class="table-edit" @click="edit(record)">
                            <a-icon type="edit" />{{ l('Edit') }}</a>
                    </span>
                </a-table>
                <a-pagination
                    class="pagination"
                    size="middle"
                    :total="totalItems"
                    showSizeChanger
                    showQuickJumper
                    :showTotal="showTotalFun"
                    @change="onChange"
                    @showSizeChange="showSizeChange" />
            </a-row>
        </a-card>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { ModalHelper } from "@/shared/helpers";
import { LanguageServiceProxy } from "@/shared/service-proxies";
import moment from "moment";
import { AppConsts } from "@/abpPro/AppConsts";
import { appSessionService } from "@/shared/abp";
import EditLanguageText from "./edit-language-text/edit-language-text";

export default {
    mixins: [AppComponentBase],
    name: "language-texts",
    components: {},
    data() {
        return {
            form: this.$form.createForm(this),
            // 表格
            columns: [
                {
                    title: this.l("Key"),
                    dataIndex: "key",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "key" }
                },
                {
                    title: this.l("BaseValue"),
                    dataIndex: "baseValue",
                    align: "center",
                    scopedSlots: { customRender: "baseValue" }
                },
                {
                    title: this.l("TargetValue"),
                    dataIndex: "targetValue",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "targetValue" }
                },
                {
                    title: this.l("Actions"),
                    dataIndex: "actions",
                    align: "center",
                    scopedSlots: { customRender: "actions" }
                }
            ],
            // 总数
            totalItems: 0,
            // 当前页码
            pageNumber: 1,
            // 共多少页
            totalPages: 1,
            // 条数显示范围
            pagerange: [1, 1],
            // 显示条数
            pageSizeOptions: ["10", "20", "30", "40"],
            request: { sorting: "", maxResultCount: 10, skipCount: 0 },
            // 请求参数
            filterText: "",
            tableData: [],
            // loading
            spinning: false,
            // 语言下拉
            ILanguageInfo: [],
            // 源
            sources: [],
            _languageServiceProxy: "",
            // 请求参数
            sourceName: "",
            baseLanguageName: "",
            targetLanguageName: "",
            targetValueFilter: ""
        };
    },
    created() {
        this.ILanguageInfo = abp.localization.languages;
        this.sources = abp.localization.sources;
        this._languageServiceProxy = new LanguageServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.$nextTick(() => {
            console.log(this.$route.query.lang);
            this.form.setFieldsValue({
                baseLanguageName: this.ILanguageInfo.find(
                    item => item.isDefault
                ).name,
                targetLanguageName: this.ILanguageInfo.find(
                    item => item.name === this.$route.query.lang
                ).name,
                sourceName: this.sources[0].name,
                targetValueFilter: "ALL"
            });
            this.getData();
        });
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            console.log(this.form.getFieldsValue());
            this.sourceName = this.form.getFieldsValue().sourceName;
            this.baseLanguageName = this.form.getFieldsValue().baseLanguageName;
            this.targetLanguageName = this.form.getFieldsValue().targetLanguageName;
            this.targetValueFilter = this.form.getFieldsValue().targetValueFilter;
            this.spinning = true;
            this._languageServiceProxy
                .getLanguageTexts(
                    this.sourceName,
                    this.baseLanguageName,
                    this.targetLanguageName,
                    this.targetValueFilter,
                    this.filterText,
                    undefined,
                    this.request.maxResultCount,
                    this.request.skipCount
                )
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    console.log(res);
                    this.tableData = res.items;
                    // this.tableData.map(item => {
                    //     item.creationTimeStr = item.creationTime
                    //         ? moment(item.creationTime).format(
                    //               "YYYY-MM-DD HH:mm:ss"
                    //           )
                    //         : "-";
                    // });
                    this.totalItems = res.totalCount;
                    this.totalPages = Math.ceil(
                        res.totalCount / this.request.maxResultCount
                    );
                    this.pagerange = [
                        (this.pageNumber - 1) * this.request.maxResultCount + 1,
                        this.pageNumber * this.request.maxResultCount
                    ];
                });
        },
        /**
         * 编辑语言
         */
        edit(item) {
            console.log(item);
            let senItem = Object.assign(item, {
                baseLanguageName: this.form.getFieldsValue().baseLanguageName,
                baseLanguageNameText: this.ILanguageInfo.find(
                    item =>
                        item.name ===
                        this.form.getFieldsValue().baseLanguageName
                ).displayName,
                targetLanguageNameText: this.ILanguageInfo.find(
                    item =>
                        item.name ===
                        this.form.getFieldsValue().targetLanguageName
                ).displayName,
                targetLanguageName: this.form.getFieldsValue()
                    .targetLanguageName,
                sourceName: this.form.getFieldsValue().sourceName,
                value: item.targetValue
                // sourceName:
            });
            ModalHelper.create(
                EditLanguageText,
                {
                    editItem: senItem
                },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                if (res) {
                    this.getData();
                }
            });
        },
        /**
         * 分页事件
         */
        showTotalFun() {
            return this.l(
                "GridFooterDisplayText",
                this.pageNumber,
                this.totalPages,
                this.totalItems,
                this.pagerange[0],
                this.pagerange[1]
            );
        },
        /**
         * 分页
         */
        onChange(page, pageSize) {
            this.pageNumber = page;
            this.request.skipCount = (page - 1) * this.request.maxResultCount;
            this.getData();
        },
        showSizeChange(current, size) {
            this.pageNumber = 1;
            this.request.maxResultCount = size;
            this.getData();
        },
        /**
         * 返回语言列表
         */
        backLanguageList() {
            this.$router.push({
                path: "/app/admin/languages"
            });
        }
    }
};
</script>

<style scoped lang="less">
.subscriptionEndUtc {
    text-align: center;
}
// table
.list-table {
    i {
        margin-right: 10px;
    }
}
.pagination {
    margin: 10px auto;
    text-align: right;
}
.table-edit {
    .anticon {
        margin-right: 10px;
    }
}
.table-delete,
.ant-dropdown-link {
    margin-left: 20px;
}
</style>
