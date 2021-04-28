<template>
    <a-spin @click="clearMenu" :spinning="spinning">
        <a-row :gutter="8" class="header">
            <a-col :span="16">
                <p class="left">
                    <a-icon type="share-alt" />
                    {{ l('OrganizationTree') }}
                </p>
            </a-col>
            <a-col :span="8" v-if="isGranted('Pages.Administration.OrganizationUnits.ManageOrganizationTree')">
                <p>
                    <a @click="addUnit(null)">
                        <a-icon type="plus" /> {{ l('AddRootUnit') }} </a>
                    <a :title="l('Refresh')" @click="getData()">
                        <a-icon type="reload" /></a>
                </p>
            </a-col>
        </a-row>
        <a-row class="tree">
            <a-tree :treeData="treeData" showIcon defaultExpandAll @select="onSelect" @rightClick="onRightClick" @expand="onExpand">
                <a-icon type="folder" slot="folder" />
                <a-icon type="file" slot="file" />
                <a-icon type="folder-open" slot="folder-open" />
                <template slot="custom" slot-scope="{ expanded }">
                    <a-icon :type="expanded ? 'folder-open' : 'folder'" />
                </template>
            </a-tree>
            <div :style="tmpStyle" class="right-click-item" v-if="NodeTreeItem">
                <ul>
                    <li @click="editUnit()">
                        <a-icon type="edit" />
                        <span>
                            {{ l('Edit') }}
                        </span>
                    </li>
                    <li @click="addSubUnit()">
                        <a-icon type="plus" />
                        <span>
                            {{ l('AddSubUnit') }}
                        </span>
                    </li>
                    <li @click.stop="deleteProp($event)">
                        <!-- <a-icon type="delete" />
                        <span>
                            {{ l('Delete') }}
                        </span> -->
                        <a-popconfirm
                            placement="right"
                            :title="l('ConfirmDeleteWarningMessage')"
                            @confirm="deleteconfirm"
                            @cancel="clearMenu"
                            :okText="l('Ok')"
                            :cancelText="l('Cancel')">
                            <a-icon type="delete" />
                            <span>
                                {{ l('Delete') }}
                            </span>
                        </a-popconfirm>
                    </li>
                </ul>
            </div>
        </a-row>
    </a-spin>
</template>

<script>
import { AppComponentBase } from "@/shared/component-base";
import { OrganizationUnitServiceProxy } from "@/shared/service-proxies";
import CreateOrEditOrganiaztionUnit from "../create-or-edit-organiaztion-unit/create-or-edit-organiaztion-unit";
import { ModalHelper } from "@/shared/helpers";
import Bus from "@/shared/bus/bus";

