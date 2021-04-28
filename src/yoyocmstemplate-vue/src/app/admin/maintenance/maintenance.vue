<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('Maintenance')"></page-header>
        <a-card :bordered="false">
            <a-tabs default-active-key="1">
                <!-- 缓存 -->
                <a-tab-pane key="1" :tab="l('Caching')">
                    <a-card :title="l('CachesHeaderInfo')" :bordered="false">
                        <a-button slot="extra" type="primary" @click="clearAllCaches">
                            <a-icon type="delete" />
                            {{ l('ClearAll') }}
                        </a-button>
                        <a-table :columns="columns" :showHeader="false" :pagination="false" :data-source="cashtabledata">
                            <span slot="action" slot-scope="text, record">
                                <a @click="clearCache(record)">清空</a>
                            </span>
                        </a-table>
                    </a-card>
                </a-tab-pane>
                <!-- 网站日志 -->
                <a-tab-pane key="2" :tab="l('WebSiteLog')">
                    <a-card :title="l('WebSiteLogsHeaderInfo')" :bordered="false">
                        <a-button slot="extra" type="primary" @click="downloadWebLogs">
                            <a-icon type="download" />
                            {{ l('DownloadAll') }}
                        </a-button>
                        <a-button slot="extra" type="primary" @click="getWeblogData">
                            <a-icon type="reload" />
                            {{ l('Refresh') }}
                        </a-button>
                        <div class="web-log-view">
                            <p v-for="(item,index) in weblogList" :key="index">{{ item }}</p>
                        </div>
                    </a-card>
                </a-tab-pane>
            </a-tabs>
        </a-card>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import {
    HostCachingServiceProxy,
    WebSiteLogServiceProxy
} from "@/shared/service-proxies";
import { fileDownloadService } from "@/shared/utils";

export default {
    name: "maintenance",
    mixins: [AppComponentBase],
    data() {
        return {
            spinning: false,
            // 缓存
            _hostCachingServiceProxy: "",
            cashtabledata: [],
            columns: [
                {
                    dataIndex: "name",
                    key: "name",
                    slots: { title: "customTitle" },
                    scopedSlots: { customRender: "name" }
                },
                {
                    title: "Action",
                    key: "action",
                    scopedSlots: { customRender: "action" }
                }
            ],
            // web日志
            weblogList: [],
            _webSiteLogServiceProxy: ""
        };
    },
    created() {
        this._hostCachingServiceProxy = new HostCachingServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this._webSiteLogServiceProxy = new WebSiteLogServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.getCashData();
        this.getWeblogData();
    },
    methods: {
        /**
         * 获取混存数据
         */
        getCashData() {
            this.spinning = true;
            this._hostCachingServiceProxy
                .getAllCaches()
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.cashtabledata = res.items;
                });
        },
        /**
         * 获取网站log
         */
        getWeblogData() {
            this.spinning = true;
            this._webSiteLogServiceProxy
                .getLatestWebLogs()
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.weblogList = res.latestWebLogLines;
                });
        },
        /**
         * 单个清空缓存
         */
        clearCache(item) {
            this.spinning = true;
            this._hostCachingServiceProxy
                .clearCache({
                    id: item.name
                })
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.$notification["success"]({
                        message: this.l("CacheSuccessfullyCleared")
                    });
                    this.getCashData();
                });
        },
        /**
         * 清理全部缓存
         */
        clearAllCaches() {
            this.spinning = true;
            this._hostCachingServiceProxy
                .clearAllCaches()
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.$notification["success"]({
                        message: this.l("AllCachesSuccessfullyCleared")
                    });
                    this.getCashData();
                });
        },
        /**
         * 下载日志
         */
        downloadWebLogs() {
            this.spinning = true;
            this._webSiteLogServiceProxy
                .downloadWebLogs()
                .finally(() => {
                    this.spinning = false;
                })
                .then(result => {
                    fileDownloadService.downloadTempFile(result);
                });
        }
    }
};
</script>

<style scoped lang="less">
.web-log-view {
    max-height: 400px;
    overflow-y: scroll;
}
</style>
