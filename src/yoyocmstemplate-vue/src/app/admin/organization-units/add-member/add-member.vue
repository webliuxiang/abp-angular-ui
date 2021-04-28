<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">
                <a-icon type="team" />{{ l('SelectUsers') }}</div>
        </div>
        <!-- 搜索框 -->
        <a-input-search
            name="filterText"
            @search="getData"
            :placeholder="l('SearchWithThreeDot')"
            enterButton
            v-model="filterText" />
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
        <!-- table -->
        <a-table
            @change="handleChange"
            :pagination="false"
            :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
            :columns="columns"
            :rowKey="data => data.value"
            :dataSource="data">
        </a-table>
        <!-- 分页 -->
        <a-pagination
            class="pagination"
            size="middle"
            :total="totalItems"
            showSizeChanger
            showQuickJumper
            :showTotal="showTotalFun"
            @change="onChange"
            @showSizeChange="showSizeChange" />
        <!-- 按钮 -->
        <div class="modal-footer">
            <a-button :disabled="saving" @click="close()" type="button">
                <a-icon type="close-circle" />
                {{ l('Cancel') }}
            </a-button>
            <a-button :loading="saving" :type="'primary'" @click="handleSubmit()">
                <a-icon type="save" />
                {{ l('Save') }}
            </a-button>
        </div>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { ModalComponentBase } from "@/shared/component-base";
import { OrganizationUnitServiceProxy } from "@/shared/service-proxies";
import Bus from "@/shared/bus/bus";

export default {
    name: "add-member",
    mixins: [AppComponentBase, ModalComponentBase],
    data() {
        return {
            spinning: false,
            // 搜索
            filterText: "",
            // 选中item
            selectedRowKeys: [],
            // 分页
            request: { maxResultCount: 10, skipCount: 0 },
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
            _organizationUnitServiceProxy: null,
            // 用户表格
            columns: [
                {
                    title: this.l("Name"),
                    dataIndex: "name",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "name" }
                }
            ],
            // 用户数据
            data: []
        };
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._organizationUnitServiceProxy = new OrganizationUnitServiceProxy(
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
            this._organizationUnitServiceProxy
                .findUsers(
                    Object.assign(
                        {
                            organizationUnitId: this.organizationUnitId,
                            filterText: this.filterText
                        },
                        this.request
                    )
                )
                .finally(() => {
                    this.spinning = false;
                })
                .then(result => {
                    this.data = result.items.map(o => {
                        return {
                            ...o,
                            id: parseInt(o.value)
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
         * 提交表单
         */
        handleSubmit() {
            const selectCount = this.selectedRowKeys.length;
            if (selectCount <= 0) {
                abp.message.warn(this.l("PleaseSelectAtLeastOneItem"));
                return;
            }
            this.saving = true;
            this.spinning = true;
            let input = {
                organizationUnitId: this.organizationUnitId,
                userIds: this.selectedRowKeys
            };
            this._organizationUnitServiceProxy
                .addUsers(input)
                .finally(() => {
                    this.saving = false;
                    this.spinning = false;
                })
                .then(() => {
                    this.notify.success(this.l("SavedSuccessfully"));
                    this.success(input.userIds);
                });
        },
        /**
         * 清空选择
         */
        restCheckStatus() {
            this.selectedRowKeys = [];
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
.modal-header {
    .anticon {
        margin-right: 10px;
    }
}
.pagination {
    margin: 10px auto;
    text-align: right;
}
</style>
