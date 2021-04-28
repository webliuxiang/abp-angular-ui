<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('AuditLogs')"></page-header>
        <a-card :bordered="false">
            <a-form :layout="'vertical'" @submit.prevent="getData">
                <a-row :gutter="8">
                    <!-- 用户名 -->
                    <a-col :sm="24">
                        <a-form-item :label="l('UserName')">
                            <a-input-search
                                name="username"
                                :placeholder="l('UserName')"
                                @search="getData"
                                enterButton
                                v-model="username"
                                v-decorator="['username']" />
                        </a-form-item>
                    </a-col>
                    <!-- 错误状态 -->
                    <a-col :sm="12">
                        <a-form-item :label="l('ErrorState')">
                            <a-select v-model="hasException">
                                <a-select-option value="">{{ l('All') }}</a-select-option>
                                <a-select-option value="false">{{ l('Success') }}</a-select-option>
                                <a-select-option value="true">{{ l('HasError') }}</a-select-option>
                            </a-select>
                        </a-form-item>
                    </a-col>
                    <!--日期范围 -->
                    <a-col :sm="12">
                        <a-form-item :label="l('DateRange')">
                            <a-range-picker :placeholder="[l('StartDateTime'), l('EndDateTime')]" v-model="startToEndDate" />
                        </a-form-item>
                    </a-col>
                    <!-- 服务 -->
                    <a-col :sm="12" v-if="advancedFiltersVisible">
                        <a-form-item :label="l('Service')">
                            <a-input
                                :placeholder="l('Service')"
                                v-model="serviceName"
                                v-decorator="['serviceName']" />
                        </a-form-item>
                    </a-col>
                    <!-- 持续时间 -->
                    <a-col :sm="12" v-if="advancedFiltersVisible">
                        <a-form-item :label="l('Service')">
                            <a-input-number id="MinExecutionDuration" :min="0" :placeholder="l('MinExecutionDuration')" style="text-align: center; width: 152px;" :max="86400000" :step="1" v-model="minExecutionDuration" />
                            ～
                            <a-input-number id="MaxExecutionDuration" :min="0" :placeholder="l('MaxExecutionDuration')" style="text-align: center; width: 152px;" :max="86400000" :step="1" v-model="maxExecutionDuration" />
                        </a-form-item>
                    </a-col>
                    <!-- 方法名 -->
                    <a-col :sm="12" v-if="advancedFiltersVisible">
                        <a-form-item :label="l('MethodName')">
                            <a-input
                                :placeholder="l('MethodName')"
                                v-model="methodName"
                                v-decorator="['methodName']" />
                        </a-form-item>
                    </a-col>
                    <!-- 浏览器 -->
                    <a-col :sm="12" v-if="advancedFiltersVisible">
                        <a-form-item :label="l('Browser')">
                            <a-input
                                :placeholder="l('Browser')"
                                v-model="browserInfo"
                                v-decorator="['browserInfo']" />
                        </a-form-item>
                    </a-col>
                    <!-- -->
                </a-row>
            </a-form>
            <!-- 操作部分 -->
            <a-row :gutter="8">
                <a-col :md="20" :sm="12">
                </a-col>
                <!-- 显示暴击过滤 -->
                <a-col :md="4" :sm="12" :offset="20" class="text-right">
                    <a @click="advancedFiltersVisible=!advancedFiltersVisible">
                        {{advancedFiltersVisible ? l('HideAdvancedFilters') : l('ShowAdvancedFilters')}}
                        <a-icon :type="advancedFiltersVisible ? 'up' : 'down'" />
                    </a>
                </a-col>
            </a-row>
            <!-- table -->
            <a-row class="table--container">
                <a-table
                    class="list-table"
                    @change="getData"
                    :pagination="false"
                    :columns="columns"
                    :rowKey="tableDatas => tableDatas.id"
                    :dataSource="tableData">
                    <!-- 异常 -->
                    <p class="roles" slot="exceptionType" slot-scope="text, record">
                        <a-icon v-if="record.exception" class="text-warning" type="exclamation-circle" />
                        <a-icon v-if="!record.exception" class="text-green" type="check-circle" />
                    </p>
                    <span slot="actions" slot-scope="text, record">
                        <!-- 查看详情 -->
                        <a class="table-edit" @click="getDetails(record)">
                            <a-icon type="search" />{{ l('ViewDetails') }}</a>
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
import { AuditLogServiceProxy } from "@/shared/service-proxies";
import AuditLogsDetails from "./audit-logs-detail/audit-logs-detail";
import moment from "moment";

export default {
    mixins: [AppComponentBase],
    name: "audit-logs",
    components: {
        AuditLogsDetails
    },
    data() {
        return {
            // 是否显示高级过滤
            advancedFiltersVisible: false,
            // 表格
            columns: [
                {
                    title: this.l("Time"),
                    dataIndex: "executionTimeStr",
                    sorter: true,
                    align: "executionTimeStr",
                    width: 100,
                    scopedSlots: { customRender: "executionTimeStr" }
                },
                {
                    title: this.l("UserName"),
                    dataIndex: "userName",
                    align: "center",
                    scopedSlots: { customRender: "userName" }
                },
                {
                    title: this.l("Service"),
                    dataIndex: "serviceName",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "serviceName" }
                },
                {
                    title: this.l("Action"),
                    dataIndex: "methodName",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "methodName" }
                },
                {
                    title: this.l("Duration"),
                    dataIndex: "executionDuration",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "executionDuration" }
                },
                {
                    title: this.l("IpAddress"),
                    dataIndex: "clientIpAddress",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "clientIpAddress" }
                },
                {
                    title: this.l("Client"),
                    dataIndex: "clientName",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "clientName" }
                },
                {
                    title: this.l("Browser"),
                    dataIndex: "browserInfo",
                    align: "center",
                    width: 100,
                    scopedSlots: { customRender: "browserInfo" }
                },
                {
                    title: this.l("ExceptionType"),
                    dataIndex: "exceptionType",
                    align: "center",
                    scopedSlots: { customRender: "exceptionType" }
                },
                {
                    title: this.l("Actions"),
                    dataIndex: "actions",
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
            selectedRows: [],
            tableData: [],
            // loading
            spinning: false,
            // 用户名
            username: "",
            // 错误状态
            hasException: "",
            // 日期范围
            startToEndDate: [],
            // 服务
            serviceName: "",
            // 持续时间
            minExecutionDuration: "",
            maxExecutionDuration: "",
            // 方法名
            methodName: "",
            // 浏览器
            browserInfo: "",
            _auditLogServiceProxy: ""
        };
    },
    created() {
        this._auditLogServiceProxy = new AuditLogServiceProxy(
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
            console.log(this.startToEndDate);
            this.spinning = true;
            this._auditLogServiceProxy
                .getPagedAuditLogs(
                    this.startToEndDate[0] === null
                        ? undefined
                        : this.startToEndDate[0],
                    this.startToEndDate[1] === null
                        ? undefined
                        : this.startToEndDate[1],
                    this.username,
                    this.serviceName,
                    this.methodName,
                    this.browserInfo,
                    this.hasException,
                    this.minExecutionDuration,
                    this.maxExecutionDuration,
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
                        item.executionTimeStr = item.executionTime
                            ? moment(item.executionTime).format(
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
                    console.log(this.tableData);
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
         * 查看详情
         */
        getDetails(item) {
            ModalHelper.create(
                AuditLogsDetails,
                { auditLog: item },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                console.log(res);
                if (res) {
                    this.getData();
                }
            });
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
.table--container {
    margin-top: 20px;
}
</style>
