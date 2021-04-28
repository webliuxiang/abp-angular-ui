<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('Tenants')"></page-header>
        <a-card :bordered="false">
            <a-form :layout="'vertical'" @submit.prevent="getData">
                <a-row :gutter="8">
                    <!-- 租户名称/租户代码 -->
                    <a-col :sm="12">
                        <a-form-item :label="l('TenantNameOrTenancyCode')">
                            <a-input-search
                                name="filterText"
                                :placeholder="l('SearchWithThreeDot')"
                                @search="getData"
                                enterButton
                                v-model="filterText"
                                v-decorator="['filterText']" />
                        </a-form-item>
                    </a-col>
                    <!-- 版本 -->
                    <a-col :sm="12">
                        <a-form-item :label="l('Edition')">
                            <edition-combo
                                :placeholder="l('PleaseSelect')"
                                :selectedEdition="selectedEdition"
                                @selectedEditionChange="selectedEditionChange($event)"></edition-combo>
                        </a-form-item>
                    </a-col>
                </a-row>
                <a-row :gutter="8" v-if="advancedFiltersVisible">
                    <!-- 订阅结束日期 -->
                    <a-col :sm="12">
                        <a-form-item :label="l('SubscriptionEndDateUtc')">
                            <a-range-picker
                                name="SubscriptionEndDateRange"
                                v-decorator="['subscribableDateRange']"
                                :placeholder="[l('StartDateTime'),l('EndDateTime')]"
                                @change="subscribableDateChange"
                                style="width:100%;" />
                        </a-form-item>
                    </a-col>
                    <!-- 创建时间 -->
                    <a-col :sm="12">
                        <a-form-item :label="l('CreationTime')">
                            <a-range-picker
                                name="CreationDateRange"
                                v-decorator="['createDateRange']"
                                :placeholder="[l('StartDateTime'),l('EndDateTime')]"
                                @change="createDateChange"
                                style="width:100%;" />
                        </a-form-item>
                    </a-col>
                </a-row>
            </a-form>
            <!-- 操作部分 -->
            <a-row :gutter="8">
                <a-col :md="20" :sm="12">
                    <a-button
                        :type="'primary'"
                        v-if="isGranted('Pages.Tenants.Create')"
                        @click="createNewTenant">
                        <a-icon type="plus" />
                        <span>{{l("CreateNewTenant")}}</span>
                    </a-button>
                    <a-button :type="'danger'" v-if="isGranted('Pages.Tenants.Delete')" @click="batchDelete">
                        <a-icon type="delete" />
                        <span>{{l("BatchDelete")}}</span>
                    </a-button>
                </a-col>

                <a-col :md="4" :sm="12" class="text-right">
                    <a @click="advancedFiltersVisible=!advancedFiltersVisible">
                        {{advancedFiltersVisible ? l('HideAdvancedFilters') : l('ShowAdvancedFilters')}}
                        <a-icon :type="advancedFiltersVisible ? 'up' : 'down'" />
                    </a>
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
                    :rowSelection="{selectedRowKeys: selectedRowKeys, onChange: onSelectChange}"
                    :columns="columns"
                    :rowKey="tableDatas => tableDatas.id"
                    :dataSource="tableData">
                    <p class="subscriptionEndUtc" slot="subscriptionEndUtc" slot-scope="record">{{ record }}</p>
                    <a class="isActive" slot="isActive" slot-scope="record">
                        <a-icon v-if="record" type="check" />
                        <a-icon v-if="!record" type="close" />
                    </a>
                    <p
                        class="creationTime"
                        slot="creationTime"
                        slot-scope="record">{{ record | moment('YYYY-MM-DD') }}</p>
                    <template slot-scope="text, record" slot="Actions">
                        <!-- 修改 -->
                        <a @click="edit(record.id)" v-if="isGranted('Pages.Tenants.Edit')">
                            <a-icon type="edit" />
                            <span>{{ l('Edit') }}</span>
                            <a-divider type="vertical" />
                        </a>
                        <!-- 删除 -->
                        <a-popconfirm
                            placement="top"
                            :okText="l('Ok')"
                            :cancelText="l('Cancel')"
                            @confirm="deleteItem(record)"
                            v-if="isGranted('Pages.Tenants.Delete')">
                            <template slot="title">{{ l('ConfirmDeleteWarningMessage') }}</template>
                            <a>
                                <a-icon type="delete" />
                                <span>{{ l('Delete') }}</span>
                                <a-divider type="vertical" />
                            </a>
                        </a-popconfirm>
                        <!-- 更多 -->
                        <a-dropdown>
                            <a class="ant-dropdown-link" @click="e => e.preventDefault()">
                                {{ l('More') }}
                                <a-icon type="down" />
                            </a>
                            <a-menu slot="overlay">
                                <a-menu-item
                                    v-if="isGranted('Pages.Tenants.Impersonation')"
                                    @click="tenantImpersonateLogin(record)">
                                    <a href="javascript:;">
                                        <a-icon type="login" />
                                        <span>{{ l('LoginAsThisTenant') }}</span>
                                    </a>
                                </a-menu-item>
                                <a-menu-item
                                    v-if="record.isActive && isGranted('Pages.Tenants.ChangeFeatures')"
                                    @click="changeTenantFeatures(record)">
                                    <a href="javascript:;">
                                        <a-icon type="codepen" />
                                        <span>{{ l('Features') }}</span>
                                    </a>
                                </a-menu-item>
                                <a-menu-item @click="unlockTenantAdminUser(record)">
                                    <a href="javascript:;">
                                        <a-icon type="unlock" />
                                        <span>{{ l('Unlock') }}</span>
                                    </a>
                                </a-menu-item>
                            </a-menu>
                        </a-dropdown>
                    </template>
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
import EditionCombo from "../shared/edition-combo/edition-combo.vue";
import { ModalHelper } from "@/shared/helpers";
// import {
//   PagedListingComponentBase,
//   PagedRequestDto
// } from '../../../shared/component-base/paged-listing-component-base';
import {
    TenantServiceProxy,
    TenantListDto,
    SubscribableEditionComboboxItemDto,
    EntityDtoOfInt64,
    NameValueDto,
    CommonLookupServiceProxy,
    CommonLookupFindUsersInput,
    EntityDto
} from "@/shared/service-proxies";
import { CreateTenantComponent } from "./create-tenant/index";
import { EditTenantComponent } from "./edit-tenant/index";
import { CommonLookupComponent } from "./common-lookup/index";
import { EditTenantFeaturesComponent } from "./edit-tenant-features/index";
import moment from "moment";
import { impersonationService } from "@/shared/auth/index";

