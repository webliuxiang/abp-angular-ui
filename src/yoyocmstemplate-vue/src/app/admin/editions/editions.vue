<template>
    <div>
        <page-header :title="l('Editions')"></page-header>
        <!-- 表格部分 -->
        <a-card :bordered="false">
            <!-- 操作部分 -->
            <a-row :gutter="8">
                <a-col :md="20" :sm="12" class="btn-gutter">
                    <a-button @click="create()" type="primary" v-if="isGranted('Pages.Editions.Create')">
                        <i class="anticon anticon-plus"></i>
                        <span>{{l("CreateNewEdition")}}</span>
                    </a-button>
                    <a-button
                        @click="batchDelete()"
                        type="danger"
                        v-if="isGranted('Pages.Editions.Delete')">
                        <i class="anticon anticon-delete"></i>
                        <span>{{l("BatchDelete")}}</span>
                    </a-button>
                </a-col>
            </a-row>
            <!-- 数据部分 -->
            <div class="my-md">
                <a-alert :showIcon="true" type="info">
                    <template slot="message">
                        <span v-html="l('GridSelectedXItemsTips',selectedRowKeys.length)"></span>
                        <a @click="restCheckStatus(dataList)" class="ml-md">
                            {{l('ClearEmpty')}}
                        </a>
                        <a-divider type="vertical"></a-divider>
                        <a @click="refresh()">
                            {{l('Refresh')}}
                        </a>
                    </template>
                </a-alert>
            </div>
            <a-row class="my-md">
                <a-table :dataSource="dataList" :loading="isTableLoading"
                    :rowSelection="{selectedRowKeys: selectedRowKeys, onChange: onSelectChange}"
                    @change="tableChange">
                    <a-table-column :title="l('Edition')" dataIndex="name" key="name" sorter="true">

                    </a-table-column>
                    <a-table-column :title="l('EditionName')" dataIndex="displayName"
                        key="displayName" sorter="true"></a-table-column>
                    <a-table-column :title="l('CreationTime')" dataIndex="creationTimeStr"
                        key="creationTime" sorter="true"></a-table-column>
                    <a-table-column :title="l('Actions')" key="action">
                        <template slot-scope="text, record">
                            <span>
                                <a @click="edit(record.id)">
                                    <a-icon type="edit" />
                                    <span>{{l('Edit')}}</span>
                                </a>
                                <a-divider type="vertical" />
                                <a-popconfirm

                                    :cancelText="l('Cancel')"
                                    :okText="l('Ok')"
                                    :title="l('ConfirmDeleteWarningMessage')"
                                    @confirm="onDelete(record)">
                                    <a href="#">
                                        <a-icon type="delete" />
                                        <span>{{l('Delete')}}</span>
                                    </a>
                                </a-popconfirm>
                            </span>
                        </template>
                    </a-table-column>

                </a-table>
            </a-row>
        </a-card>
    </div>
</template>

<script>
import { PagedListingComponentBase } from "@/shared/component-base";
import { ModalHelper } from "@/shared/helpers";
import { EditionServiceProxy } from "@/shared/service-proxies";
import CreateOrEditEdition from "./create-or-edit-edition/create-or-edit-edition";

export default {
    name: "editions",
    mixins: [PagedListingComponentBase],
    components: {},
    data() {
        return {
            editionService: null
        };
    },
    created() {
        this.editionService = new EditionServiceProxy(this.$apiUrl, this.$api);
    },
    methods: {
        create() {
            ModalHelper.create(CreateOrEditEdition, {
                editionId: undefined
            }).subscribe(res => {
                if (res) {
                    this.refresh();
                }
            });
        },
        edit(entityId) {
            ModalHelper.create(CreateOrEditEdition, {
                editionId: entityId
            }).subscribe(res => {
                if (res) {
                    this.refresh();
                }
            });
        },
        onDelete(entity) {
            abp.message.confirm(
                this.l("EditionDeleteWarningMessage", entity.displayName),
                isConfirmed => {
                    if (isConfirmed) {
                        this.editionService
                            .deleteEdition(entity.id)
                            .then(() => {
                                this.refresh();
                                this.notify.success(
                                    this.l("SuccessfullyDeleted")
                                );
                            });
                    }
                }
            );
        },
        forceRefresh() {
            this.refreshGoFirstPage();
        },
        fetchDataList(request, pageNumber, finishedCallback) {
            // 获取数据抽象接口，必须实现
            this.editionService
                .getEditions()
                .finally(() => {
                    finishedCallback();
                })
                .then(result => {
                    this.dataList = result.items.map(o => {
                        return {
                            key: o.id,
                            ...o,
                            creationTimeStr: o.creationTime.format(
                                "YYYY-MM-DD HH:mm:ss"
                            )
                        };
                    });
                });
        },
        /**
         * table选择事件
         */
        onSelectChange(selectedRowKeys, selectedRows) {
            this.selectedRowKeys = selectedRowKeys;
            console.log(selectedRowKeys);
        },
        /**
         * 批量删除
         * todo
         * 接口暂缺
         */
        batchDelete() {}
    }
};
</script>

<style scoped lang="less">
.ant-table-body i {
    margin-right: 10px;
}
</style>
