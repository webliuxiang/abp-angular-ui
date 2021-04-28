<template>
    <section class="organization-container">
        <page-header :title="l('OrganizationUnits')"></page-header>
        <a-row :gutter="8">
            <a-col :span="8" class="organization-unit-tree-panel">
                <organization-unit-tree-panel @selectedNode="selectedNodeFunc"></organization-unit-tree-panel>
            </a-col>
            <a-col :span="16" class="organization-unit-tree-panel">
                <a-row :gutter="8" class="units-header">
                    <a-col :span="24" class="title">
                        <a-icon type="team" />
                        <span v-if="selectedTree && selectedTree.hasOwnProperty('id')">{{ selectedTree.displayName }}</span>
                    </a-col>
                </a-row>
                <a-row class="form">
                    <!-- 暂无数据 -->
                    <div class="no-data" v-if="!selectedTree || !selectedTree.hasOwnProperty('id')">
                        <p>{{ l('SelectAnOrganizationUnitToSeeMembers') }}</p>
                    </div>
                    <a-tabs defaultActiveKey="1" v-if="selectedTree && selectedTree.hasOwnProperty('id')">
                        <!-- 用户 -->
                        <a-tab-pane :tab="l('Users')" key="1">
                            <organization-unit-members-panel></organization-unit-members-panel>
                        </a-tab-pane>
                        <!-- 角色 -->
                        <a-tab-pane :tab="l('Roles')" key="2" forceRender>
                            <organization-unit-role-panel></organization-unit-role-panel>
                        </a-tab-pane>
                    </a-tabs>
                </a-row>
            </a-col>
        </a-row>
    </section>
</template>

<script>
import { PagedListingComponentBase } from "@/shared/component-base";
import OrganizationUnitTreePanel from "./organization-unit-tree-panel/organization-unit-tree-panel";
import OrganizationUnitMembersPanel from "./organization-unit-members-panel/organization-unit-members-panel";
import OrganizationUnitRolePanel from "./organization-unit-role-panel/organization-unit-role-panel";

export default {
    name: "organization-units",
    mixins: [PagedListingComponentBase],
    components: {
        OrganizationUnitTreePanel,
        OrganizationUnitMembersPanel,
        OrganizationUnitRolePanel
    },
    data() {
        return {
            // 选择的树结构
            selectedTree: {}
        };
    },
    mounted() {
        this.selectedTree = {};
    },
    methods: {
        /**
         * 选择树结构
         */
        selectedNodeFunc(data) {
            this.selectedTree = data;
        }
    }
};
</script>

<style scoped lang="less">
.organization-unit-tree-panel {
    background-color: #fff;
    border: 1px solid #e8e8e8;
}
.units-header {
    height: 50px;
    border-bottom: 1px solid #e8e8e8;
    > .title {
        line-height: 50px;
        font-size: 16px;
        color: rgba(0, 0, 0, 0.85);
        font-weight: 500;
        margin-left: 20px;
    }
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
/deep/.ant-tabs-bar {
    margin: 0 20px;
}
// 暂无数据
.no-data {
    border: 1px solid #e8e8e8;
    margin: 20px;
    p {
        text-align: center;
        margin-bottom: 0;
        line-height: 120px;
    }
}
</style>
