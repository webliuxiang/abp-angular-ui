<template>

    <vue-perfect-scrollbar class="alain-pro__side-nav-wrap alain-pro__menu">
        <a-menu
            :inlineCollapsed="collapsed"
            :openKeys.sync="openKeys"
            :selectedKeys.sync="selectedKeys"
            :theme="theme"
            mode="inline"
            v-if="list&&list.length!==0">
            <template v-for="item in list">
                <a-menu-item
                    :key="item.key"
                    @click="click(item)"
                    v-if="!hasChildren(item)&&!item._hidden&&item._aclResult">
                    <!--<router-link :to="item.link" @click.native="click(item)"-->
                    <!--active-class="active">-->
                    <!--&lt;!&ndash; 缺少icon &ndash;&gt;-->
                    <!--<nav-item-icon :data="item.icon" :text="item.text"></nav-item-icon>-->
                    <!--</router-link>-->
                    <nav-item-icon :data="item.icon" :text="item.text"></nav-item-icon>

                </a-menu-item>

                <sub-menu-item v-if="hasChildren(item) && item._aclResult" :click="click"
                    :hasChildren="hasChildren"
                    :key="item.key"
                    :menu-info="item" />

                <!-- <sub-menu-item v-else :click="click"
                    :hasChildren="hasChildren"
                    :key="item.key"
                    :menu-info="item" /> -->

            </template>
        </a-menu>
    </vue-perfect-scrollbar>

</template>

<script>
import * as _ from 'lodash';
import VuePerfectScrollbar from 'vue-perfect-scrollbar'
import { layoutService, menuService, Nav, reuseTabService } from '@/shared/common';
import SubMenuItem from './sub-menu-item';
import NavItemIcon from './nav-item-icon';

export default {
    name: "yoyo-sidebar-nav",
    components: {
        VuePerfectScrollbar,
        NavItemIcon,
        SubMenuItem
    },
    data() {
        return {
            change$: null,
            changePos$: null,
            listVal: [], // 菜单数据
            openKeysVal: [], // 普通模式下展开的菜单key
            collapsedOpenKeysVal: [], // 折叠模式下 展开的菜单
            selectedKeysVal: [], // 选中的菜单key
        };
    },
    component: {
        SubMenuItem
    },
    computed: {
        theme() {
            return layoutService.data.theme;
        },
        collapsed() {
            return layoutService.data.collapsed;
        },
        reuseTab() {
            return layoutService.data.reuseTab;
        },
        list: {
            get() {
                return this.listVal;
            },
            set(val) {
                this.listVal = val;
            }

        },
        isPad() {
            return layoutService.data.isPad;
        },
        openKeys: {
            get() {
                if (this.collapsed) {
                    return this.collapsedOpenKeysVal;
                }
                return this.openKeysVal;
            },
            set(val) {
                if (this.collapsed) {
                    this.collapsedOpenKeysVal = val;
                    return;
                }
                this.openKeysVal = val;
            }
        },
        selectedKeys: {
            get() {
                return this.selectedKeysVal;
            },
            set(val) {
                this.selectedKeysVal = val;
            }
        }
    },
    watch: {
        collapsed(val) {
            // 关闭左侧菜单时，清空展开的菜单项
            if (val) {
                this.collapsedOpenKeysVal = [];
            }
        }
    },
    created() {
        this.change$ = menuService.change.subscribe((res) => {
            this.list = res;
        });

        // 订阅复用标签
        if (layoutService.data.reuseTab) {
            this.changePos$ = reuseTabService.changePos.subscribe((res) => {
                this.processSelectAndOpenKeys(res);
            });
        } else {
            this.processSelectAndOpenKeys(this.$route.path);
        }
        if (this.isPad) {
            layoutService.data.collapsed = true;
        }

    },
    methods: {
        click(item) { // 单击菜单项
            if (this.isPad && !this.collapsed) {
                layoutService.data.collapsed = true;
            }
            this.$router.push({ path: item.link });
        },
        hasChildren(item) { // 是否有子项
            if (item.children && item.children.length > 0) {
                return true;
            }
            return false;
        },
        processMenuOpen(currentUrl, menus, parentMenu) { // 处理菜单展开状态
            menus.forEach(item => {
                if (parentMenu && item.link === currentUrl) {
                    parentMenu._open = true;
                }
                if (item.children && item.children.length > 0) {
                    this.processMenuOpen(currentUrl, item.children, item);
                }
            });
        },
        processSelectAndOpenKeys(path) {
            let menuItem = menuService.findByLink(path);

            if (menuItem) {
                this.selectedKeys = [menuItem.key];

                let tmpMenuParent = menuItem.__parent;
                let tmpOpenKeys = [];
                while (true) {
                    if (!tmpMenuParent) {
                        break;
                    }
                    tmpOpenKeys.push(tmpMenuParent.key);
                    tmpMenuParent = tmpMenuParent.__parent
                }

                if (!this.collapsed) {
                    this.openKeys = _.union(this.openKeys, tmpOpenKeys);
                }

            } else {
                this.selectedKeys = [];
            }

        }

    },
    beforeDestroy() {
        if (this.change$) {
            this.change$.unsubscribe();
        }
        if (this.changePos$) {
            this.changePos$.unsubscribe();
        }
    }
}
</script>

<style lang="less" scoped>
</style>
