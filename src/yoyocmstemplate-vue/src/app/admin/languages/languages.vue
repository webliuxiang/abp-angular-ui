<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('Languages')"></page-header>
        <a-card :bordered="false">
            <a-form :layout="'vertical'" @submit.prevent="getData">
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
                </a-row>
            </a-form>
            <!-- 操作部分 -->
            <a-row :gutter="8">
                <a-col :md="20" :sm="12">
                    <a-button
                        :type="'primary'"
                        v-if="isGranted('Pages.Administration.Languages.Create')"
                        @click="createOrEditLanguage()">
                        <a-icon type="plus" />
                        <span>{{l("CreateNewLanguage")}}</span>
                    </a-button>
                    <a-button :type="'danger'" v-if="isGranted('Pages.Administration.Languages.Delete')" @click="batchDelete">
                        <a-icon type="delete" />
                        <span>{{l("BatchDelete")}}</span>
                    </a-button>
                </a-col>
            </a-row>

            <!-- 数据部分 -->
            <div class="my-md">
                <a-alert :type="'info'" :showIcon="true">
                    <template slot="message">
                        <span v-html="l('GridSelectedXItemsTips',selectedRowKeys.length)"></span>
                        <a @click="restCheckStatus()" class="ml-md">{{l('ClearEmpty')}}</a>
                        <a-divider type="vertical"></a-divider>
                        <a @click="getData()">{{l('Refresh')}}</a>
                    </template>
                </a-alert>
            </div>

            <a-row>
                <a-table
                    class="list-table"
                    @change="getData"
                    :pagination="false"
                    :rowSelection="{selectedRowKeys: selectedRowKeys,onChange: onSelectChange}"
                    :columns="columns"
                    :rowKey="tableDatas => tableDatas.id"
                    :dataSource="tableData">
                    <p class="isDisabledStr" slot="isDisabledStr" slot-scope="text, record">
                        <a-icon v-if="!record.isDisabled" style="color:#1890ff" type="check" />
                        <a-icon v-if="record.isDisabled" style="color:#1890ff" type="close" />
                    </p>
                    <span slot="actions" slot-scope="text, record">
                        <!-- 修改 -->
                        <a class="table-edit" @click="createOrEditLanguage(record.id)" v-if="isGranted('Pages.Administration.Languages.Edit') && record.tenantId === tenantId">
                            <a-icon type="edit" />{{ l('Edit') }}</a>
                        <!-- 删除 -->
                        <a-popconfirm placement="top" v-if="isGranted('Pages.Administration.Languages.Delete') && record.tenantId === tenantId" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="deleteItem(record)">
                            <template slot="title">
                                {{ l('ConfirmDeleteWarningMessage') }}
                            </template>
                            <a class="table-delete">
                                <a-icon type="delete" />{{ l('Delete') }}</a>
                        </a-popconfirm>
                        <!-- 更多 -->
                        <a-dropdown v-if="isGrantedAny('Pages.Administration.Languages.ChangeTexts', 'Pages.Administration.Languages.Edit')">
                            <a class="ant-dropdown-link" @click="e => e.preventDefault()">
                                {{ l('Actions') }}
                                <a-icon type="down" />
                            </a>
                            <a-menu slot="overlay">
                                <a-menu-item
                                    v-if="isGranted('Pages.Administration.Languages.ChangeTexts')"
                                    @click="changeTexts(record)">
                                    <a href="javascript:;">
                                        <span>{{ l('ChangeTexts') }}</span>
                                    </a>
                                </a-menu-item>
                                <a-menu-item
                                    v-if="isGranted('Pages.Administration.Languages.Edit')"
                                    @click="setAsDefaultLanguage(record)">
                                    <a href="javascript:;">
                                        <span>{{ l('SetAsDefaultLanguage') }}</span>
                                    </a>
                                </a-menu-item>
                            </a-menu>
                        </a-dropdown>
                    </span>
                </a-table>
            </a-row>
        </a-card>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { ModalHelper } from "@/shared/helpers";
