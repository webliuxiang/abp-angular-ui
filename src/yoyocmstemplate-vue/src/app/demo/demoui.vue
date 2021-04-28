<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('DemoManagement')"></page-header>
        <a-card :bordered="false" :title="l('AddressSelection')">
            <x-address-linkage :placeHolder="l('MouseMovementLoading')" :mouseclickplaceHolder="l('MouseClickLoading')"></x-address-linkage>
        </a-card>
        <a-card :bordered="false" :title="l('DateAndTime')">
            <a-date-picker placeholder="请选择日期" />
            <br>
            <br>
            <a-month-picker placeholder="请选择日期" />
            <br>
            <br>
            <a-date-picker mode="year" placeholder="请选择日期" />
            <br>
            <br>
            <a-range-picker :placeholder="['开始日期','结束日期']" />
            <br>
            <br>
            <a-week-picker placeholder="请选择日期" />
        </a-card>
        <a-card :bordered="false" :title="l('DateAndTime')">
            <a-date-picker mode="year" placeholder="请选择日期" />
            <br>
            <br>
            <a-week-picker placeholder="请选择日期" />
            <br>
            <br>
            <a-range-picker :placeholder="['开始日期','结束日期']" />
            <br>
            <br>
            <a-month-picker placeholder="请选择日期" />
            <br>
            <br>
            <a-date-picker placeholder="请选择日期" />
            <br>
            <br>
            <a-time-picker :default-open-value="moment('00:00:00', 'HH:mm:ss')" placeholder="请选择时间" />
        </a-card>
        <a-card :bordered="false" title="支付Demo">
            <a-button type="primary" @click="onClickPay">
                赏口饭
            </a-button>
        </a-card>
        <a-card :bordered="false" :title="l('DemoMarkDown')">
            <mavon-editor
                v-model="content"
                ref="md"
                @change="Markdownchange"
                style="min-height: 300px" />
            <br>
            <a-button type="primary">{{ l('Save') }}</a-button>
            <a-button @click="resetMarkdown($event)">
                {{ l('Reset') }}
            </a-button>
        </a-card>
        <a-card :bordered="false" :title="l('ExportToExcel')">
            <a-button @click="exportToExcel">
                <a-icon type="file-excel" />{{ l('ExportToExcel') }}</a-button>
            <br><br>
            <a-table :columns="columns" :data-source="tabledata">
                <span slot="rolesStr" slot-scope="text, record">
                    <a-tag v-for="item in record.roles" :key="item.roleId">{{ item.roleName }}</a-tag>
                </span>
            </a-table>
        </a-card>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import XAddressLinkage from "./x-address-linkage/x-address-linkage";
import {
    PurchaseServiceProxy,
    UserServiceProxy
} from "@/shared/service-proxies";
import { fileDownloadService } from "@/shared/utils";
// 导入组件 及 组件样式
import { mavonEditor } from "mavon-editor";
import "mavon-editor/dist/css/index.css";
import moment from "moment";

export default {
    name: "demoui",
    mixins: [AppComponentBase],
    components: {
        XAddressLinkage,
        mavonEditor
    },
    data() {
        return {
            spinning: false,
            _purchaseServiceProxy: "",
            _userServiceProxy: "",
            _fileDownloadService: "",
            content: "", // 输入的markdown
            html: "", // 及时转的html
            skipCount: 0,
            tabledata: [],
            columns: [
                {
                    title: this.l("UserName"),
                    dataIndex: "userName",
                    key: "userName"
                },
                {
                    title: this.l("Roles"),
                    dataIndex: "rolesStr",
                    key: "rolesStr",
                    align: "center",
                    scopedSlots: { customRender: "rolesStr" }
                },
                {
                    title: this.l("CreationTime"),
                    dataIndex: "creationTimeStr",
                    key: "creationTimeStr"
                }
            ]
        };
    },
    created() {
        this._purchaseServiceProxy = new PurchaseServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this._userServiceProxy = new UserServiceProxy(this.$apiUrl, this.$api);
        this._fileDownloadService = fileDownloadService;
        this.getUser();
    },
    methods: {
        getUser() {
            this.spinning = true;
            this._userServiceProxy
                .getPaged(
                    [],
                    undefined,
                    undefined,
                    undefined,
                    undefined,
                    "",
                    "",
                    20,
                    this.skipCount
                )
                .finally(() => {
                    this.spinning = false;
                })
                .then(result => {
                    this.tabledata = result.items;
                    this.tabledata.map(item => {
                        item.creationTimeStr = item.creationTime
                            ? moment(item.creationTime).format(
                                  "YYYY-MM-DD HH:mm:ss"
                              )
                            : "-";
                    });
                    console.log(result);
                });
        },
        moment,
        /**
         * 赏扣饭
         */
        onClickPay() {
            this.spinning = true;
            this._purchaseServiceProxy
                .createPay({
                    orderCode: "123456789AAAA"
                })
                .finally(() => {
                    this.spinning = false;
                })
                .then(result => {
                    window.open(result);
                });
        },
        /**
         * markdown
         * 所有操作都会被解析重新渲染
         */
        Markdownchange(value, render) {
            this.html = render;
        },
        resetMarkdown() {
            this.content = "";
            this.html = "";
        },
        /**
         * 导出
         */
        exportToExcel() {
            this.spinning = true;
            this._userServiceProxy
                .getUsersToExcel()
                .finally(() => {
                    this.spinning = false;
                })
                .then(result => {
                    this._fileDownloadService.downloadTempFile(result);
                });
        }
    }
};
</script>

<style scoped>
</style>
