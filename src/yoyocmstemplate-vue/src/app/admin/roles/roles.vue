<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('Roles')"></page-header>
        <a-card :bordered="false">
            <a-form :layout="'vertical'" @submit.prevent="getData">
                <a-row :gutter="8">
                    <!-- 搜索 -->
                    <a-col :sm="12">
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
                    <!-- 权限过滤 -->
                    <a-col :sm="12">
                        <a-form-item>
                            <permission-combox
                                :multiple="true"
                                :dropDownStyle="{ 'max-height': '500px' }"
                                :selectedPermission="selectedPermission"
                                @selectedPermissionChange="refreshGoFirstPage">
                            </permission-combox>
                        </a-form-item>
                    </a-col>
                </a-row>
            </a-form>
            <!-- 操作部分 -->
            <a-row :gutter="8">
                <a-col :md="20" :sm="12">
                    <a-button
                        :type="'primary'"
                        v-if="isGranted('Pages.Administration.Roles.Create')"
                        @click="createOrEdit()">
                        <a-icon type="plus" />
                        <span>{{l("CreateNewRole")}}</span>
                    </a-button>
                    <a-button :type="'danger'" v-if="isGranted('Pages.Administration.Roles.Delete')" @click="batchDelete">
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
                    :rowSelection="{selectedRowKeys: selectedRowKeys, onChange: onSelectChange, getCheckboxProps:getCheckboxProps}"
                    :columns="columns"
                    :rowKey="tableDatas => tableDatas.id"
                    :dataSource="tableData">
                    <p class="displayName" slot="displayName" slot-scope="text, record">{{ record.displayName }}
                        <a-tag v-if="record.isStatic" color="#108ee9"> {{ l('Static') }}</a-tag>
                        <a-tag v-if="record.isDefault" color="#2db7f5"> {{ l('Default') }}</a-tag>
                    </p>
                    <span slot="actions" slot-scope="text, record">
                        <!-- 修改 -->
                        <a class="table-edit" @click="createOrEdit(record.id)" v-if="isGranted('Pages.Administration.Roles.Edit')">
                            <a-icon type="edit" />{{ l('Edit') }}</a>
                        <!-- 删除 -->
                        <a-popconfirm placement="top" v-if="!record.isStatic && isGranted('Pages.Administration.Roles.Delete')" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="deleteItem(record)">
                            <template slot="title">
                                {{ l('ConfirmDeleteWarningMessage') }}
                            </template>
                            <a class="table-delete">
                                <a-icon type="delete" />{{ l('Delete') }}</a>
                        </a-popconfirm>
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
import PermissionCombox from "../shared/permission-combox/permission-combox.vue";
import { ModalHelper } from "@/shared/helpers";
import { RoleServiceProxy } from "@/shared/service-proxies";
import CreateEditRoleCompent from "./create-or-edit-role/create-or-edit-role";
import moment from "moment";

export default {
    mixins: [AppComponentBase],
    name: "tenants",
    components: {
        PermissionCombox,
        CreateEditRoleCompent
    },
    data() {
        return {
            _roleServiceProxy: null,
            // 选中的权限过滤
            selectedPermission: [],
            // 表格
            columns: [
                {
                    title: this.l("RoleName"),
                    dataIndex: "displayName",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "displayName" }
                },
                {
                    title: this.l("CreationTime"),
                    dataIndex: "creationTimeStr",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "creationTimeStr" }
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
        this._roleServiceProxy = new RoleServiceProxy(this.$apiUrl, this.$api);
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._roleServiceProxy
                .getPaged(
                    this.selectedPermission,
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
                    this.tableData.map(item => {
                        item.creationTimeStr = item.creationTime
                            ? moment(item.creationTime).format(
                                  "YYYY-MM-DD HH:mm:ss"
                              )
                            : "-";
                    });
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
         * table默认配置 （是否禁用）
         */
        getCheckboxProps: record => ({
            props: {
                disabled: record.isStatic
            }
        }),
        /**
         * 添加角色  修改角色
         */
        createOrEdit(id) {
            ModalHelper.create(
                CreateEditRoleCompent,
                { id: id },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                console.log(res);
                if (res) {
                    this.getData();
                }
            });
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
                        this._roleServiceProxy
                            .batchDelete(ids)
                            .finally(() => {
                                this.spinning = false;
                            })
                            .then(() => {
                                this.selectedRowKeys = [];
                                this.refreshGoFirstPage(
                                    this.selectedPermission
                                );
                                this.$notification["success"]({
                                    message: this.l("SuccessfullyDeleted")
                                });
                            });
                    }
                }
            );
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
         * 单个删除
         */
        deleteItem(item) {
            console.log(item);
            if (item.isStatic) {
                abp.message.warn(
                    this.l(
                        "XUserCannotBeDeleted",
                        AppConsts.userManagement.defaultAdminUserName
                    )
                );
                return;
            }
            this.spinning = true;
            this._roleServiceProxy
                .delete(item.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(() => {
                    this.refreshGoFirstPage(this.selectedPermission);
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
         * 选择完权限过滤
         */
        refreshGoFirstPage(data) {
            this.selectedPermission = data;
            this.request = { maxResultCount: 10, skipCount: 0 };
            this.getData();
        }
    }
};
</script>

<style scoped lang="less">
@import "./roles.less";
</style>
