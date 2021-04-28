<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('ManageBlog')"></page-header>
        <a-card :bordered="false">
            <a-row :gutter="8">
                <a-col class="gutter-row" :span="20">
                    <a-input v-model="filterText" :placeholder="l('SearchWithThreeDot')" />
                </a-col>
                <a-col class="gutter-row" :span="4">
                    <a-button type="primary" @click="getData">
                        {{ l('Search') }}
                    </a-button>
                    <a-button @click="refreshGoFirstPage"> {{ l('Reset') }}</a-button>
                </a-col>
            </a-row>
            <!-- 按钮 -->
            <a-row :gutter="8" class="btn--container">
                <a-col class="gutter-row" :span="24">
                    <a-button type="primary" v-if="isGranted('Pages.Blog.Create')" @click="createOrEdit()">
                        <a-icon type="plus" />
                        {{ l('Create') }}
                    </a-button>
                    <a-button v-if="isGranted('Pages.Blog.ExportExcel')" @click="exportToExcel()">
                        <a-icon type="file-excel" />
                        <span>{{l("ExportToExcel")}}</span>
                    </a-button>
                    <a-button type="danger" v-if="isGranted('Pages.Blog.BatchDelete')" @click="batchDelete()">
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
                    <span slot="actions" slot-scope="text, record">
                        <!-- 修改 -->
                        <a class="table-edit" @click="createOrEdit(record.id)" v-if="isGranted('Pages.Blog.Edit')">
                            <a-icon type="edit" />{{ l('Edit') }}</a>
                        <!-- 新建文章 -->
                        <a class="table-plus" @click="newPost(record.id)">
                            <a-icon type="plus" />{{ l('NewPost') }}</a>
                        <!-- 删除 -->
                        <a-popconfirm placement="top" class="table-delete" v-if="isGranted('Pages.Blog.Delete')" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="deleteItem(record)">
                            <template slot="title">
                                {{ l('ConfirmDeleteWarningMessage') }}
                            </template>
                            <a class="table-delete">
                                <a-icon type="delete" />{{ l('Delete') }}</a>
                        </a-popconfirm>
                        <!-- 更多 -->
                        <!-- <a-dropdown v-if="isGrantedAny('Pages.Blog.Create', 'Pages.Blog.Delete')">
                           <a class="ant-dropdown-link" @click="e => e.preventDefault()">
                                {{ l('More') }}
                                <a-icon type="down" />
                            </a>
                            <a-menu slot="overlay">
                                <a-menu-item
                                    v-if="isGranted('Pages.Blog.Create')">
                                    <a href="javascript:;" @click="createOrEdit()">
                                        <a-icon type="plus" />
                                        <span>{{ l('Create') }}</span>
                                    </a>
                                </a-menu-item> -->
                        <!-- 删除 -->
                        <!-- <a-menu-item v-if="isGranted('Pages.Blogroll.Delete')">
                                    <a-popconfirm placement="top" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="deleteItem(record)">
                                        <template slot="title">
                                            {{ l('ConfirmDeleteWarningMessage') }}
                                        </template>
                                        <a class="table-delete">
                                            <a-icon type="delete" />{{ l('Delete') }}</a>
                                    </a-popconfirm>
                                </a-menu-item>
                            </a-menu>
                        </a-dropdown>  -->
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
import { BlogServiceProxy } from "@/shared/service-proxies";
import CreateOrEditBlogsComponent from "./create-or-edit-blogs";
import NewPostsComponent from "../posts/new-posts";
import { fileDownloadService } from "@/shared/utils";
import moment from "moment";

export default {
    mixins: [AppComponentBase],
    name: "bannerads",
    components: {},
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
                    title: this.l("BlogName"),
                    dataIndex: "name",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "name" }
                },
                {
                    title: this.l("BlogShortName"),
                    dataIndex: "shortName",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "shortName" }
                },
                {
                    title: this.l("BlogDescription"),
                    dataIndex: "description",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "description" }
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
            _blogServiceProxy: ""
        };
    },
    created() {
        this._blogServiceProxy = new BlogServiceProxy(this.$apiUrl, this.$api);
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._blogServiceProxy
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
                CreateOrEditBlogsComponent,
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
                        this._blogServiceProxy
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
         * 清空
         */
        restCheckStatus() {
            this.selectedRowKeys = [];
        },
        /**
         * 单个删除
         */
        deleteItem(item) {
            this.spinning = true;
            this._blogServiceProxy
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
         * 导出为excel
         */
        exportToExcel() {
            this.spinning = true;
            this._blogServiceProxy.getToExcelFile().then(result => {
                this.spinning = false;
                fileDownloadService.downloadTempFile(result);
            });
        },
        /*
         * 新建文章
         */
        newPost(id) {
            // this.$router.push({
            //     path: "/app/blogging/create-or-edit-posts",
            //     query: { blogId: id }
            // });
            ModalHelper.create(
                NewPostsComponent,
                { id: id },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                if (res) {
                    this.refreshGoFirstPage();
                }
            });
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
.table-plus,
.table-delete {
    margin-left: 10px;
}
</style>
