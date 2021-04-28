<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">{{l('FindUser')}}</div>
        </div>
        <a-input-search
            :placeholder="l('SearchWithThreeDot')"
            v-model="filterText"
            @search="refresh"
            :enterButton="l('Search')"
            size="large" />
        <a-table class="lookup-table" bordered :pagination="false"
            :rowKey="tableData => tableData.value"
            :columns="columns" :dataSource="tableData">
            <a-button slot="select" slot-scope="text, record" type="primary" @click="selectItem(record)">
                <a-icon type="select" />{{ l('Select') }}</a-button>
        </a-table>
        <a-pagination
            class="pagination"
            showSizeChanger
            @change="onChange"
            @showSizeChange="onShowSizeChange"
            :defaultCurrent="1"
            :total="totalCount" />
    </a-spin>
</template>

<script>
import { ModalComponentBase } from "@/shared/component-base";
import { CommonLookupServiceProxy } from "@/shared/service-proxies";

export default {
    name: "common-lookup",
    mixins: [ModalComponentBase],
    data() {
        return {
            spinning: false,
            // 输入框
            filterText: "",
            // 表格
            columns: [
                {
                    title: this.l("Select"),
                    dataIndex: "select",
                    key: "select",
                    width: 150,
                    scopedSlots: { customRender: "select" }
                },
                {
                    title: this.l("Name"),
                    dataIndex: "name",
                    key: "name"
                }
            ],
            tableData: [],
            // 分页
            page: 1,
            pageSize: 10,
            totalCount: 0
        };
    },
    created() {
        this._commonLookupServiceProxy = new CommonLookupServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.fullData(); // 模态框必须,填充数据到data字段
    },
    mounted() {
        console.log(this.tenantId);
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._commonLookupServiceProxy
                .findUsers({
                    tenantId: this.tenantId,
                    maxResultCount: this.pageSize,
                    skipCount: (this.page - 1) * this.pageSize,
                    filterText: this.filterText
                })
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.totalCount = res.totalCount;
                    this.tableData = res.items;
                })
                .catch(err => {
                    console.log(err);
                });
        },
        /**
         * 搜索
         */
        refresh() {
            this.pageSize = 10;
            this.page = 1;
            this.getData();
        },
        /**
         * 分页
         */
        onShowSizeChange(current, pageSize) {
            this.pageSize = pageSize;
            this.getData();
        },
        onChange(page, pageSize) {
            this.page = page;
            this.getData();
        },
        /**
         * 选择
         */
        selectItem(item) {
            this.success(item);
            // if (!boolOrPromise) {
            //   return;
            // }
            // if (boolOrPromise === true) {
            //   this.itemSelected.emit(item);
            //   this.success(item);
            //   return;
            // }
            // // assume as observable
            // (boolOrPromise as Observable<boolean>).subscribe(result => {
            //   if (result) {
            //     this.itemSelected.emit(item);
            //     this.success(item);
            //   }
            // });
        }
    }
};
</script>

<style lang="less" scoped>
.lookup-table {
    margin: 20px auto;
}
.pagination {
    text-align: right;
}
</style>
