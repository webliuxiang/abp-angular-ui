<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">
                <span v-if="id">{{ l('Edit') }}</span>
                <span v-if="!id">{{ l('Create') }}</span>
            </div>
        </div>
        <a-form :form="form" @submit="handleSubmit">
            <!-- 标题 -->
            <a-form-item :label="l('WebSiteNoticeTitle')">
                <a-input
                    :placeholder="l('WebSiteNoticeTitleInputDesc')"
                    v-decorator="[
                        'title',
                        {
                            rules: [
                                {
                                    required: true,
                                    message: l('ThisFieldIsRequired'),
                                }
                            ],
                        },
                        ]" />
            </a-form-item>
            <!-- 内容 -->
            <a-form-item :label="l('WebSiteNoticeContent')">
                <a-textarea
                    :placeholder="l('WebSiteNoticeContentInputDesc')"
                    :auto-size="{ minRows: 3, maxRows: 5 }"
                    maxLength="3000"
                    v-decorator="[
                        'content',
                        {
                            rules: [
                                {
                                    required: true,
                                    message: l('ThisFieldIsRequired'),
                                }
                            ],
                        },
                        ]" />
            </a-form-item>
            <!-- 阅读数 -->
            <a-form-item :label="l('ViewCount')">
                <a-input-number v-decorator="[
                        'viewCount',
                        {
                            rules: [
                            ],
                        },
                        ]" :min="0" :max="1000000" :step="1" />
            </a-form-item>
            <a-form-item class="btn--container">
                <a-button type="button" @click="close()">
                    {{ l("Cancel") }}
                </a-button>
                <a-button type="primary" html-type="submit">
                    {{ l('Save') }}
                </a-button>
            </a-form-item>
        </a-form>
    </a-spin>
</template>

<script>
import { ModalComponentBase } from "@/shared/component-base";
import { ModalHelper } from "@/shared/helpers";
import { WebSiteNoticeServiceProxy } from "@/shared/service-proxies";

export default {
    mixins: [ModalComponentBase],
    name: "create-edit-bloggrollstype",
    data() {
        return {
            // 获取到的数据
            entity: {},
            spinning: false,
            _webSiteNoticeServiceProxy: ""
        };
    },
    components: {},
    beforeCreate() {
        this.form = this.$form.createForm(this, { name: "register" });
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._webSiteNoticeServiceProxy = new WebSiteNoticeServiceProxy(
            this.$apiUrl,
            this.$api
        );
    },
    mounted() {
        console.log(this.id);
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._webSiteNoticeServiceProxy
                .getForEdit(this.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    console.log(res);
                    this.entity = res.bannerAd;
                    this.form.setFieldsValue({
                        title: res.webSiteNotice.title,
                        content: res.webSiteNotice.content,
                        viewCount: res.webSiteNotice.viewCount
                    });
                });
        },
        /**
         * 提交表单
         */
        handleSubmit(e) {
            e.preventDefault();
            this.form.validateFieldsAndScroll((err, values) => {
                if (!err) {
                    this.spinning = true;
                    this._webSiteNoticeServiceProxy
                        .createOrUpdate({
                            webSiteNotice: Object.assign(values, {
                                id: this.id
                            })
                        })
                        .finally(() => {
                            this.spinning = false;
                        })
                        .then(res => {
                            this.$notification["success"]({
                                message: this.l("SavedSuccessfully")
                            });
                            this.success(true);
                        })
                        .catch(err => {
                            console.log(err);
                        });
                }
            });
        }
    }
};
</script>

<style lang="less" scoped>
/deep/.btn--container .ant-form-item-children {
    display: block;
    margin: 10px auto;
    text-align: center;
}
.pleaseSelect-text {
    font-size: 14px;
    padding: 0 14px;
    text-align: center;
    color: rgba(0, 0, 0, 0.65);
    font-weight: 400;
    line-height: 30px;
    margin-bottom: 0;
}
</style>