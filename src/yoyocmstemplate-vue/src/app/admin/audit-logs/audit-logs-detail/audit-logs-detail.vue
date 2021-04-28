<template>
    <a-spin :spinning="spinning">
        <!-- 标题 -->
        <div class="modal-header">
            <div class="modal-title">
                <span v-if="auditLog">{{ l('AuditLogDetail') }}</span>
            </div>
        </div>
        <!-- 用户信息 -->
        <h3>{{l("UserInformations")}}</h3>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="4">
                <div class="gutter-box">{{l("UserName")}}:</div>
            </a-col>
            <a-col class="gutter-row" :span="10">
                <div class="gutter-box">{{ auditLog.userName }}</div>
            </a-col>
        </a-row>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="4">
                <div class="gutter-box">{{l("IpAddress")}}:</div>
            </a-col>
            <a-col class="gutter-row" :span="10">
                <div class="gutter-box">{{ auditLog.clientIpAddress }}</div>
            </a-col>
        </a-row>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="4">
                <div class="gutter-box">{{l("Client")}}:</div>
            </a-col>
            <a-col class="gutter-row" :span="10">
                <div class="gutter-box">{{ auditLog.clientName }}</div>
            </a-col>
        </a-row>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="4">
                <div class="gutter-box">{{l("Browser")}}:</div>
            </a-col>
            <a-col class="gutter-row" :span="10">
                <div class="gutter-box">{{ auditLog.browserInfo }}</div>
            </a-col>
        </a-row>

        <!-- 操作信息 -->
        <br>
        <h3>{{l("ActionInformations")}}</h3>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="4">
                <div class="gutter-box">{{l("Service")}}:</div>
            </a-col>
            <a-col class="gutter-row" :span="10">
                <div class="gutter-box">{{ auditLog.serviceName }}</div>
            </a-col>
        </a-row>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="4">
                <div class="gutter-box">{{l("Action")}}:</div>
            </a-col>
            <a-col class="gutter-row" :span="10">
                <div class="gutter-box">{{ auditLog.methodName }}</div>
            </a-col>
        </a-row>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="4">
                <div class="gutter-box">{{l("Time")}}:</div>
            </a-col>
            <a-col class="gutter-row" :span="10">
                <div class="gutter-box">{{ getExecutionTime() }}</div>
            </a-col>
        </a-row>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="4">
                <div class="gutter-box">{{l("Duration")}}:</div>
            </a-col>
            <a-col class="gutter-row" :span="10">
                <div class="gutter-box">{{ auditLog.executionDuration }}ms</div>
            </a-col>
        </a-row>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="4">
                <div class="gutter-box">{{l("Parameters")}}:</div>
            </a-col>
            <a-col class="gutter-row" :span="10">
                <div class="gutter-box">{{ auditLog.parameters }}</div>
            </a-col>
        </a-row>

        <!-- 数据 -->
        <br>
        <h3>{{l("CustomData")}}</h3>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="4">
                <div class="gutter-box">{{l("None")}}:</div>
            </a-col>
            <a-col class="gutter-row" :span="10">
                <div class="gutter-box">{{ auditLog.customData }}</div>
            </a-col>
        </a-row>

        <!-- 错误状态 -->
        <br>
        <h3>{{l("ErrorState")}}</h3>
        <a-row :gutter="8">
            <a-col class="gutter-row" :span="10">
                <p v-if="!auditLog.exception">
                    <a-badge status="success" />{{ l('Success') }}</p>
                <p v-if="auditLog.exception">
                    <a-badge status="error" />{{ l('Error') }}</p>
            </a-col>
        </a-row>

        <!-- 按钮 -->
        <div class="modal-footer">
            <a-button :disabled="saving" @click="close()" type="button">
                <a-icon type="close-circle" />
                {{ l('Cancel') }}
            </a-button>
        </div>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { ModalComponentBase } from "@/shared/component-base";
import moment from "moment";

export default {
    name: "audit-logs-detail",
    mixins: [AppComponentBase, ModalComponentBase],
    data() {
        return {
            spinning: false
        };
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
    },
    methods: {
        // 时间
        getExecutionTime() {
            return (
                moment(this.auditLog.executionTime).fromNow() +
                " (" +
                moment(this.auditLog.executionTime).format(
                    "YYYY-MM-DD HH:mm:ss"
                ) +
                ")"
            );
        }
    }
};
</script>

<style scoped>
</style>
