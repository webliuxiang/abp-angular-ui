<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">
                <span v-if="id">{{ l('Edit') }}</span>
                <span v-if="!id">{{ l('Create') }}</span>
            </div>
        </div>
        <a-form :form="form" @submit="handleSubmit">
            <!-- 博客名称 -->
            <a-form-item :label="l('BlogName')">
                <a-input
                    :placeholder="l('BlogNameInputDesc')"
                    v-decorator="[
                        'name',
                        {
                            rules: [
                                {
                                    required: true,
                                    message: l('BlogNameInputDesc'),
                                },
                                {
                                    min: 3,
                                    message: l('MinLength', 3),
                                }
                            ],
                        },
                        ]" />
            </a-form-item>

            <!-- 博客短名称 -->
            <a-form-item :label="l('BlogShortName')">
                <a-input
                    :placeholder="l('BlogShortNameInputDesc')"
                    v-decorator="[
                        'shortName',
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

            <!-- 博客描述 -->
            <a-form-item :label="l('BlogDescription')">
                <a-textarea
                    :placeholder="l('BlogDescriptionInputDesc')"
                    :auto-size="{ minRows: 3, maxRows: 5 }"
                    maxLength="3000"
                    v-decorator="[
                        'description',
                        {
                            rules: [
                            ],
                        },
                        ]" />
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
import { BlogServiceProxy } from "@/shared/service-proxies";

export default {
    mixins: [ModalComponentBase],
    name: "create-or-edit-blogrolls",
    data() {
        return {
            // 获取到的数据
            entity: {},
            spinning: false,
            _blogServiceProxy: ""
        };
    },
    components: {},
    beforeCreate() {
        this.form = this.$form.createForm(this, { name: "register" });
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._blogServiceProxy = new BlogServiceProxy(this.$apiUrl, this.$api);
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
            this._blogServiceProxy
                .getForEdit(this.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.entity = res.blog;
                    this.form.setFieldsValue({
                        name: this.entity.name,
                        shortName: this.entity.shortName,
                        description: this.entity.description
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
                    this._blogServiceProxy
                        .createOrUpdate({
                            blog: Object.assign(values, { id: this.id })
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