<template functional>
    <a-sub-menu :key="props.menuInfo.key">
        <span slot="title">
            <nav-item-icon :data="props.menuInfo.icon" :text="props.menuInfo.text"></nav-item-icon>
        </span>

        <template v-for="item in props.menuInfo.children">
            <a-menu-item :key="item.key"
                         @click="props.click(item)"
                         v-if="!props.hasChildren(item)&&!item._hidden&&item._aclResult">
                <nav-item-icon :data="item.icon" :text="item.text"></nav-item-icon>
            </a-menu-item>
            <sub-menu-item :key="item.key" :menu-info="item" v-else/>
        </template>
    </a-sub-menu>
</template>
<script>
    import NavItemIcon from './nav-item-icon';

    export default {
        props: ['menuInfo', 'hasChildren', 'click'],
        components: {NavItemIcon},
    };
</script>
