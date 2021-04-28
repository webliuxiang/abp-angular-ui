<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('WechatAppConfig')"></page-header>
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
                        v-if="isGranted('Pages.WechatAppConfig.Create')"
                        @click="createOrEdit()">
                        <a-icon type="plus" />
                        <span>{{l("Create")}}</span>
                    </a-button>
                    <a-button :type="'danger'" v-if="isGranted('Pages.WechatAppConfig.BatchDelete')" @click="batchDelete">
                        <a-icon type="delete" />
                        <span>{{l("BatchDelete")}}</span>
                    </a-button>
                    <a-button v-if="isGranted('Pages.WechatAppConfig.ExportToExcel')" @click="exportToExcel">
                        <a-icon type="file-excel" />
                        <span>{{l("ExportToExcel")}}</span>
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
                    <div slot="QRCodeUrlSlot" slot-scope="text, record">
                        <img v-if="record.qrCodeUrl" :src="record.qrCodeUrl" @click="showImg(record.qrCodeUrl)" style="width:50px;" />
                    </div>
                    <span slot="actions" slot-scope="text, record">
                        <!-- 修改 -->
                        <a class="table-edit" @click="createOrEdit(record)" v-if="isGranted('Pages.WechatAppConfig.Edit')">
                            <a-icon type="edit" />{{ l('Edit') }}</a>
                        <!-- 删除 -->
                        <a-popconfirm placement="top" v-if="isGranted('Pages.WechatAppConfig.Delete')" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="deleteItem(record)">
                            <template slot="title">
                                {{ l('ConfirmDeleteWarningMessage') }}
                            </template>
                            <a class="table-delete">
                                <a-icon type="delete" />{{ l('Delete') }}</a>
                        </a-popconfirm>
                        <!-- 更多 -->
                        <!-- <a-dropdown v-if="isGrantedAny('Pages.Administration.Languages.ChangeTexts', 'Pages.Administration.Languages.Edit')">
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
                        </a-dropdown> -->
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
import { WechatAppConfigServiceProxy } from "@/shared/service-proxies";
import CreateOrEditWechatAppConfig from "./create-or-edit-wechat-app-config/create-or-edit-wechat-app-config";
import { appSessionService } from "@/shared/abp";
import moment from "moment";

export default {
    mixins: [AppComponentBase],
    name: "languages",
    components: {
        CreateOrEditWechatAppConfig
    },
    data() {
        return {
            _wechatAppConfigServiceProxy: null,
            // 表格
            columns: [
                {
                    title: this.l("QRCodeUrl"),
                    dataIndex: "QRCodeUrlSlot",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "QRCodeUrlSlot" }
                },
                {
                    title: this.l("WechatAppId"),
                    dataIndex: "appId",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "appId" }
                },
                {
                    title: this.l("WechatName"),
                    dataIndex: "name",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "name" }
                },
                {
                    title: this.l("WechatAppType"),
                    dataIndex: "appTypeStr",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "appTypeStr" }
                },
                {
                    title: this.l("Actions"),
                    dataIndex: "actions",
                    sorter: true,
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
            // 选择多少项
            selectedRowKeys: [],
            selectedRows: [],
            // 请求参数
            filterText: undefined,
            tableData: [],
            // loading
            spinning: false
        };
    },
    created() {
        this._wechatAppConfigServiceProxy = new WechatAppConfigServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._wechatAppConfigServiceProxy
                .getPaged(
                    this.filterText,
                    this.request.sorting,
                    this.request.maxResultCount,
                    this.request.skipCount
                )
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.tableData = res.items;
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
         * 创建编辑
         */
        createOrEdit(item) {
            ModalHelper.create(
                CreateOrEditWechatAppConfig,
                { entity: item },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                if (res) {
                    this.request.skipCount = 0;
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
                        this._wechatAppConfigServiceProxy
                            .batchDelete(ids)
                            .finally(() => {
                                this.spinning = false;
                            })
                            .then(() => {
                                this.request.skipCount = 0;
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
         * 导出为excel
         * todo
         */
        exportToExcel() {},
        /**
         * 单个删除
         */
        deleteItem(item) {
            this.spinning = true;
            this._wechatAppConfigServiceProxy
                .delete(item.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(() => {
                    this.request.skipCount = 0;
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
         * table选择事件
         */
        onSelectChange(selectedRowKeys, selectedRows) {
            this.selectedRowKeys = selectedRowKeys;
            this.selectedRows = selectedRows;
            console.log(this.selectedRows);
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
