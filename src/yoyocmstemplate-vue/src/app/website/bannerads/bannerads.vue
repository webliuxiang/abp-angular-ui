<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('ManageBannerAd')"></page-header>
        <a-card :bordered="false">
            <a-row :gutter="8">
                <a-col class="gutter-row" :span="20">
                    <a-input v-model="filterText" :placeholder="l('SearchWithThreeDot')" />
                </a-col>
                <a-col class="gutter-row" :span="4">
                    <a-button type="primary" @click="getData">
                        {{ l('Search') }}
                    </a-button>
                    <a-button @click="refreshGoFirstPage">{{ l('Reset') }}</a-button>
                </a-col>
            </a-row>
            <!-- 按钮 -->
            <a-row :gutter="8" class="btn--container">
                <a-col class="gutter-row" :span="24">
                    <a-button type="primary" v-if="isGranted('Pages.BannerAd.Create')" @click="createOrEdit()">
                        <a-icon type="plus" />
                        {{ l('Create') }}
                    </a-button>
                    <a-button type="danger" v-if="isGranted('Pages.BannerAd.BatchDelete')" @click="batchDelete()">
                        <a-icon type="delete" />
                        {{ l('BatchDelete') }}
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
            <!-- table -->
            <a-row>
                <a-table
                    class="list-table"
                    @change="getData"
                    :pagination="false"
                    :rowSelection="{selectedRowKeys: selectedRowKeys, onChange: onSelectChange}"
                    :columns="columns"
                    :rowKey="tableDatas => tableDatas.id"
                    :dataSource="tableData">
                    <!-- 图片 -->
                    <p class="imageUrl" slot="imageUrl" slot-scope="text, record">
                        <a-avatar :size="64" :src="record.imageUrl" />
                    </p>
                    <p class="thumbImgUrl" slot="thumbImgUrl" slot-scope="text, record">
                        <a-avatar :size="64" :src="record.thumbImgUrl" />
                    </p>
                    <!-- 目标url -->
                    <a-tooltip slot="bannerAdUrl" slot-scope="text, record">
                        <template slot="title">
                            {{ record.url }}
                        </template>
                        {{ record.url }}
                        <br>
                        <a target="_blank" :href="record.url">预览</a>
                    </a-tooltip>
                    <span slot="actions" slot-scope="text, record">
                        <!-- 修改 -->
                        <a class="table-edit" @click="createOrEdit(record.id)" v-if="isGranted('Pages.BannerAd.Edit')">
                            <a-icon type="edit" />{{ l('Edit') }}</a>
                        <!-- 更多 -->
                        <a-dropdown v-if="isGrantedAny('Pages.BannerAd.Create', 'Pages.BannerAd.Delete')">
                            <a class="ant-dropdown-link" @click="e => e.preventDefault()">
                                {{ l('More') }}
                                <a-icon type="down" />
                            </a>
                            <a-menu slot="overlay">
                                <a-menu-item
                                    v-if="isGranted('Pages.BannerAd.Create')">
                                    <a href="javascript:;" @click="createOrEdit()">
                                        <a-icon type="plus" />
                                        <span>{{ l('Create') }}</span>
                                    </a>
                                </a-menu-item>
                                <!-- 删除 -->
                                <a-menu-item v-if="isGranted('Pages.BannerAd.Delete')">
                                    <a-popconfirm placement="top" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="deleteItem(record)">
                                        <template slot="title">
                                            {{ l('ConfirmDeleteWarningMessage') }}
                                        </template>
                                        <a class="table-delete">
                                            <a-icon type="delete" />{{ l('Delete') }}</a>
                                    </a-popconfirm>
                                </a-menu-item>
                            </a-menu>
                        </a-dropdown>
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
import { BannerImgServiceProxy } from "@/shared/service-proxies";
import CreateOrEditBannerAdComponent from "./create-or-edit-banner-ad";
import moment from "moment";

export default {
    mixins: [AppComponentBase],
    name: "bannerads",
    components: { CreateOrEditBannerAdComponent },
    data() {
        return {
            spinning: false,
            // 搜索
            filterText: "",
            // 选择多少项
            selectedRowKeys: [],
            tableData: [],
            // 表格
            columns: [
                {
                    title: this.l("Title"),
                    dataIndex: "title",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "title" }
                },
                {
                    title: this.l("ImageUrl"),
                    dataIndex: "imageUrl",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "imageUrl" }
                },
                {
                    title: this.l("ThumbImgUrl"),
                    dataIndex: "thumbImgUrl",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "thumbImgUrl" }
                },
                {
                    title: this.l("Description"),
                    dataIndex: "description",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "description" }
                },
                {
                    title: this.l("BannerAdUrl"),
                    dataIndex: "bannerAdUrl",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "bannerAdUrl" }
                },
                {
                    title: this.l("Weight"),
                    dataIndex: "weight",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "weight" }
                },
                {
                    title: this.l("Price"),
                    dataIndex: "price",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "price" }
                },
                {
                    title: this.l("BannerAdTypes"),
                    dataIndex: "types",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "types" }
                },
                {
                    title: this.l("ViewCount"),
                    dataIndex: "viewCount",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "viewCount" }
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
            _bannerImgServiceProxy: ""
        };
    },
    created() {
        this._bannerImgServiceProxy = new BannerImgServiceProxy(
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
            this._bannerImgServiceProxy
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
                    console.log(res.items);
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
         * table选择事件
         */
        onSelectChange(selectedRowKeys, selectedRows) {
            this.selectedRowKeys = selectedRowKeys;
            this.selectedRows = selectedRows;
            console.log(this.selectedRows);
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
         */ onSelectChange(selectedRowKeys, selectedRows) {
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
        },
        /**
         * 重置
         */
        refreshGoFirstPage() {
            this.filterText = "";
            this.request.skipCount = 0;
            this.getData();
        },
        /**
         * 新建修改
         */
        createOrEdit(id) {
            ModalHelper.create(
                CreateOrEditBannerAdComponent,
                { id: id },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                if (res) {
                    this.refreshGoFirstPage();
                }
            });
        },
        /**
         * 批量删除
         */
        batchDelete() {
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
                        this._bannerImgServiceProxy
                            .batchDelete(ids)
                            .finally(() => {
                                this.spinning = false;
                            })
                            .then(() => {
                                this.selectedRowKeys = [];
                                this.refreshGoFirstPage();
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
            this._bannerImgServiceProxy
                .delete(item.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(() => {
                    this.refreshGoFirstPage();
                    this.$notification["success"]({
                        message: this.l("SuccessfullyDeleted")
                    });
                });
        },
        /**
         * 清空
         */
        restCheckStatus() {
            this.selectedRowKeys = [];
        }
    }
};
</script>

<style scoped lang="less">
.btn--container {
    margin: 20px 0;
}
.pagination {
    margin: 10px auto;
    text-align: right;
}
.ant-dropdown-link {
    margin-left: 10px;
}
</style>
