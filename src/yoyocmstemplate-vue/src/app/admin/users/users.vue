<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('Users')"></page-header>
        <a-card :bordered="false">
            <a-form :layout="'vertical'" @submit.prevent="getData">
                <a-row :gutter="8">
                    <!-- 搜索 -->
                    <a-col :sm="24">
                        <a-form-item>
                            <a-input-search
                                name="filterText"
                                :placeholder="l('SearchWithThreeDot')"
                                @search="getData"
                                enterButton
                                v-model="filterText"
                                v-decorator="['filterText']" />
                        </a-form-item>
                    </a-col>
                    <!-- 权限过滤 -->
                    <a-col :sm="12" v-if="advancedFiltersVisible">
                        <a-form-item>
                            <permission-combox
                                :multiple="true"
                                :dropDownStyle="{ 'max-height': '500px' }"
                                :selectedPermission="selectedPermission"
                                @selectedPermissionChange="refreshGoFirstPage">
                            </permission-combox>
                        </a-form-item>
                    </a-col>
                    <!-- 角色过滤 -->
                    <a-col :sm="12" v-if="advancedFiltersVisible">
                        <a-form-item>
                            <role-combox :selectedRole="role" @selectedRoleChange="selectedRoleChange"></role-combox>
                        </a-form-item>
                    </a-col>
                </a-row>
            </a-form>
            <!-- 操作部分 -->
            <a-row :gutter="8">
                <a-col :md="20" :sm="12">
                    <!-- 添加用户 -->
                    <a-button
                        :type="'primary'"
                        v-if="isGranted('Pages.Administration.Users.Create')"
                        @click="createOrEdit()">
                        <a-icon type="plus" />
                        <span>{{l("CreateNewUser")}}</span>
                    </a-button>
                    <!-- 批量删除 -->
                    <a-button :type="'danger'" v-if="isGranted('Pages.Administration.Users.Delete')" @click="batchDelete">
                        <a-icon type="delete" />
                        <span>{{l("BatchDelete")}}</span>
                    </a-button>
                    <!-- 导出excel -->
                    <a-button v-if="isGranted('Pages.Administration.Users.ExportToExcel')" @click="exportToExcel">
                        <a-icon type="file-excel" />
                        <span>{{l("ExportToExcel")}}</span>
                    </a-button>
                    <!-- 下载excel -->
                    <a-button v-if="isGranted('Pages.Administration.Users.Impersonation')" @click="ImportUsersSampleFile">
                        <a-icon type="file-excel" />
                        <span>{{l("ImportToExcelTemplate")}}</span>
                    </a-button>
                    <!-- 导入excel -->
                    <!-- <a-button v-if="isGranted('Pages.Administration.Users.Impersonation')">
                        <a-icon type="file-excel" />
                        <span>{{l("ImportFromExcel")}}</span>
                    </a-button> -->
                    <a-upload
                        :action="importFromExcelUrl"
                        :multiple="false"
                        :fileList="fileList"
                        accept="xls/*"
                        :headers="uploadHeaders"
                        @change="uploadChange">
                        <a-button style="margin-left:10px" v-if="isGranted('Pages.Administration.Users.Impersonation')">
                            <a-icon type="file-excel" />
                            <span>{{l("ImportFromExcel")}}</span>
                        </a-button>
                    </a-upload>
                </a-col>
                <!-- 显示暴击过滤 -->
                <a-col :md="4" :sm="12" class="text-right">
                    <a @click="advancedFiltersVisible=!advancedFiltersVisible">
                        {{advancedFiltersVisible ? l('HideAdvancedFilters') : l('ShowAdvancedFilters')}}
                        <a-icon :type="advancedFiltersVisible ? 'up' : 'down'" />
                    </a>
                </a-col>
            </a-row>

            <!-- 数据部分 -->
            <div class="my-md">
                <a-alert :type="'info'" :showIcon="true">
                    <template slot="message">
                        <span v-html="l('GridSelectedXItemsTips',selectedRowKeys.length)"></span>
                        <a @click="restCheckStatus()" class="ml-md">{{l('ClearEmpty')}}</a>
                        <a-divider type="vertical"></a-divider>
                        <a @click="getData()">{{l('Refresh')}}</a>
                    </template>
                </a-alert>
            </div>

            <a-row>
                <a-table
                    class="list-table"
                    @change="getData"
                    :pagination="false"
                    :rowSelection="{selectedRowKeys: selectedRowKeys, onChange: onSelectChange, getCheckboxProps:getCheckboxProps}"
                    :columns="columns"
                    :rowKey="tableDatas => tableDatas.id"
                    :dataSource="tableData">
                    <!-- 角色 -->
                    <p class="roles" slot="roles" slot-scope="text, record">
                        <a-tag v-for="item in record.roles" :key="item.roleId">{{ item.roleName }}</a-tag>
                    </p>
                    <!-- 是否邮箱验证 -->
                    <a class="isEmailConfirmed" slot="isEmailConfirmed" slot-scope="record">
                        <a-icon v-if="record" type="check" />
                        <a-icon v-if="!record" type="close" />
                    </a>
                    <!-- 是否激活-->
                    <a class="isActive" slot="isActive" slot-scope="record">
                        <a-icon v-if="record" type="check" />
                        <a-icon v-if="!record" type="close" />
                    </a>
                    <!-- 上次登录时间 -->
                    <a class="lastLoginTime" slot="lastLoginTime" slot-scope="record">
                        <p v-if="record">{{ record }}</p>
                        <p v-if="!record">---</p>
                    </a>
                    <span slot="actions" slot-scope="text, record">
                        <!-- 修改 -->
                        <a class="table-edit" @click="createOrEdit(record.id)" v-if="isGranted('Pages.Administration.Users.Edit')">
                            <a-icon type="edit" />{{ l('Edit') }}</a>
                        <!-- 删除 -->
                        <a-popconfirm placement="top" v-if="!isAdmin(record) && isGranted('Pages.Administration.Users.Delete')" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="deleteItem(record)">
                            <template slot="title">
                                {{ l('ConfirmDeleteWarningMessage') }}
                            </template>
                            <a class="table-delete">
                                <a-icon type="delete" />{{ l('Delete') }}</a>
                        </a-popconfirm>
                        <!-- 更多 -->
                        <a-dropdown v-if="!isGrantedAny(
                                'Pages.Administration.Users.Impersonation',
                                'Pages.Administration.Users.ChangePermissions',
                                'Pages.Administration.Users.Unlock'
                                )">
                            <a class="ant-dropdown-link" @click="e => e.preventDefault()">
                                {{ l('Actions') }}
                                <a-icon type="down" />
                            </a>
                            <a-menu slot="overlay">
                                <a-menu-item
                                    v-if="isGranted('Pages.Administration.Users.Impersonation') && record.id !== _appSessionService.userId"
                                    @click="tenantImpersonateLogin(record)">
                                    <a href="javascript:;">
                                        <a-icon type="login" />
                                        <span>{{ l('LoginAsThisUser') }}</span>
                                    </a>
                                </a-menu-item>
                                <a-menu-item
                                    v-if="isGranted('Pages.Administration.Users.Edit')"
                                    @click="editUserPermissions(record)">
                                    <a href="javascript:;">
                                        <a-icon type="codepen" />
                                        <span>{{ l('Permissions') }}</span>
                                    </a>
                                </a-menu-item>
                                <a-menu-item v-if="enabledUnlock" @click="unlockTenantAdminUser(record)">
                                    <a href="javascript:;">
                                        <a-icon type="unlock" />
                                        <span>{{ l('Unlock') }}</span>
                                    </a>
                                </a-menu-item>
                            </a-menu>
                        </a-dropdown>
                    </span>
                </a-table>
                <a-pagination
                    class="pagination"
                    size="middle"
                    :total="totalItems"
                    showSizeChanger
                    showQuickJumper
                    :showTotal="showTotalFun"
                    @change="onChange"
                    @showSizeChange="showSizeChange" />
            </a-row>
        </a-card>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import PermissionCombox from "../shared/permission-combox/permission-combox.vue";
