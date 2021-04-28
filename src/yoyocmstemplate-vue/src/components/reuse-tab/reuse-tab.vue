<template>
    <div :class="{'reuse-tab_pad':isPad}" class="reuse-tab ad-rt fixed">
        <a-tabs :activeKey="pos" :animated="false" type="line">
            <template v-for="item of list">
                <a-tab-pane :key="item.path">
                    <template slot="tab">
                        <span @click="to($event, item)" class="name">
                            {{item.displayTitle}}
                        </span>
                        <a-icon @click="close($event, item)"
                                class="reuse-tab__op"
                                type="close"
                                v-if="item.closable">
                        </a-icon>
                    </template>
                </a-tab-pane>
            </template>
        </a-tabs>
    </div>
</template>

<script>
    import {layoutService, reuseTabService} from '@/shared/common';

    export default {
        name: "reuse-tab",
        data() {
            return {
                listVal: [],
                posVal: null,
                change$: null,
                changePos$: null,
            }
        },
        computed: {
            list() {
                return this.listVal;
            },
            pos() {
                return this.posVal;
            },
            isPad() {
                return layoutService.data.isPad;
            }
        },
        created() {
            this.change$ = reuseTabService.change.subscribe((tabs) => {
                this.listVal = tabs;
            });
            this.changePos$ = reuseTabService.changePos.subscribe((to) => {
                this.posVal = to;
            });
        },
        methods: {
            to(event, item) {
                reuseTabService.to(item);
            },
            close(event, item) {
                reuseTabService.remove(item);
            },
        },
        destroyed() {
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
    @import "./style/index.less";
</style>
