<template>
    <a-spin :spinning="spinning">
        <div class="user-container">
            <div class="table--container">
                <!-- 操作 -->
                <a-row :gutter="8" class="opeattion-container">
                    <a-col :span="16">
                        <a-input-search
                            name="filterText"
                            @search="getData"
                            :placeholder="l('SearchWithThreeDot')"
                            enterButton
                            v-model="filterText" />
                    </a-col>
                    <a-col :span="6" :offset="2">
                        <a @click="addUser()" v-if="isGranted('Pages.Administration.OrganizationUnits.ManageUsers')">
                            <a-icon type="plus" /> {{ l('AddUser') }} </a>
                        <a-divider v-if="isGranted('Pages.Administration.OrganizationUnits.ManageUsers')" type="vertical"></a-divider>
                        <a @click="batchDelete()" v-if="isGranted('Pages.Administration.OrganizationUnits.ManageUsers')">
                            <a-icon type="delete" />{{ l('BatchDelete') }} </a>
                        <a-divider type="vertical" v-if="isGranted('Pages.Administration.OrganizationUnits.ManageUsers')"></a-divider>
                        <a :title="l('Refresh')" @click="clearFilterAndRefresh()">
                            <a-icon type="reload" /></a>
                    </a-col>
                </a-row>
                <!-- 数据部分 -->
                <div class="my-md">
                    <a-alert :type="'info'" :showIcon="true">
                        <template slot="message">
                            <span v-html="l('GridSelectedXItemsTips',selectedRowKeys.length)"></span>
                            <a @click="restCheckStatus()" class="ml-md">{{l('ClearEmpty')}}</a>
                            <a-divider type="vertical"></a-divider>
                            <a @click="clearFilterAndRefresh()">{{l('Refresh')}}</a>
                        </template>
                    </a-alert>
                </div>
                <a-table
                    @change="handleChange"
                    :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
                    :columns="columns"
                    :rowKey="data => data.id"
                    :dataSource="data">
                    <span slot="actions" v-if="isGranted('Pages.Administration.OrganizationUnits.ManageUsers')" slot-scope="text, record">
                        <a-popconfirm placement="top" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="removeMember(record)">
                            <template slot="title">
                                {{ l('RemoveUserFromOuWarningMessage', record.userName, selectTree.displayName) }}
                            </template>
                            <a class="table-delete">
                                <a-icon type="delete" />{{ l('Delete') }}</a>
                        </a-popconfirm>
                    </span>
                </a-table>
            </div>
        </div>
        <a-pagination
            class="pagination"
            size="middle"
            :total="totalItems"
            showSizeChanger
            showQuickJumper
            :showTotal="showTotalFun"
            @change="onChange"
            @showSizeChange="showSizeChange" />
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { OrganizationUnitServiceProxy } from "@/shared/service-proxies";
import { ModalHelper } from "@/shared/helpers";
import Bus from "@/shared/bus/bus";
import AddMemberComponent from "../add-member/add-member";

