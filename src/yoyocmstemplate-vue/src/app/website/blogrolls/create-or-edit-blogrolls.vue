<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">
                <span v-if="id">{{ l('Edit') }}</span>
                <span v-if="!id">{{ l('Create') }}</span>
            </div>
        </div>
        <a-form :form="form" @submit="handleSubmit">
            <!-- 分类名称 -->
            <a-form-item :label="l('BlogrollTypeName')">
                <a-select :placeholder="l('BlogrollTypeNameInputDesc')" style="width: 100%" v-decorator="[
                        'blogrollTypeId',
                        {
                            rules: [
                                {
                                    required: true,
                                    message: l('ThisFieldIsRequired'),
                                }
                            ],
                        },
                        ]">
                    <a-select-option v-for="item in blogrollTypeList" :key="item.id" :value="item.id">
                        {{ item.name }}
                    </a-select-option>
                </a-select>
            </a-form-item>

            <!-- 链接名称 -->
            <a-form-item :label="l('BlogrollName')">
                <a-input
                    :placeholder="l('BlogrollNameInputDesc')"
                    v-decorator="[
                        'name',
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

            <!-- 友情链接地址 -->
            <a-form-item :label="l('BlogrollUrl')">
                <a-input
                    :placeholder="l('BlogrollUrlInputDesc')"
                    v-decorator="[
                        'url',
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

            <!-- 启用白名单 -->
            <a-form-item :label="l('Except')">
                <a-switch v-decorator="['except', {valuePropName: 'checked'} ]" />
            </a-form-item>

            <!-- 推荐 -->
            <a-form-item :label="l('Recommend')">
                <a-switch v-decorator="['recommend', {valuePropName: 'checked'} ]" />
            </a-form-item>

            <!-- 权重 -->
            <a-form-item :label="l('Weight')">
                <a-input-number v-decorator="[
                        'weight',
                        {
                            rules: [
                            ],
                        },
                        ]" :min="0" :max="1000000" :step="1" />
            </a-form-item>

            <!-- 友情链接logo -->
            <a-form-item :label="l('BlogrollLogo')">
                <a-input
                    :placeholder="l('BlogrollLogoInputDesc')"
                    v-decorator="[
                        'logo',
                        {
                            rules: [
                               
                            ],
                        },
                        ]" />
            </a-form-item>

            <!-- 图标地址 -->
            <a-form-item :label="l('IconName')">
                <a-popover :title="l('Preview')">
                    <template slot="content">
                        <img v-if="entity.iconName" :src="entity.iconName" alt="">
                        <span v-if="!entity.iconName">{{ l('NotSelected') }}</span>
                    </template>
                    <a-input
                        :placeholder="l('iconNameInputDesc')"
                        v-decorator="[
                        'iconName',
                        ]">
                        <p class="pleaseSelect-text" slot="addonAfter" @click="chooseFile()">{{ l('PleaseSelect') }}</p>
                    </a-input>
                </a-popover>
                <a-input
                    :placeholder="l('IconNameInputDesc')"
                    v-decorator="[
                        'iconName',
                        ]">
                    ； </a-input>
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
import FileManagerComponent from "../../admin/file-manager/file-manager";
import {
    BlogrollTypeServiceProxy,
    BlogrollServiceProxy
} from "@/shared/service-proxies";

export default {
    mixins: [ModalComponentBase],
    name: "create-or-edit-blogrolls",
    data() {
        return {
            // 获取到的数据
            entity: {},
            spinning: false,
            _blogrollTypeServiceProxy: "",
            _blogrollServiceProxy: "",
            blogrollTypeList: []
        };
    },
    components: {
        FileManagerComponent
    },
    beforeCreate() {
        this.form = this.$form.createForm(this, { name: "register" });
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._blogrollTypeServiceProxy = new BlogrollTypeServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this._blogrollServiceProxy = new BlogrollServiceProxy(
            this.$apiUrl,
            this.$api
        );
    },
    mounted() {
        this.form.setFieldsValue({
            weight: 0,
            price: 0,
            viewCount: 0
        });
        console.log(this.id);
        this.getBlogrollTypeList();
    },
    methods: {
        /**
         * 获取数据
         */
        getBlogrollTypeList() {
            this.spinning = true;
            this._blogrollTypeServiceProxy
                .getPaged("", "", 100, 0)
                .then(res => {
                    this.blogrollTypeList = res.items;
                    this.getData();
                });
        },
        getData() {
            this._blogrollServiceProxy
                .getForEdit(this.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    console.log(res);
                    this.entity = res.blogroll;
                    this.form.setFieldsValue({
                        blogrollTypeId: this.entity.blogrollTypeId,
                        name: this.entity.name,
                        url: this.entity.url,
                        except: this.entity.except,
                        recommend: this.entity.recommend,
                        weight: this.entity.weight,
                        logo: this.entity.logo,
                        iconName: this.entity.iconName
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
                    this._blogrollServiceProxy
                        .createOrUpdate({
                            blogroll: Object.assign(values, { id: this.id })
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
        },
        /**
         * 选择文件
         */
        chooseFile(type) {
            ModalHelper.create(
                FileManagerComponent,
                {
                    componentType: "modal"
                },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                if (res) {
                    this.form.setFieldsValue({
                        iconName: res.path
                    });
                    this.entity.iconName = res.path;
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