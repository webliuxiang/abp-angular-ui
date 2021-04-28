<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">
                <span v-if="id">{{ l('Edit') }}</span>
                <span v-if="!id">{{ l('Create') }}</span>
            </div>
        </div>
        <a-form :form="form" @submit="handleSubmit">
            <!-- 标签名称 -->
            <a-form-item :label="l('TagName')">
                <a-input
                    :placeholder="l('TagNameInputDesc')"
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

            <!-- 博客名称 -->
            <a-form-item :label="l('BlogName')">
                <a-select v-decorator="[
                        'blogId',
                        {
                            rules: [
                            ],
                        },
                        ]" style="width: 100%">
                    <a-select-option :value="item.id" v-for="item in bloglist" :key="item.id">
                        {{ item.name }}
                    </a-select-option>
                </a-select>
            </a-form-item>

            <!-- 标签描述 -->
            <a-form-item :label="l('TagDescription')">
                <a-textarea
                    :placeholder="l('TagDescriptionInputDesc')"
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
import { BlogServiceProxy, TagServiceProxy } from "@/shared/service-proxies";

export default {
    mixins: [ModalComponentBase],
    name: "create-or-edit-blogrolls",
    data() {
        return {
            // 获取到的数据
            entity: {},
            spinning: false,
            bloglist: [],
            _blogServiceProxy: "",
            _tagServiceProxy: ""
        };
    },
    components: {},
    beforeCreate() {
        this.form = this.$form.createForm(this, { name: "register" });
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._blogServiceProxy = new BlogServiceProxy(this.$apiUrl, this.$api);
        this._tagServiceProxy = new TagServiceProxy(this.$apiUrl, this.$api);
    },
    mounted() {
        console.log(this.id);
        this.getBolg();
        this.getData();
    },
    methods: {
        /**
         * 获取博客
         */
        getBolg() {
            this.spinning = true;
            this._blogServiceProxy
                .getBlogs()
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.bloglist = res;
                    console.log(res);
                });
        },
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._tagServiceProxy
                .getForEdit(this.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    console.log(res);
                    this.entity = res.tag;
                    this.form.setFieldsValue({
                        name: this.entity.name,
                        blogId: this.entity.blogId,
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
                    this._tagServiceProxy
                        .createOrUpdate({
                            tag: Object.assign(values, { id: this.id })
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