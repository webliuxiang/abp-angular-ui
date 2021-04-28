<template>
    <a-spin :spinning="spinning">
        <page-header :title="l('FileManager')"></page-header>
        <a-card :bordered="false">
            <!-- 按钮 -->
            <a-row :gutter="8">
                <a-col :sm="22" :md="20">
                    <!-- <a-button type="primary">
                        <a-icon type="upload" />
                        {{ l('FileUpload') }}
                    </a-button> -->
                    <a-upload
                        class="fileUpload"
                        :action="uploadFileUrl"
                        :multiple="false"
                        :fileList="uploadfileList"
                        :headers="uploadHeaders"
                        @change="uploadChange">
                        <a-button type="primary">
                            <a-icon type="upload" />
                            <span>{{l("FileUpload")}}</span>
                        </a-button>
                    </a-upload>
                    <a-button @click="openDirectory">
                        <a-icon type="plus" />
                        {{ l('CreateDirectory') }}
                    </a-button>
                    <a-button @click="getData">
                        <a-icon type="reload" />
                        {{ l('Refresh') }}
                    </a-button>
                </a-col>
                <a-col :sm="2" :md="4">
                    <a-button @click="showType = 'big'" :disabled="showType === 'big'">
                        <a-icon type="appstore" />
                    </a-button>
                    <a-button class="bars-btn" @click="showType = 'small'" :disabled="showType === 'small'">
                        <a-icon type="bars" />
                    </a-button>
                </a-col>
            </a-row>
            <!-- big横向展示 -->
            <section class="big-list-item" v-show="showType==='big'">
                <a-row :gutter="10">
                    <a-col :sm="3" v-if="path.length > 1" @click="back()" class="file-item">
                        <i class="file-item__icon iconfont icon-rollback"></i>
                        <div class="file-item__name" style="text-align:center">...</div>
                        <!-- 路径比较深的显示back按钮 -->
                    </a-col>
                    <a-col :sm="3" v-for="item in fileList" :key="item.id" :class="{ 'file_item__selected': item.selected }" @click="cho(item)">
                        <div class="dd-btn">
                            <a-dropdown>
                                <a class="ant-dropdown-link" @click="e => e.preventDefault()">
                                    <a-icon type="ellipsis" />
                                </a>
                                <a-menu slot="overlay">
                                    <a-menu-item>
                                        <a-popconfirm placement="top" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="copyImg(item.id)">
                                            <template slot="title">
                                                {{ l('AreYouSure') }}
                                            </template>
                                            <a href="javascript:;">Copy</a>
                                        </a-popconfirm>
                                    </a-menu-item>
                                    <a-menu-item>
                                        <a href="javascript:;" @click="copyData(item.path, 'link')">{{ l('CopyUrl') }}</a>
                                    </a-menu-item>
                                    <a-menu-item>
                                        <a href="javascript:;" @click="copyData(item.path, 'code')">{{ l('CopyCode') }}</a>
                                    </a-menu-item>
                                    <a-menu-item>
                                        <a href="javascript:;" @click="rename(item)">{{ l('ReName') }}</a>
                                    </a-menu-item>
                                    <a-menu-item>
                                        <a href="javascript:;" @click="movefile(item)">{{ l('Move') }}</a>
                                    </a-menu-item>
                                    <a-menu-item>
                                        <!-- <a href="javascript:;">{{ l('DeleteSysFile') }}</a> -->
                                        <a-popconfirm placement="top" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="remove(item.id)">
                                            <template slot="title">
                                                {{ l('AreYouSure') }}
                                            </template>
                                            <a href="javascript:;">{{ l('DeleteSysFile') }}</a>
                                        </a-popconfirm>
                                    </a-menu-item>
                                </a-menu>
                            </a-dropdown>
                        </div>
                        <i
                            v-if="item.dir === true"
                            class="file-item__icon iconfont   "
                            :class="'icon-' + item.fileExt"
                            style="font-size: 29px;"></i>
                        <div v-if="item.dir === false">
                            <i
                                v-if="!item.isImg"
                                class="file-item__icon iconfont   "
                                :class="'icon-' + item.fileExt"
                                style="font-size: 29px;"></i>
                            <div class="file-item__img" v-if="item.isImg" :style="{ 'background-image': 'url(' + item.path + ')' }"></div>
                        </div>
                        <div class="file_item__name">{{ item.name }}</div>
                        <div class="file_item__pixel">
                            <span v-if="item.isImg">{{ item.width }}x{{ item.height }}</span>
                        </div>
                    </a-col>
                </a-row>
            </section>
            <!-- smalltable展示 -->
            <section class="small-list-item" v-show="showType==='small'">
                <div v-if="path.length > 1" @click="back()" class="back-item">
                    <i class="file-item__icon iconfont icon-rollback"></i>&nbsp;&nbsp;...
                </div>
                <a-table :columns="columns" :pagination="false" :data-source="fileList" :customRow="tableclick">
                    <span slot="name" slot-scope="text, record">
                        <div class="icon-container">
                            <i
                                v-if="record.dir === true"
                                class="file-item__icon iconfont   "
                                :class="'icon-' + record.fileExt"
                                style="font-size: 29px;"></i>
                            <i
                                v-if="record.dir === false && !record.isImg"
                                class="file-item__icon iconfont   "
                                :class="'icon-' + record.fileExt"
                                style="font-size: 29px;"></i>
                            <div class="file-item__img" v-if="record.dir === false && record.isImg" :style="{ 'background-image': 'url(' + record.path + ')' }"></div>
                        </div>
                        {{ record.name }}
                    </span>
                    <span slot="pixel" v-if="record.isImg" slot-scope="text, record">
                        {{ record.width }}x{{ record.height }}
                    </span>
                    <span slot="lastModificationTimeStr" class="lastModificationTimeStr" slot-scope="text, record">
                        {{ record.lastModificationTimeStr }}
                        <div class="dd-btn">
                            <a-dropdown>
                                <a class="ant-dropdown-link" @click="e => e.preventDefault()">
                                    <a-icon type="ellipsis" />
                                </a>
                                <a-menu slot="overlay" class="ant-dropdown-link">
                                    <a-menu-item>
                                        <a-popconfirm placement="top" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="copyImg(record.id)">
                                            <template slot="title">
                                                {{ l('AreYouSure') }}
                                            </template>
                                            <a href="javascript:;">Copy</a>
                                        </a-popconfirm>
                                    </a-menu-item>
                                    <a-menu-item>
                                        <a href="javascript:;" @click="copyData(record.path, 'link')">{{ l('CopyUrl') }}</a>
                                    </a-menu-item>
                                    <a-menu-item>
                                        <a href="javascript:;" @click="copyData(record.path, 'code')">{{ l('CopyCode') }}</a>
                                    </a-menu-item>
                                    <a-menu-item>
                                        <a href="javascript:;" @click="rename(record)">{{ l('ReName') }}</a>
                                    </a-menu-item>
                                    <a-menu-item>
                                        <a href="javascript:;" @click="movefile(record)">{{ l('Move') }}</a>
                                    </a-menu-item>
                                    <a-menu-item>
                                        <!-- <a href="javascript:;">{{ l('DeleteSysFile') }}</a> -->
                                        <a-popconfirm placement="top" :okText="l('Ok')" :cancelText="l('Cancel')" @confirm="remove(record.id)">
                                            <template slot="title">
                                                {{ l('AreYouSure') }}
                                            </template>
                                            <a href="javascript:;">{{ l('DeleteSysFile') }}</a>
                                        </a-popconfirm>
                                    </a-menu-item>
                                </a-menu>
                            </a-dropdown>
                        </div>
                    </span>
                </a-table>
            </section>
            <!-- 分页 -->
            <a-pagination
                class="pagination"
                size="middle"
                :total="totalItems"
                showSizeChanger
                showQuickJumper
                :showTotal="showTotalFun"
                @change="onChange"
                @showSizeChange="showSizeChange" />
        </a-card>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { SysFileServiceProxy } from "@/shared/service-proxies";
