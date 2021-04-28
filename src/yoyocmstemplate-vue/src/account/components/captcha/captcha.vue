<template>
    <div>
        <a-row>
            <a-col :span="12">
                <a-input :placeholder="placeholder" @click="initImg()" @keyup="onKey($event)" v-model="cValue">
                    <i class="anticon-picture" slot="prefix"></i>
                </a-input>
            </a-col>
            <a-col :span="12">
                <img :src="captchaUrl" @click="clearImg()" v-if="captchaUrl"/>
            </a-col>
        </a-row>
    </div>
</template>

<script>
    import {appSessionService} from '@/shared/abp';
    import {AppComponentBase} from "@/shared/component-base";

    export default {
        name: "captcha",
        mixins: [AppComponentBase],
        props: ['type', 'primaryKey', 'placeholder','value'],
        model: {
            prop: 'value',
            event: 'change',
        },
        data() {
            return {
                defaultValue: '',
                captchaUrlVal: '',
                oldPrimaryKey: '',
                primaryKeyVal: '',
                cValue: '',
            }
        },
        computed: {
            captchaUrl(){
                return this.captchaUrlVal;
            },
            checkKey() {
                return this.oldPrimaryKey && this.oldPrimaryKey === this.primaryKeyVal;
            }
        },
        mounted() {

        },
        methods: {
            initImg() { // 初始化验证码图片
                if (!this.primaryKeyVal || this.primaryKeyVal === '' || this.captchaUrl || this.checkKey) {
                    return;
                }
                this.clearImg();
            },
            clearImg() { // 重置验证码图片
                if (!this.primaryKeyVal || this.primaryKeyVal === '') {
                    // 未输入验证码key
                    return;
                }

                let tid = appSessionService.tenantId;
                if (!tid) {
                    tid = '';
                }

                this.oldPrimaryKey = this.primaryKeyVal;
                let timestamp = new Date().getTime();
                this.captchaUrlVal = `${this.$apiUrl}/api/TokenAuth/GenerateVerification?name=${this.primaryKeyVal}&t=${this.type}&tid=${tid}&timestamp=${timestamp}`;
            },
            onKey(e) { // tab触发到此控件
                if (e.key === 'Tab') {
                    this.initImg();
                }
            }
        },
        watch: {
            primaryKey(val) {
                this.primaryKeyVal = val;
            },
            cValue(val) {
                this.$emit('change', val);
            },
        }
    }
</script>

<style lang="less" scoped>
    @import "./captcha.less";
</style>