import { LanguageServiceProxy } from "@/shared/service-proxies";
import CreateOrEditLanguage from "./create-or-edit-language/create-or-edit-language";
import moment from "moment";
import { appSessionService } from "@/shared/abp";

export default {
    mixins: [AppComponentBase],
    name: "languages",
    components: {
        CreateOrEditLanguage
    },
    data() {
        return {
            _languageServiceProxy: null,
            // 选中的权限过滤
            selectedPermission: [],
            // 表格
            columns: [
                {
                    title: this.l("Name"),
                    dataIndex: "displayNameStr",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "displayNameStr" }
                },
                {
                    title: this.l("Code"),
                    dataIndex: "name",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "name" }
                },
                {
                    title: this.l("CreationTime"),
                    dataIndex: "creationTimeStr",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "creationTimeStr" }
                },
                {
                    title: this.l("Enabled"),
                    dataIndex: "isDisabledStr",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "isDisabledStr" }
                },
                {
                    title: this.l("Actions"),
                    dataIndex: "actions",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "actions" }
                }
            ],
            // 选择多少项
            selectedRowKeys: [],
            selectedRows: [],
            // 请求参数
            filterText: undefined,
            tableData: [],
            // loading
            spinning: false,
            tenantId: ""
        };
    },
    created() {
        this._languageServiceProxy = new LanguageServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.tenantId = appSessionService.tenantId;
        console.log(this.tenantId);
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._languageServiceProxy
                .getLanguages(this.filterText)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    console.log(res);
                    this.tableData = res.items;
                    this.tableData.map(item => {
                        item.creationTimeStr = item.creationTime
                            ? moment(item.creationTime).format(
                                  "YYYY-MM-DD HH:mm:ss"
                              )
                            : "-";
                        item.displayNameStr =
                            item.name === res.defaultLanguageName
                                ? item.displayName + "(默认)"
                                : item.displayName;
                    });
                });
        },
        /**
         * 创建编辑语言
         */
        createOrEditLanguage(id) {
            ModalHelper.create(
                CreateOrEditLanguage,
                { id: id },
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
         * table选择事件
         */
        onSelectChange(selectedRowKeys, selectedRows) {
            this.selectedRowKeys = selectedRowKeys;
            this.selectedRows = selectedRows;
            console.log(this.selectedRows);
        },
        /**
         * 批量删除
         */
        batchDelete(e) {
            const selectCount = this.selectedRowKeys.length;
            if (selectCount <= 0) {
                abp.message.warn(this.l("PleaseSelectAtLeastOneItem"));
                return;
            }
            this.message.confirm(
                this.l("ConfirmDeleteXItemsWarningMessage", selectCount),
                res => {
                    if (res) {
                        const ids = _.map(this.selectedRowKeys);
                        console.log(ids);
                        this.spinning = true;
                        this._languageServiceProxy
                            .batchDelete(ids)
                            .finally(() => {
                                this.spinning = false;
                            })
                            .then(() => {
                                this.getData();
                                this.$notification["success"]({
                                    message: this.l("SuccessfullyDeleted")
                                });
                            });
                    }
                }
            );
        },
        /**
         * 单个删除
         */
        deleteItem(item) {
            this.spinning = true;
            this._languageServiceProxy
                .deleteLanguage(item.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(() => {
                    this.getData();
                    this.$notification["success"]({
                        message: this.l("SuccessfullyDeleted")
                    });
                });
        },
        /**
         * 清空选择
         */
        restCheckStatus() {
            this.selectedRowKeys = [];
        },
        /**
         * 设为默认语言
         */
        setAsDefaultLanguage(item) {
            this.spinning = true;
            this._languageServiceProxy
                .setDefaultLanguage({
                    name: item.name
                })
                .finally(() => {
                    this.spinning = false;
                })
                .then(() => {
                    this.getData();
                    this.$notification["success"]({
                        message: this.l("SuccessfullySaved")
                    });
                });
        },
        /**
         * 更换文本
         */
        changeTexts(item) {
            setTimeout(() => {
                this.$router.push({
                    path: "/app/admin/languagetexts",
                    query: { lang: item.name }
                });
            }, 300);
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
