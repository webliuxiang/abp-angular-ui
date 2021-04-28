<template>
    <a-spin :spinning="spinning">
        <!-- 标题 -->
        <div class="modal-header">
            <div class="modal-title">
                <a-icon type="user" />
                <span v-if="user.id">{{ l('Edit') }}:{{ user.userName }}</span>
                <span v-if="!user.id">{{ l('CreateNewUser') }}</span>
            </div>
        </div>
        <!-- tab切换 -->
        <a-tabs defaultActiveKey="1">
            <!-- 用户信息 -->
            <a-tab-pane key="1">
                <span slot="tab">
                    <a-icon type="user" />
                    {{ l('UserInformations') }}
                </span>
                <a-form :form="form" :label-col="{ span: 5 }" :wrapper-col="{ span: 12 }" @submit="handleSubmit">
                    <!-- 用户名 -->
                    <a-form-item :label="l('UserName')">
                        <a-input :placeholder="l('UserName')"
                            v-decorator="['userName', { rules: [
                                { required: true, message: l('ThisFieldIsRequired') },
                                { message: l('MaxLength'),max:32  }
                             ] }]" />
                    </a-form-item>
                    <!-- 头像 -->
                    <a-form-item :label="l('UploadProfilePicture')">
                        <a-upload
                            name="profilePictureFile"
                            :action="uploadPictureUrl"
                            listType="picture-card"
                            :fileList="fileList"
                            :beforeUpload="beforeUpload"
                            accept="image/*"
                            :headers="uploadHeaders"
                            @preview="handlePreview"
                            @change="uploadPictureChange($event)">
                            <div v-if="fileList.length < 1">
                                <a-icon type="plus" />
                                <div class="ant-upload-text">{{ l('UploadProfilePicture') }}</div>
                            </div>
                        </a-upload>
                        <a-modal :visible="previewVisible" :footer="null" @cancel="handleCancel">
                            <img alt="example" style="width: 100%" :src="previewImage" />
                        </a-modal>
                    </a-form-item>
                    <!-- 邮箱 -->
                    <a-form-item :label="l('EmailAddress')">
                        <a-input :placeholder="l('EmailAddress')"
                            v-decorator="[
                                'emailAddress',
                                {
                                    rules: [
                                    {
                                        type: 'email',
                                        message: l('InvalidEmailAddress') ,
                                    },
                                    {
                                        required: true,
                                        message: l('ThisFieldIsRequired'),
                                    },
                                    ],
                                },
                                ]" />
                    </a-form-item>
                    <!-- 电话号码 -->
                    <a-form-item :label="l('PhoneNumber')">
                        <a-input :placeholder="l('PhoneNumber')"
                            v-decorator="[
                                'phoneNumber',
                                {
                                    rules: [
                                    { message: l('MaxLength'),max:24  }
                                    ],
                                },
                                ]" />
                    </a-form-item>
                    <!-- 使用随机密码 -->
                    <a-form-item>
                        <span slot="label">
                            {{ l('SetRandomPassword') }}
                        </span>
                        <a-switch v-decorator="['setRandomPassword', {valuePropName: 'checked'} ]" :checkedChildren="l('Yes')" :unCheckedChildren="l('No')" @change="isSetRandomPassword = !isSetRandomPassword" />
                    </a-form-item>
                    <!-- 密码 -->
                    <a-form-item :label="l('Password')" v-if="!isSetRandomPassword">
                        <a-input :placeholder="l('Password')"
                            type="password"
                            v-decorator="['password', { rules: [
                                { required: true, message: l('ThisFieldIsRequired') },
                                { message: l('MinLength'),min:6  },
                                { message: l('MaxLength'),max:32  }
                             ] }]" />
                    </a-form-item>
                    <!-- 密码核对 -->
                    <a-form-item :label="l('PasswordRepeat')" v-if="!isSetRandomPassword">
                        <a-input :placeholder="l('PasswordRepeat')"
                            type="password"
                            v-decorator="['PasswordRepeat', {
                                rules: [
                                    { 
                                        required: true, message: l('ThisFieldIsRequired') 
                                    },
                                    {
                                        validator: compareToFirstPassword,
                                    },
                                ]
                             }]" />
                    </a-form-item>
                    <a-row :gutter="8">
                        <!-- 下次登录需要修改密码 -->
                        <a-col :sm="11" class="halfwidth next-password" :offset="1">
                            <a-form-item>
                                <span slot="label">
                                    {{ l('ShouldChangePasswordOnNextLogin') }}
                                </span>
                                <a-switch v-decorator="['needToChangeThePassword', {valuePropName: 'checked'} ]" :checkedChildren="l('Yes')" :unCheckedChildren="l('No')" />
                            </a-form-item>
                        </a-col>
                        <!-- 发送激活右键 -->
                        <a-col :sm="10" class="halfwidth" :offset="2">
                            <a-form-item>
                                <span slot="label">
                                    {{ l('SendActivationEmail') }}
                                </span>
                                <a-switch v-decorator="['sendActivationEmail', {valuePropName: 'checked'} ]" :checkedChildren="l('Yes')" :unCheckedChildren="l('No')" />
                            </a-form-item>
                        </a-col>
                    </a-row>
                    <a-row :gutter="8">
                        <!-- 激活 -->
                        <a-col :sm="10" class="halfwidth" :offset="2">
                            <a-form-item>
                                <span slot="label">
                                    {{ l('Active') }}
                                </span>
                                <a-switch v-decorator="['isActive', {valuePropName: 'checked'} ]" :checkedChildren="l('Yes')" :unCheckedChildren="l('No')" />
                            </a-form-item>
                        </a-col>
                        <!-- 启用锁定 -->
                        <a-col :sm="10" class="halfwidth" :offset="2">
                            <a-form-item>
                                <span slot="label">
                                    {{ l('IsLockoutEnabled') }}
                                </span>
                                <a-switch v-decorator="['isLockoutEnabled', {valuePropName: 'checked'} ]" :checkedChildren="l('Yes')" :unCheckedChildren="l('No')" />
                            </a-form-item>
                        </a-col>
                    </a-row>
                </a-form>
            </a-tab-pane>
            <!-- 角色 -->
            <a-tab-pane key="2">
                <a-badge slot="tab" :count="checkedRoles.length">
                    <a-icon type="medicine-box" />
                    {{ l('Roles') }}
                </a-badge>
                <a-checkbox-group :options="roleList" v-model="checkedRoles" />
            </a-tab-pane>
            <!-- 组织单元 -->
            <a-tab-pane key="3">
                <span slot="tab">
                    <a-icon type="share-alt" />
                    {{ l('OrganizationUnits') }}
                </span>
                <organization-unit-tree
                    :multiple="true"
                    :dropDownStyle="{ 'max-height': '500px' }"
                    :data="organizationList"
                    @selectedUnitChange="refreshGoFirstPage">
                </organization-unit-tree>
            </a-tab-pane>
        </a-tabs>
        <!-- 按钮 -->
        <div class="modal-footer">
            <a-button :disabled="saving" @click="close()" type="button">
                <a-icon type="close-circle" />
                {{ l('Cancel') }}
            </a-button>
            <a-button :loading="saving" :type="'primary'" @click="handleSubmit()">
                <a-icon type="save" />
                {{ l('Save') }}
            </a-button>
        </div>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { ModalComponentBase } from "@/shared/component-base";