import { ModalHelper } from "@/shared/helpers";
import { UserServiceProxy } from "@/shared/service-proxies";
import RoleCombox from "../shared/role-combox/role-combox";
import CreateOrEditUserCompent from "./create-or-edit-user/create-or-edit-user";
import EditUserPermissionsCompent from "./edit-user-permissions/edit-user-permissions";
import moment from "moment";
import { AppConsts } from "@/abpPro/AppConsts";
import { appSessionService } from "@/shared/abp";
import { fileDownloadService } from "@/shared/utils";
import { impersonationService } from "@/shared/auth/index";

export default {
    mixins: [AppComponentBase],
    name: "tenants",
    components: {
        PermissionCombox,
        CreateOrEditUserCompent,
        RoleCombox,
        EditUserPermissionsCompent
    },
    data() {
        return {
            // 是否显示高级过滤
            advancedFiltersVisible: false,
            _userServiceProxy: null,
            // 选中的权限过滤
            selectedPermission: [],
            // 表格
            columns: [
                {
                    title: this.l("UserName"),
                    dataIndex: "userName",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "userName" }
                },
                {
                    title: this.l("Roles"),
                    dataIndex: "roles",
                    align: "center",
                    scopedSlots: { customRender: "roles" }
                },
                {
                    title: this.l("EmailAddress"),
                    dataIndex: "emailAddress",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "emailAddress" }
                },
                // 邮箱验证
                {
                    title: this.l("EmailConfirm"),
                    dataIndex: "isEmailConfirmed",
                    sorter: true,
                    align: "center",
                    filters: [
                        { text: this.l("All"), value: "" },
                        { text: this.l("Yes"), value: "true" },
                        { text: this.l("No"), value: "false" }
                    ],
                    filterMultiple: false,
                    onFilter: (value, record) =>
                        String(record.isEmailConfirmed).indexOf(value) === 0,
                    scopedSlots: { customRender: "isEmailConfirmed" }
                },
                // 是否激活
                {
                    title: this.l("Active"),
                    dataIndex: "isActive",
                    sorter: true,
                    align: "center",
                    filters: [
                        { text: this.l("All"), value: "" },
                        { text: this.l("Yes"), value: "true" },
                        { text: this.l("No"), value: "false" }
                    ],
                    filterMultiple: false,
                    onFilter: (value, record) =>
                        String(record.isActive).indexOf(value) === 0,
                    scopedSlots: { customRender: "isActive" }
                },
                {
                    title: this.l("LastLoginTime"),
                    dataIndex: "lastLoginTime",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "lastLoginTime" }
                },
                {
                    title: this.l("CreationTime"),
                    dataIndex: "creationTimeStr",
                    sorter: true,
                    align: "center",
                    scopedSlots: { customRender: "creationTimeStr" }
                },
                {
                    title: this.l("Actions"),
                    dataIndex: "actions",
                    align: "center",
                    scopedSlots: { customRender: "actions" }
                }
            ],
            // 总数
            totalItems: 0,
            // 当前页码
            pageNumber: 1,
            // 共多少页
            totalPages: 1,
            // 条数显示范围
            pagerange: [1, 1],
            // 显示条数
            pageSizeOptions: ["10", "20", "30", "40"],
            request: { sorting: "", maxResultCount: 10, skipCount: 0 },
            // 选择多少项
            selectedRowKeys: [],
            selectedRows: [],
            // 请求参数
            filterText: undefined,
            tableData: [],
            // loading
            spinning: false,
            // 选中的角色Ids过滤
            role: [],
            /**
             * 是否已验证邮箱过滤
             */
            isEmailConfirmed: undefined,
            /**
             * 是否激活过滤
             */
            isActive: undefined,
            _appSessionService: "",
            _fileDownloadService: "",
            // 上传excel
            fileList: [],
            importFromExcelUrl:
                AppConsts.remoteServiceBaseUrl + "/UserImport/ImportFromExcel",
            uploadHeaders: {}
        };
    },
    created() {
        this._userServiceProxy = new UserServiceProxy(this.$apiUrl, this.$api);
        this._fileDownloadService = fileDownloadService;
        this.getData();
        this._appSessionService = appSessionService;
        console.log(this._appSessionService);
        Object.assign(this.uploadHeaders, {
            Authorization: "Bearer " + abp.auth.getToken()
        });
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._userServiceProxy
                .getPaged(
                    this.selectedPermission,
                    this.role,
                    this.isEmailConfirmed,
                    this.isActive,
                    undefined,
                    this.filterText,
                    this.request.sorting,
                    this.request.maxResultCount,
                    this.request.skipCount
                )
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.tableData = res.items;
                    this.tableData.map(item => {
                        item.creationTimeStr = item.creationTime
                            ? moment(item.creationTime).format(
                                  "YYYY-MM-DD HH:mm:ss"
                              )
                            : "-";
                    });
                    this.totalItems = res.totalCount;
                    this.totalPages = Math.ceil(
                        res.totalCount / this.request.maxResultCount
                    );
                    this.pagerange = [
                        (this.pageNumber - 1) * this.request.maxResultCount + 1,
                        this.pageNumber * this.request.maxResultCount
                    ];
                });
        },
        /**
         * table默认配置 （是否禁用）
         */
        getCheckboxProps: record => ({
            props: {
                disabled: record.id === appSessionService.userId
            }
        }),
        /**
         * 添加用户  修改用户
         */
        createOrEdit(id) {
            ModalHelper.create(
                CreateOrEditUserCompent,
                { id: id },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                if (res) {
                    this.getData();
                }
            });
        },
        /**
         * 批量删除
         */
        batchDelete(e) {
            const selectCount = this.selectedRowKeys.length;
            if (selectCount <= 0) {
                abp.message.warn(this.l("PleaseSelectAtLeastOneItem"));
                return;
            }
            this.message.confirm(
                this.l("ConfirmDeleteXItemsWarningMessage", selectCount),
                res => {
                    if (res) {
                        const ids = _.map(this.selectedRowKeys);
                        this.spinning = true;
                        this._userServiceProxy
                            .batchDelete(ids)
                            .finally(() => {
                                this.spinning = false;
                            })
                            .then(() => {
                                this.selectedRowKeys = [];
                                this.refreshGoFirstPage(
                                    this.selectedPermission
                                );
                                this.$notification["success"]({
                                    message: this.l("SuccessfullyDeleted")
                                });
                            });
                    }
                }
            );
        },
        /**
         * 分页事件
         */
        showTotalFun() {
            return this.l(
                "GridFooterDisplayText",
                this.pageNumber,
                this.totalPages,
                this.totalItems,
                this.pagerange[0],
                this.pagerange[1]
            );
        },
        /**
         * 单个删除
         */
        deleteItem(item) {
            if (item.isStatic) {
                abp.message.warn(
                    this.l(
                        "XUserCannotBeDeleted",
                        AppConsts.userManagement.defaultAdminUserName
                    )
                );
                return;
            }
            this.spinning = true;
            this._userServiceProxy
                .delete(item.id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(() => {
                    this.refreshGoFirstPage(this.selectedPermission);
                    this.$notification["success"]({
                        message: this.l("SuccessfullyDeleted")
                    });
                });
        },
        /**
         * 清空选择
         */
        restCheckStatus() {
            this.selectedRowKeys = [];
        },
        /**
         * table选择事件
         */
        onSelectChange(selectedRowKeys, selectedRows) {
            this.selectedRowKeys = selectedRowKeys;
            this.selectedRows = selectedRows;
        },
        /**
         * 分页
         */
        onChange(page, pageSize) {
            this.pageNumber = page;
            this.request.skipCount = (page - 1) * this.request.maxResultCount;
            this.getData();
        },
        showSizeChange(current, size) {
            this.pageNumber = 1;
            this.request.maxResultCount = size;
            this.getData();
        },
        /**
         * 选择完权限过滤
         */
        refreshGoFirstPage(data) {
            this.selectedPermission = data;
            this.request = { maxResultCount: 10, skipCount: 0 };
            this.getData();
        },
        /**
         * 角色过滤选择
         */
        selectedRoleChange(data) {
            this.role = data;
            this.getData();
        },
        /**
         * 是否是管理员
         */
        isAdmin(item) {
            return (
                item.userName === AppConsts.userManagement.defaultAdminUserName
            );
        },
        /**
         * 是否显示解锁按钮
         */
        enabledUnlock() {
            return (
                this.isGranted("Pages.Administration.Users.Edit") &&
                this.setting.getBoolean(
                    "Abp.Zero.UserManagement.UserLockOut.IsEnabled"
                )
            );
        },
        /**
         * 导出为excel
         */
        exportToExcel() {
            this._userServiceProxy.getUsersToExcel().then(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
        },
        /**
         * 下载excel模版
         */
        ImportUsersSampleFile() {
            this._fileDownloadService.downloadTemplateFile(
                "ImportUsersSampleFile.xlsx"
            );
        },
        /**
         *
         */
        uploadChange(info) {
            let fileList = [...info.fileList];
            fileList = fileList.slice(-2);
            fileList = fileList.map(file => {
                if (file.response) {
                    file.url = file.response.url;
                }
                return file;
            });
            this.fileList = fileList;
            if (info.file.status === "done") {
                this.refreshGoFirstPage();
            }
        },
        /**
         * 使用此账户登录
         */
        tenantImpersonateLogin(item) {
            impersonationService.impersonate(
                item.id,
                this._appSessionService.tenantId
            );
        },
        /**
         * 权限
         */
        editUserPermissions(item) {
            ModalHelper.create(
                EditUserPermissionsCompent,
                { id: item.id, userName: item.userName },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                console.log(res);
                if (res) {
                    this.getData();
                }
            });
        },
        /**
         * 解锁
         */
        unlockTenantAdminUser(item) {
            this.spinning = true;
            this._userServiceProxy
                .unlock({
                    id: item.id
                })
                .finally(() => {
                    this.spinning = false;
                })
                .then(() => {
                    this.getData();
                    this.notify.success(this.l("SuccessfullyUnlock"));
                });
        }
    }
};
</script>

<style scoped lang="less">
@import "./users.less";
</style>