export default {
    mixins: [AppComponentBase],
    name: "tenants",
    components: {
        EditionCombo
    },
    data() {
        return {
            _tenantService: null,
            selectedEdition: {
                value: 0
            },
            columns: [
                {
                    title: this.l("TenancyCodeName"),
                    dataIndex: "tenancyName",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "tenancyName" }
                },
                {
                    title: this.l("Name"),
                    dataIndex: "name",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "name" }
                },
                {
                    title: this.l("Edition"),
                    dataIndex: "editionDisplayName",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "editionDisplayName" }
                },
                {
                    title: this.l("SubscriptionEndUtc"),
                    dataIndex: "subscriptionEndUtc",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "subscriptionEndUtc" }
                },
                {
                    title: this.l("Active"),
                    dataIndex: "isActive",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "isActive" }
                },
                {
                    title: this.l("CreationTime"),
                    dataIndex: "creationTime",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "creationTime" }
                },
                {
                    title: this.l("Actions"),
                    dataIndex: "Actions",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "Actions" }
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
            request: { maxResultCount: 10, skipCount: 0 },
            // 选择多少项
            selectedRowKeys: [],
            selectedRows: [],
            advancedFiltersVisible: false, // 是否显示高级过滤
            // 请求参数
            filterText: undefined,
            editionId: undefined,
            subscribableDateRange: [null, null], // 订阅时间范围
            createDateRange: [null, null], // 创建时间范围
            tableData: [],
            // loading
            spinning: true
        };
    },
    created() {
        this._tenantService = new TenantServiceProxy(this.$apiUrl, this.$api);
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._tenantService
                .getPaged(
                    this.advancedFiltersVisible
                        ? this.subscribableDateRange[0] || undefined
                        : undefined, // 订阅结束时间开始
                    this.advancedFiltersVisible
                        ? this.subscribableDateRange[1] || undefined
                        : undefined, // 订阅结束时间结束
                    this.advancedFiltersVisible
                        ? this.createDateRange[0] || undefined
                        : undefined, // 创建时间开始
                    this.advancedFiltersVisible
                        ? this.createDateRange[1] || undefined
                        : undefined, // 创建时间结束
                    this.editionId || undefined, // 版本id
                    this.filterText, // 名称过滤字符串
                    this.sorting, // 排序字段
                    this.request.maxResultCount, // 最大数据量
                    this.request.skipCount // 跳过数据量
                )
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    console.log(res);
                    this.tableData = res.items;
                    this.tableData.map(item => {
                        item.subscriptionEndUtc = item.subscriptionEndUtc
                            ? moment(item.subscriptionEndUtc).format(
                                  "YYYY-MM-DD"
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
                })
                .catch(err => {
                    console.log(err);
                });
        },
        /**
         * 版本选择
         */
        selectedEditionChange(e) {
            this.editionId = e ? e.value : null;
            console.log(this.editionId);
            this.getData();
        },
        /**
         * 订阅日期改变
         */
        subscribableDateChange(e) {
            this.subscribableDateRange = e;
            this.getData();
        },
        /**
         * 创建时间改变
         */
        createDateChange(e) {
            this.createDateRange = e;
            this.getData();
        },
        /**
         * 新建租户
         */
        createNewTenant(e) {
            console.log(e);
            ModalHelper.create(CreateTenantComponent, null, {
                width: "400px"
            }).subscribe(res => {
                if (res) {
                    this.getData();
                }
            });
        },
        /**
         * 批量删除
         */
        batchDelete(e) {
            if (!this.selectedRowKeys || this.selectedRowKeys.length == 0) {
                return;
            }
            this.spinning = true;

            let batchDeleteInput = this.selectedRowKeys.map(o => {
                return new EntityDto({ id: o });
            });

            this._tenantService
                .batchDelete(batchDeleteInput)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.getData();
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
         * 单个删除
         */
        deleteItem(item) {
            this.spinning = true;
            this._tenantService
                .delete(item.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.getData();
                });
        },
        /**
         * 编辑
         */
        edit(id) {
            console.log(id);
            ModalHelper.create(
                EditTenantComponent,
                { entityId: id },
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
         * 使用此租户登录
         */
        tenantImpersonateLogin(item) {
            console.log(item);
            ModalHelper.create(
                CommonLookupComponent,
                { tenantId: item.id },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                if (res) {
                    this.impersonateUser(res, item.id);
                }
            });
        },
        /**
         * 编辑租户
         */
        changeTenantFeatures(item) {
            ModalHelper.create(
                EditTenantFeaturesComponent,
                { tenantId: item.id },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                if (res) {
                    // this.getData();
                }
            });
        },
        /**
         * 解锁
         */
        unlockTenantAdminUser(item) {
            console.log(item);
            this.spinning = true;
            this._tenantService
                .unlockTenantAdmin({ id: item.id })
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.$notification["success"]({
                        message: this.l("UnlockedTenandAdmin")
                    });
                })
                .catch(err => {
                    console.log(err);
                });
        },
        impersonateUser(item, id) {
            impersonationService.impersonate(parseInt(item.value), id);
        }
    }
};
</script>

<style scoped>
@import "./tenants.less";
</style>