import { AppConsts } from "@/abpPro/AppConsts";
import {
    ProfileServiceProxy,
    UserServiceProxy
} from "@/shared/service-proxies";
import OrganizationUnitTree from "../../shared/organization-unit-tree/organization-unit-tree";

function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

export default {
    name: "create-or-edit-user",
    mixins: [AppComponentBase, ModalComponentBase],
    components: {
        OrganizationUnitTree
    },
    data() {
        return {
            spinning: false,
            id: "",
            // 表单
            formLayout: "horizontal",
            form: this.$form.createForm(this, { name: "coordinated" }),
            // 用户
            user: {},
            previewVisible: false,
            previewImage: "",
            // 上传图片
            fileList: [],
            uploadPictureUrl:
                AppConsts.remoteServiceBaseUrl +
                "/Profile/UploadProfilePictureReturnFileId",
            maxProfilPictureBytesValue: AppConsts.maxProfilPictureMb,
            uploadHeaders: {},
            _profileServiceProxy: "",
            _userServiceProxy: "",
            // 是否设置随机密码
            isSetRandomPassword: true,
            // 角色list
            roleList: [],
            checkedRoles: [],
            // 选择组织单源数据
            organizationList: {},
            selectedUnit: []
        };
    },
    created() {
        this.fullData(); // 模态框必须,填充数据到data字段
        Object.assign(this.uploadHeaders, {
            Authorization: "Bearer " + abp.auth.getToken()
        });
        this._profileServiceProxy = new ProfileServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this._userServiceProxy = new UserServiceProxy(this.$apiUrl, this.$api);
        this.init();
    },
    methods: {
        /**
         * 初始化
         */
        init() {
            this.spinning = true;
            this._userServiceProxy
                .getForEditTree(this.id)
                .finally(() => (this.spinning = false))
                .then(result => {
                    console.log(result);
                    this.roleList = result.roles.map(
                        item => item.roleDisplayName
                    );
                    this.organizationList = Object.assign(result, {
                        selectedOrganizationUnits: this.selectedUnit
                    });
                    // 设置表单
                    this.$nextTick(() => {
                        this.form.setFieldsValue({
                            userName: result.user.userName,
                            emailAddress: result.user.emailAddress,
                            phoneNumber: result.user.phoneNumber,
                            password: result.user.password ? true : false,
                            setRandomPassword: result.user
                                .needToChangeThePassword
                                ? true
                                : false,
                            sendActivationEmail: result.user.sendActivationEmail
                                ? true
                                : false,
                            isActive: result.user.isActive ? true : false,
                            isLockoutEnabled: result.user.isLockoutEnabled
                                ? true
                                : false
                        });
                    });
                    // 设置头像
                    if (result.user.profilePictureId) {
                        this.getProfilePicture(result.user.profilePictureId);
                    }
                });
        },
        /**
         * 验证密码输入
         */
        compareToFirstPassword(rule, value, callback) {
            const form = this.form;
            if (value && value !== form.getFieldValue("password")) {
                callback(this.l("PasswordsDontMatch"));
            } else {
                callback();
            }
        },
        /**
         * 提交表单
         */
        handleSubmit() {
            this.form.validateFields((err, values) => {
                if (!err) {
                    console.log(this.fileList);
                    let parmas = {
                        assignedRoleNames: [],
                        organizationUnits: this.selectedUnit,
                        user: {}
                    };
                    // 处理头像
                    Object.assign(values, {
                        profilePictureId: this.id
                            ? this.fileList.length
                                ? this.fileList[0].name
                                : ""
                            : this.fileList.length
                            ? this.fileList[0].response.result.profilePictureId
                            : "",
                        needToChangeThePassword: values.needToChangeThePassword
                            ? values.needToChangeThePassword
                            : false,
                        id: this.id
                    });
                    parmas.user = values;
                    // 处理角色
                    parmas.assignedRoleNames = this.organizationList.roles
                        .filter(obj =>
                            this.checkedRoles.some(
                                item => item === obj.roleDisplayName
                            )
                        )
                        .map(item => item.roleName);
                    console.log(parmas);
                    this.spinning = true;
                    this._userServiceProxy
                        .createOrUpdate(parmas)
                        .finally(() => {
                            this.spinning = false;
                        })
                        .then(() => {
                            this.notify.success(this.l("SavedSuccessfully"));
                            this.success(true);
                        });
                }
            });
        },
        /**
         * 上传图片
         */
        beforeUpload(file) {
            const isJPG =
                file.type === "image/jpeg" ||
                file.type === "image/png" ||
                file.type === "image/gif";
            if (!isJPG) {
                abp.message.error(this.l("OnlySupportPictureFile"));
            }
            const isLtXM =
                file.size / 1024 / 1024 < this.maxProfilPictureBytesValue;
            console.log(isLtXM);
            console.log(this.maxProfilPictureBytesValue);
            if (!isLtXM) {
                abp.message.error(
                    this.l(
                        "ProfilePicture_Warn_SizeLimit",
                        this.maxProfilPictureBytesValue
                    )
                );
            }
            const isValid = isJPG && isLtXM;
            return isValid;
        },
        handleCancel() {
            this.previewVisible = false;
        },
        async handlePreview(file) {
            console.log(file);
            if (!file.url && !file.preview) {
                file.preview = await getBase64(file.originFileObj);
            }
            this.previewImage = file.url || file.preview;
            this.previewVisible = true;
        },
        uploadPictureChange({ fileList }) {
            console.log(fileList);
            this.fileList = fileList;
        },
        /**
         * 通过头像Id获取头像
         * @param profilePictureId 头像Id
         */
        getProfilePicture(profilePictureId) {
            if (profilePictureId) {
                this.spinning = true;
                this._profileServiceProxy
                    .getProfilePictureById(profilePictureId)
                    .finally(() => (this.spinning = false))
                    .then(result => {
                        if (result && result.profilePicture) {
                            let profilePreviewImage =
                                "data:image/jpeg;base64," +
                                result.profilePicture;
                            // 把图像加到头像列表 显示
                            this.fileList = [
                                {
                                    uid: -1,
                                    name: profilePictureId,
                                    status: "done",
                                    url: profilePreviewImage
                                }
                            ];
                        }
                    });
            }
        },
        /**
         * 选择完权限过滤
         */
        refreshGoFirstPage(data) {
            this.selectedUnit = data;
            console.log(this.selectedUnit);
        }
    }
};
</script>

<style scoped lang="less">
.anticon-user {
    margin-right: 10px;
}
.ant-upload-select-picture-card i {
    font-size: 32px;
    color: #999;
}

.ant-upload-select-picture-card .ant-upload-text {
    margin-top: 8px;
    color: #666;
}
.halfwidth {
    /deep/ .ant-form-item-label {
        width: 30%;
    }
}
.next-password {
    /deep/ .ant-form-item-label {
        width: 45%;
    }
}
</style>
