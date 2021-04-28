<template>
    <a-spin :spinning="spinning">
        <div class="modal-header">
            <div class="modal-title">
                <span v-if="itemid">{{ l('Edit') }}</span>
                <span v-if="!itemid">{{ l('Create') }}</span>
            </div>
        </div>
        <a-form :form="form" @submit="handleSubmit">
            <!-- 标题 -->
            <a-form-item :label="l('PostTitle')">
                <a-input
                    :placeholder="l('PostTitleInputDesc')"
                    v-decorator="[
                        'title',
                        {
                            rules: [
                                {
                                    required: true,
                                    message: l('ThisFieldIsRequired'),
                                },
                                {
                                    min: 3,
                                    message: l('MinLength', 3),
                                }
                            ],
                        },
                        ]" />
            </a-form-item>

            <!-- 文章链接 -->
            <a-form-item :label="l('PostUrl')">
                <a-input
                    :placeholder="l('PostUrlInputDesc')"
                    v-decorator="[
                        'url',
                        {
                            rules: [
                                {
                                    required: true,
                                    message: l('ThisFieldIsRequired'),
                                },
                                {
                                    min: 3,
                                    message: l('MinLength', 3),
                                }
                            ],
                        },
                        ]" />
            </a-form-item>

            <!-- 封面 -->
            <a-form-item :label="l('PostCoverImage')">
                <img :src="coverImage" v-if="coverImage" width="64" alt="">
                <br v-if="coverImage">
                <a-button type="primary" @click="chooseFile">
                    选择
                </a-button>
                <a-button v-if="coverImage" @click="coverImage = ''">删除</a-button>
            </a-form-item>

            <!-- 内容 -->
            <a-form-item :label="l('PostContent')">
                <quill-editor v-model="content" ref="myTextEditor" :options="editorOption" style="height:600px;"></quill-editor>
            </a-form-item>

            <!-- 文章类型 -->
            <a-form-item :label="l('PostType')">
                <a-radio-group button-style="solid" v-decorator="[
                        'postType',
                        {
                            rules: [
                            ],
                        },
                        ]">
                    <a-radio-button v-for="item in postTypeTypeEnum" :key="item.value" :value="item.value">
                        {{ item.key }}
                    </a-radio-button>
                </a-radio-group>
            </a-form-item>

            <!-- 标签 -->
            <a-form-item :label="l('Tags')">
                <a-select mode="tags" style="width: 100%" placeholder="请选择标签" v-decorator="[
                        'tagIds',
                        {
                            rules: [
                            ],
                        },
                        ]">
                    <a-select-option v-for="item in tagsOfBlogLIST" :key="item.id">
                        {{ item.name }}
                    </a-select-option>
                </a-select>
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
import { PostServiceProxy, BlogServiceProxy } from "@/shared/service-proxies";
import FileManagerComponent from "../../admin/file-manager/file-manager";
import "quill/dist/quill.core.css";
import "quill/dist/quill.snow.css";
import "quill/dist/quill.bubble.css";
import { quillEditor } from "vue-quill-editor";

export default {
    mixins: [ModalComponentBase],
    name: "create-or-edit-blogrolls",
    data() {
        return {
            // 获取到的数据
            entity: {},
            spinning: false,
            coverImage: "",
            editorOption: {
                placeholder: "编辑文章内容"
            },
            content: "",
            _postServiceProxy: "",
            _blogServiceProxy: "",
            // 文章类型
            postTypeTypeEnum: [],
            // 标签
            tagsOfBlogLIST: [],
            default: ""
        };
    },
    components: {
        FileManagerComponent,
        quillEditor
    },
    beforeCreate() {
        this.form = this.$form.createForm(this, { name: "register" });
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        this._postServiceProxy = new PostServiceProxy(this.$apiUrl, this.$api);
        this._blogServiceProxy = new BlogServiceProxy(this.$apiUrl, this.$api);
    },
    mounted() {
        let p1 = this.getPostTypeTypeEnum();
        let p2 = this.getTagsOfBlog();
    },
    methods: {
        /**
         * 获取文章类型
         */
        getPostTypeTypeEnum() {
            this.spinning = true;
            this._postServiceProxy
                .getForEdit(this.itemid)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    console.log(res);
                    let post = res.post;
                    this.postTypeTypeEnum = res.postTypeTypeEnum;
                    this.$nextTick(() => {
                        this.coverImage = post.coverImage;
                        this.content = post.content;
                        this.form.setFieldsValue({
                            title: post.title,
                            url: post.url,
                            postType: post.postType,
                            tagIds: post.tagIds
                        });
                    });
                });
        },
        /**
         * 获取标签
         */
        getTagsOfBlog() {
            this.spinning = true;
            this._blogServiceProxy
                .getTagsOfBlog(this.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.tagsOfBlogLIST = res;
                });
        },
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
                    if (!this.content) {
                        this.$notification["warning"]({
                            message: "请输入内容"
                        });
                        return;
                    }
                    let parmas = Object.assign(values, {
                        blogId: this.id,
                        id: this.itemid,
                        content: this.content,
                        coverImage: this.coverImage
                    });
                    this.spinning = true;
                    this._postServiceProxy
                        .createOrUpdate({
                            post: parmas
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
                    this.coverImage = res.path;
                }
            });
        },
        /**
         * 富文本
         */
        onEditorChange({ editor, html, text }) {
            this.content = html;
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
/deep/.ql-container {
    height: 85%;
}
</style>