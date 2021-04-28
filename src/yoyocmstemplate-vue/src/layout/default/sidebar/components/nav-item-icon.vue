<template functional>
    <span v-if="props.data">
        <template v-if="props.data.type==='icon'">
            <a-icon :type="props.data.value"></a-icon>
        </template>
        <template v-if="props.data.type==='img'">
            <img :src="props.data.value" class="sidebar-nav__item-icon sidebar-nav__item-img" alt="">
        </template>
        <template v-else>
            <i class="sidebar-nav__item-icon" :class="[props.data.value]">
            </i>
        </template>

        <span class="nav-menu-text">
            {{props.text}}
        </span>
    </span>
</template>

<script>
    export default {
        name: "nav-item-icon",
        props: ['data', 'text']
    }
</script>

<style lang="less" scoped>
    /** 隐藏文字内容 */
    .ant-menu-inline-collapsed {
        .nav-menu-text {
            display: none;
        }
    }
</style>