export default {
    name: "organization-unit-tree-panel",
    mixins: [AppComponentBase],
    data() {
        return {
            spinning: false,
            _organizationUnitServiceProxy: null,
            _ouData: [],
            treeData: [],
            NodeTreeItem: null, // 右键菜单
            tmpStyle: "",
            // 选中的item
            activedNode: {}
        };
    },
    created() {
        this._organizationUnitServiceProxy = new OrganizationUnitServiceProxy(
            this.$apiUrl,
            this.$api
        );
        this.$nextTick(() => {
            Bus.$on("reloadOrganizationUnitTree", data => {
                if (data) {
                    this.getData();
                }
            });
        });
        this.getData();
        console.log(this.activedNode);
    },
    beforeDestroy() {
        Bus.$off("reloadOrganizationUnitTree");
    },
    methods: {
        getData() {
            this.spinning = true;
            this._organizationUnitServiceProxy
                .getAllOrganizationUnitList()
                .finally(() => {
                    this.spinning = false;
                })
                .then(result => {
                    console.log(result);
                    this._ouData = result.items;
                    this.treeData = this.treeDataMap();
                    console.log(this.treeData);
                });
        },
        /**
         * 重组Tree数据
         */
        treeDataMap() {
            const ouDtataParentIsNull = _.filter(
                this._ouData,
                item => item.parentId === null
            );
            return ouDtataParentIsNull.map(item =>
                this._recursionGenerateTree(item)
            );
        },

        /**
         * 递归重组特性菜单为nzTree数据类型
         * @param item 组织机构项
         */
        _recursionGenerateTree(item) {
            // 叶子节点
            const childs = _.filter(
                this._ouData,
                child => child.parentId === item.id
            );
            // 父节点 无返回undefined
            const parentOu = _.find(this._ouData, p => p.id === item.parentId);
            const _treeNode = {
                title: item.displayName + "(" + item.memberCount + ")",
                key: item.id.toString(),
                isLeaf: childs && childs.length <= 0,
                slots: {
                    icon: childs && childs.length > 0 ? "folder" : "file"
                },
                expanded: true,
                isMatched: true,
                code: item.code,
                memberCount: item.memberCount,
                dto: item,
                parent: parentOu,
                children: []
            };
            if (childs && childs.length) {
                childs.forEach(itemChild => {
                    const childItem = this._recursionGenerateTree(itemChild);
                    _treeNode.children.push(childItem);
                });
            }
            return _treeNode;
        },
        /**
         * 选中item
         */
        onSelect(selectedKeys, info) {
            this.activedNode = this._ouData.find(
                item => parseInt(item.id) == parseInt(selectedKeys[0])
            );
            this.$emit("selectedNode", this.activedNode);
            this.$nextTick(() => {
                Bus.$emit("selectedNode", this.activedNode);
            });
            // console.log("selected", this.activedNode);
        },
        /**
         * 展开
         */
        onExpand(onExpandarr) {
            this.resetExpandIcon(this.treeData);
            for (let i in onExpandarr) {
                this.setExpandIcon(onExpandarr[i], this.treeData);
            }
        },
        /**
         * 还原图标
         */
        resetExpandIcon(item) {
            for (let i in item) {
                if (!item[i].isLeaf) {
                    item[i].slots.icon = "folder";
                }
                if (item[i].children.length) {
                    this.resetExpandIcon(item[i].children);
                }
            }
        },
        /**
         * 设置图标
         */
        setExpandIcon(id, item) {
            for (let i in item) {
                if (item[i].key == id) {
                    item[i].slots.icon = "folder-open";
                }
                if (item[i].children.length) {
                    for (let j in item[i].children) {
                        this.setExpandIcon(id, item[i].children);
                    }
                }
            }
        },
        /**
         * 右键事件
         */
        onRightClick({ event, node }) {
            const x =
                event.currentTarget.offsetLeft +
                event.currentTarget.clientWidth;
            const y = event.currentTarget.offsetTop;
            this.NodeTreeItem = {
                pageX: x,
                pageY: y,
                id: node._props.eventKey,
                title: node._props.title,
                parentOrgId: 0
            };
            this.tmpStyle = {
                position: "absolute",
                maxHeight: 40,
                textAlign: "center",
                left: `${x + 10 - 0}px`,
                top: `${y + 6 - 0}px`,
                display: "flex",
                flexDirection: "row"
            };
        },
        /**
         * 用于点击空白处隐藏增删改菜单
         */
        clearMenu() {
            this.NodeTreeItem = null;
        },
        /**
         * 添加组织
         */
        addUnit(activedNode) {
            ModalHelper.create(
                CreateOrEditOrganiaztionUnit,
                {
                    organizationUnit: {
                        parentId: activedNode ? activedNode.id : "",
                        parentDisplayName: activedNode
                            ? activedNode.displayName
                            : ""
                    }
                },
                {
                    width: "200px"
                }
            ).subscribe(res => {
                if (res) {
                    console.log(res);
                    this.getData();
                }
            });
        },
        /**
         * 编辑组织机构
         */
        editUnit() {
            const canManageOrganizationTree = this.isGranted(
                "Pages.Administration.OrganizationUnits.ManageOrganizationTree"
            );
            if (!canManageOrganizationTree) {
                this.$notification["error"]({
                    message: this.l("YouHaveNoOperatingPermissions")
                });
                return;
            }
            if (this.activedNode.id) {
                const ouPars = {
                    id: parseInt(this.activedNode.id),
                    displayName: this.activedNode.displayName
                };
                ModalHelper.create(CreateOrEditOrganiaztionUnit, {
                    organizationUnit: ouPars
                }).subscribe(res => {
                    if (res) {
                        this.getData();
                    }
                });
            }
        },
        /**
         * 添加子节点
         * @param node 当前选中节点
         */
        addSubUnit() {
            const canManageOrganizationTree = this.isGranted(
                "Pages.Administration.OrganizationUnits.ManageOrganizationTree"
            );
            if (!canManageOrganizationTree) {
                this.$notification["error"]({
                    message: this.l("YouHaveNoOperatingPermissions")
                });
                return;
            }
            if (this.activedNode.id) {
                this.addUnit(this.activedNode);
            }
        },
        /**
         * 删除组织事件
         */
        deleteProp(e) {
            e.stopPropagation();
        },
        /**
         * 删除组织结构
         */
        deleteconfirm(e) {
            const canManageOrganizationTree = this.isGranted(
                "Pages.Administration.OrganizationUnits.ManageOrganizationTree"
            );
            if (!canManageOrganizationTree) {
                this.$notification["error"]({
                    message: this.l("YouHaveNoOperatingPermissions")
                });
                return;
            }
            if (this.activedNode.id) {
                this._organizationUnitServiceProxy
                    .delete(parseInt(this.activedNode.id))
                    .then(() => {
                        this.$notification["error"]({
                            message: this.l("SuccessfullyDeleted")
                        });
                        this.clearMenu();
                        this.getData();
                    });
            }
        }
    }
};
</script>

<style scoped lang="less">
.header {
    height: 50px;
    border-bottom: 1px solid #e8e8e8;
    p {
        line-height: 50px;
        &.left {
            font-size: 16px;
            color: rgba(0, 0, 0, 0.85);
            font-weight: 500;
            margin-left: 20px;
        }
        a {
            margin-left: 10px;
        }
    }
}
.tree {
    margin: 20px;
}
.right-click-item {
    position: relative;
    margin: 0;
    padding: 4px 0;
    text-align: left;
    list-style-type: none;
    background-color: #fff;
    background-clip: padding-box;
    border-radius: 4px;
    outline: none;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
    -webkit-transform: translate3d(0, 0, 0);
    ul {
        list-style: none;
        margin: 0;
        padding: 0;
        li {
            clear: both;
            margin: 0;
            padding: 5px 12px;
            color: rgba(0, 0, 0, 0.65);
            font-weight: normal;
            font-size: 14px;
            line-height: 22px;
            white-space: nowrap;
            cursor: pointer;
            transition: all 0.3s;
            text-align: left;
        }
    }
}
</style>