export default {
    name: "organization-unit-members-panel",
    mixins: [AppComponentBase],
    data() {
        return {
            spinning: false,
            _organizationUnitServiceProxy: null,
            selectedRowKeys: [],
            filteredInfo: null,
            sortedInfo: null,
            filterText: "",
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
            request: { maxResultCount: 10, skipCount: 0 },
            // 用户表格
            columns: [
                {
                    title: this.l("UserName"),
                    dataIndex: "userName",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "userName" }
                },
                {
                    title: this.l("AddedTime"),
                    dataIndex: "addedTimeStr",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "addedTime" }
                },
                {
                    title: this.l("Actions"),
                    dataIndex: "actions",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "actions" }
                }
            ],
            // 用户数据
            data: []
        };
    },
    computed: {},
    created() {
        this._organizationUnitServiceProxy = new OrganizationUnitServiceProxy(
            this.$apiUrl,
            this.$api
        );
        // 接受树结构传过来的选中item
        Bus.$on("selectedNode", this.getTree);
        // // 添加用户成功
        // Bus.$on("saveAddMemberSuccess", data => {
        //     if (data) {
        //         this.clearFilterAndRefresh();
        //     }
        // });
    },
    beforeDestroy() {
        Bus.$off("selectedNode");
    },
    methods: {
        /**
         * 选中树结构
         */
        getTree(data) {
            this.selectTree = data;
            this.getData();
        },
        /**
         * 拉取数据
         */
        getData() {
            this.spinning = true;
            this._organizationUnitServiceProxy
                .getPagedOrganizationUnitUsers(
                    this.selectTree.id,
                    this.filterText,
                    "",
                    this.request.maxResultCount,
                    this.request.skipCount
                )
                .finally(() => {
                    this.spinning = false;
                })
                .then(result => {
                    this.data = result.items.map(o => {
                        return {
                            ...o,
                            addedTimeStr: o.addedTime.format(
                                "YYYY-MM-DD HH:mm:ss"
                            )
                        };
                    });
                    this.totalItems = result.totalCount;
                    this.pagerange = [
                        (this.pageNumber - 1) * this.request.maxResultCount + 1,
                        this.pageNumber * this.request.maxResultCount
                    ];
                    this.totalPages = Math.ceil(
                        result.totalCount / this.request.maxResultCount
                    );
                    console.log(result);
                });
        },
        /**
         * 选中table
         */
        onSelectChange(selectedRowKeys) {
            console.log("selectedRowKeys changed: ", selectedRowKeys);
            this.selectedRowKeys = selectedRowKeys;
        },
        handleChange(pagination, filters, sorter) {
            console.log("Various parameters", pagination, filters, sorter);
            this.filteredInfo = filters;
            this.sortedInfo = sorter;
        },
        /**
         * 清除条件并刷新
         */
        clearFilterAndRefresh() {
            this.request = { maxResultCount: 10, skipCount: 0 };
            this.filterText = "";
            this.getData();
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
         * 移除用户
         * @param user 当前用户实体
         */
        removeMember(user) {
            console.log(user);
            const _ouId = parseInt(this.selectTree.id);
            this._organizationUnitServiceProxy
                .removeUser(user.id, _ouId)
                .then(() => {
                    this.$notification["success"]({
                        message: this.l("SuccessfullyRemoved")
                    });
                    this.clearFilterAndRefresh();
                    Bus.$emit("reloadOrganizationUnitTree", true);
                    // this.refreshGoFirstPage();
                    // this.memberRemoved.emit([user.id]);
                });
        },
        /**
         * 清空选择
         */
        restCheckStatus() {
            this.selectedRowKeys = [];
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
                        console.log(res);
                        const _ouId = parseInt(this.selectTree.id);
                        const ids = _.map(this.selectedRowKeys);
                        this._organizationUnitServiceProxy
                            .batchRemoveUserFromOrganizationUnit(_ouId, ids)
                            .then(() => {
                                // this.refreshGoFirstPage();
                                this.notify.success(
                                    this.l("SuccessfullyDeleted")
                                );
                                this.clearFilterAndRefresh();
                                Bus.$emit("reloadOrganizationUnitTree", true);
                                // this.memberRemoved.emit(ids);
                            });
                    }
                }
            );
        },
        /**
         * 增加用户
         */
        addUser() {
            ModalHelper.create(AddMemberComponent, {
                organizationUnitId: parseInt(this.selectTree.id)
            }).subscribe(res => {
                if (res) {
                    this.clearFilterAndRefresh();
                }
            });
        }
    }
};
</script>

<style scoped lang="less">
.user-container {
    border: 1px solid #e8e8e8;
    margin: 20px;
    padding: 20px;
    .table-delete {
        i {
            margin-right: 10px;
        }
    }
}
.opeattion-container {
    margin: 20px 0;
}
.pagination {
    margin: 10px auto;
    text-align: right;
}
</style>
