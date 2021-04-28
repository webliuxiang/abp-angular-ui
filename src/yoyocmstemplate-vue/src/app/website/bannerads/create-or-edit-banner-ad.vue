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
            <a-form-item :label="l('Title')">
                <a-input
                    :placeholder="l('Title')"
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

            <!-- 图片地址 -->
            <a-form-item :label="l('ImageUrl')">
                <a-popover :title="l('Preview')">
                    <template slot="content">
                        <img v-if="entity.imageUrl" :src="entity.imageUrl" alt="">
                        <span v-if="!entity.imageUrl">{{ l('NotSelected') }}</span>
                    </template>
                    <a-input
                        :placeholder="l('ImageUrlInputDesc')"
                        v-decorator="[
                        'imageUrl',
                        ]">
                        <p class="pleaseSelect-text" slot="addonAfter" @click="chooseFile('imageUrl')">{{ l('PleaseSelect') }}</p>
                    </a-input>
                </a-popover>
            </a-form-item>

            <!-- 缩略图地址 -->
            <a-form-item :label="l('ThumbImgUrl')">
                <a-popover :title="l('Preview')">
                    <template slot="content">
                        <img v-if="entity.thumbImgUrl" :src="entity.thumbImgUrl" alt="">
                        <span v-if="!entity.thumbImgUrl">{{ l('NotSelected') }}</span>
                    </template>
                    <a-input
                        :placeholder="l('ThumbImgUrlInputDesc')"
                        v-decorator="[
                        'thumbImgUrl',
                        ]">
                        <p class="pleaseSelect-text" slot="addonAfter" @click="chooseFile('thumbImgUrl')">{{ l('PleaseSelect') }}</p>
                    </a-input>
                </a-popover>
            </a-form-item>

            <!-- 描述 -->
            <a-form-item :label="l('Description')">
                <a-textarea
                    :placeholder="l('DescriptionInputDesc')"
                    :auto-size="{ minRows: 3, maxRows: 5 }"
                    maxLength="3000"
                    v-decorator="[
                        'description',
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

            <!-- 目标url -->
            <a-form-item :label="l('BannerAdUrl')">
                <a-input
                    :placeholder="l('BannerAdUrlInputDesc')"
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

            <!-- 价格 -->
            <a-form-item :label="l('Price')">
                <a-input
                    :placeholder="l('BannerAdPriceInputDesc')"
                    v-decorator="[
                        'price',
                        {
                            rules: [
                            ],
                        },
                        ]" />
            </a-form-item>

            <!-- 轮播图类型 -->
            <a-form-item :label="l('BannerAdTypes')">
                <a-input
                    :placeholder="l('BannerAdTypesInputDesc')"
                    v-decorator="[
                        'types',
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
import FileManagerComponent from "../../admin/file-manager/file-manager";
import { BannerImgServiceProxy } from "@/shared/service-proxies";
import pick from "lodash.pick";

export default {
    mixins: [ModalComponentBase],
    name: "create-tenant-component",
    data() {
        return {
            // 获取到的数据
            entity: {},
            spinning: false,
            _bannerImgServiceProxy: ""
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
        this._bannerImgServiceProxy = new BannerImgServiceProxy(
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
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._bannerImgServiceProxy
                .getForEdit(this.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    console.log(res);
                    this.entity = res.bannerAd;
                    this.form.setFieldsValue({
                        title: res.bannerAd.title,
                        imageUrl: res.bannerAd.imageUrl,
                        thumbImgUrl: res.bannerAd.thumbImgUrl,
                        description: res.bannerAd.description,
                        url: res.bannerAd.url,
                        weight: res.bannerAd.weight,
                        price: res.bannerAd.price,
                        types: res.bannerAd.types,
                        viewCount: res.bannerAd.viewCount
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
                    this._bannerImgServiceProxy
                        .createOrUpdate({
                            bannerAd: Object.assign(values, { id: this.id })
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
                    if (type === "imageUrl") {
                        this.form.setFieldsValue({
                            imageUrl: res.path
                        });
                        this.entity.imageUrl = res.path;
                    } else if (type === "thumbImgUrl") {
                        this.form.setFieldsValue({
                            thumbImgUrl: res.path
                        });
                        this.entity.thumbImgUrl = res.path;
                    }
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