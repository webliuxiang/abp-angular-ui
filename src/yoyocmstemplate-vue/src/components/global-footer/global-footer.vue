<template>
    <div>
        <div class="global-footer__links" v-if="$links.length > 0">
            <a :key="index"
               @click="to(item)"
               class="global-footer__links-item"
               v-for="(item, key, index) in $links"
            >{{item.title}}</a>
        </div>
        <div class="global-footer__copyright">
            <slot></slot>
        </div>
    </div>
</template>

<script>
    export default {
        name: "global-footer",
        props: ["links"],
        computed: {
            $links: vm => {
                return vm.links;
            }
        },
        methods: {
            to(item) {
                if (!item.href) {
                    return;
                }
                if (item.blankTarget) {
                    this.win.open(item.href);
                    return;
                }
                if (/^https?:\/\//.test(item.href)) {
                    window.location.href = item.href;
                } else {
                    this.$router.push({path: item.href});
                }
            }
        }
    };
</script>

<style lang="less" scoped>
    @import "./styles/index.less";
</style>