import { AppConsts } from "@/abpPro/AppConsts";
import CreateDirectory from "./create-directory";
import Rename from "./rename";
import { ModalHelper } from "@/shared/helpers";
import MoveFile from "./move-file";
import { ModalComponentBase } from "@/shared/component-base";
import moment from "moment";
// import { fileDownloadService } from "@/shared/utils";

export default {
    name: "file-manager",
    mixins: [AppComponentBase, ModalComponentBase],
    components: {
        CreateDirectory,
        Rename,
        MoveFile
    },
    data() {
        return {
            spinning: false,
            // 显示类型
            showType: "big",
            _sysFileServiceProxy: "",
            parentId: "",
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
            request: { maxResultCount: 10, skipCount: 0 },
            //  文件列表
            fileList: [],
            filepath: AppConsts.remoteServiceBaseUrl + "/sysfiles/",
            // small table
            columns: [
                {
                    title: this.l("FileName"),
                    dataIndex: "name",
                    align: "left",
                    scopedSlots: { customRender: "name" }
                },
                {
                    title: this.l("Pixel"),
                    dataIndex: "pixel",
                    align: "center",
                    scopedSlots: { customRender: "pixel" }
                },
                {
                    title: this.l("LastModificationTime"),
                    dataIndex: "lastModificationTimeStr",
                    align: "center",
                    scopedSlots: { customRender: "lastModificationTimeStr" }
                }
            ],
            // 上传文件
            uploadfileList: [],
            uploadFileUrl: "",
            uploadHeaders: {},
            // 路劲深度
            path: [0]
        };
    },
    created() {
        this.fullData();
        this._sysFileServiceProxy = new SysFileServiceProxy(
            this.$apiUrl,
            this.$api
        );
        Object.assign(this.uploadHeaders, {
            Authorization: "Bearer " + abp.auth.getToken()
        });
        this.getFileUploadUrl("");
        this.getData();
    },
    methods: {
        /**
         * 获取数据
         */
        getData() {
            this.spinning = true;
            this._sysFileServiceProxy
                .getPaged(
                    this.parentId,
                    "",
                    "",
                    this.request.maxResultCount,
                    this.request.skipCount
                )
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    this.fileList = res.items;
                    this.fileList = res.items.map(item => {
                        item.fileExt = item.fileExt.replace(".", "");
                        item.selected = false;
                        item.path = this.filepath + item.path;
                        item.lastModificationTimeStr = moment(
                            item.lastModificationTime
                        ).format("YYYY-MM-DD hh:mm:ss");
                        return item;
                    });
                    //  this.fileList .map(item=>item.)
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
         * 获取上传文件地址
         */
        getFileUploadUrl(parendId) {
            this.uploadFileUrl =
                AppConsts.remoteServiceBaseUrl +
                "/api/services/app/SysFile/Create?parentId=" +
                parendId;
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
         * big
         * 点击item
         */
        cho(item) {
            if (item.dir) {
                this.next(item);
                return;
            }
            // this.$set(this.fileList, item.selected, !item.selected);
            console.log(item);
            if (this.componentType === "modal") {
                this.success(item);
                this.close();
            }
            // this.selected.emit(i);
            // this.cdr.detectChanges();
        },
        /**
         * 进入文件夹
         */
        next(i) {
            this.path.push(i.id);
            this.parentId = i.id;
            this.getFileUploadUrl(i.id);
            this.getData();
        },
        /**
         *上传文件
         */
        uploadChange(info) {
            this.spinning = true;
            let fileList = [...info.fileList];
            fileList = fileList.slice(-2);
            fileList = fileList.map(file => {
                if (file.response) {
                    file.url = file.response.url;
                }
                return file;
            });
            this.uploadfileList = fileList;
            if (info.file.status === "done") {
                this.spinning = false;
                this.$notification["success"]({
                    message: "文件上传成功"
                });
                this.getData();
            }
        },
        /**
         * 新建文件夹
         */
        openDirectory() {
            ModalHelper.create(
                CreateDirectory,
                {
                    parentId: this.parentId
                },
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
         * 返回上一级
         */
        back() {
            this.path.pop(); // 移除最后一个元素
            const length = this.path.length;
            const tempParentId = this.path[length - 1];
            if (tempParentId === 0) {
                this.parentId = "";
            } else {
                this.parentId = tempParentId.toString();
            }
            this.getFileUploadUrl(this.parentId);
            this.getData();
        },
        /**
         * 单机table
         */
        tableclick(record, index) {
            return {
                on: {
                    click: () => {
                        this.cho(record);
                    }
                }
            };
        },
        /**
         * 复制文件
         * @param id 待copy文件id
         */
        copyImg(id) {
            this.spinning = true;
            this._sysFileServiceProxy
                .copyFile({
                    id: id
                })
                .finally(() => {
                    this.spinning = false;
                })
                .then(res => {
                    res.selected = false;
                    res.path = this.filepath + res.path;
                    // this.fileList.push(res);
                    this.getData();
                    this.$notification["success"]({
                        message: this.l("SavedSuccessfully")
                    });
                });
        },
        /**
         * 复制代码或者链接
         * @param mp url
         * @param type 代码或者链接
         */
        copyData(mp, type) {
            console.log(this.getCode(mp, type));
            this.$copyText(this.getCode(mp, type)).then(() =>
                this.$notification["success"]({
                    message: "Copy Success"
                })
            );
        },
        getCode(mp, type) {
            return type === "link" ? mp : `<img src="${mp}">`;
        },
        /**
         * 重命名
         */
        rename(item) {
            ModalHelper.create(
                Rename,
                {
                    renameItem: item
                },
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
         * 移动文件
         */
        movefile(item) {
            ModalHelper.create(
                MoveFile,
                {
                    item: item
                },
                {
                    width: "400px"
                }
            ).subscribe(res => {
                console.log(res);
                if (res) {
                    this.path = [0];
                    this.parentId = "";
                    this.getFileUploadUrl("");
                    this.getData();
                }
            });
        },
        /**
         * 删除文件
         */
        remove(id) {
            this.spinning = true;
            this._sysFileServiceProxy
                .delete(id)
                .finally(() => {
                    this.spinning = false;
                })
                .then(() => {
                    this.getData();
                    this.$notification["success"]({
                        message: this.l("SuccessfullyDeleted")
                    });
                });
        }
    }
};
</script>

<style scoped lang="less">
/deep/.ant-upload-list-text {
    display: none;
}
.bars-btn {
    margin-left: 0;
}
// 横向展示
.big-list-item {
    margin-top: 20px;
    .iconfont {
        display: block;
        font-size: 40px;
        margin: 16px 0;
        text-align: center;
    }
    /deep/.ant-row {
        width: 100%;
        margin-left: 0 !important;
        margin-right: 0 !important;
    }
    .ant-col-sm-3 {
        box-sizing: border-box;
        padding-top: 15px;
        height: 150px;
        border: 1px solid transparent;
        position: relative;
        cursor: pointer;
        &:hover {
            border: 1px solid rgba(0, 0, 0, 0.05);
            .dd-btn {
                display: block;
            }
        }
        .dd-btn {
            height: 24px;
            width: 24px;
            line-height: 24px;
            border-radius: 50%;
            color: #bfbfbf;
            background-color: #f5f5f5;
            text-align: center;
            cursor: pointer;
            position: absolute;
            margin: auto;
            right: 10px;
            top: 10px;
            display: none;
        }
        // .anticon-ellipsis {
        //     &:hover {
        //         cursor: pointer;
        //     }
        // }
    }
    .file_item__name {
        color: #4e5155;
        height: 28px;
        text-align: center;
        line-height: 28px;
        padding: 0 8px;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }
    .file_item__pixel {
        text-align: center;
    }
    .file-item__img {
        background-color: transparent;
        background-position: center center;
        background-size: cover;
        display: block;
        margin: 0 auto 0.75rem;
        width: 4rem;
        height: 4rem;
        font-size: 2.5rem;
        line-height: 4rem;
    }
    .file_item__selected {
        border-color: #1890ff !important;
        background-color: #e6f7ff !important;
    }
}
// smalltable展示
.small-list-item {
    margin: 20px 0;
    .file-item__img {
        display: block;
        margin: 0 8px;
        width: 32px;
        height: 32px;
        text-align: center;
        font-size: 20px;
        line-height: 32px;
        background-color: transparent;
        background-position: center center;
        background-size: cover;
        float: left;
    }
    .dd-btn {
        height: 24px;
        width: 24px;
        line-height: 24px;
        border-radius: 50%;
        color: #bfbfbf;
        background-color: transparent;
        text-align: center;
        cursor: pointer;
        float: right;
        // position: absolute;
        // margin: auto;
        // right: 10px;
        // top: 10px;
        // display: none;
        .ant-dropdown-link {
            color: transparent;
        }
    }
    /deep/.ant-table-tbody td {
        .icon-container {
            min-height: 10px;
            width: 50px;
            float: left;
            &::before {
                content: "";
            }
        }
        &:last-child:hover {
            .dd-btn {
                background-color: #f5f5f5;
                display: block;
                .ant-dropdown-link {
                    color: #bfbfbf;
                }
            }
        }
    }
    .back-item {
        cursor: pointer;
        height: 50px;
    }
}
.fileUpload {
    float: left;
    margin-right: 10px;
}
// 分页
.pagination {
    text-align: center;
}
</style>
