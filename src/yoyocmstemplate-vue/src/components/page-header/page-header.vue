<template>
  <div class="page-header">
    <div class="page-header-index-wide">
      <s-breadcrumb />
      <div class="detail">
        <div class="main" v-if="!$route.meta.hiddenHeaderContent">
          <div class="row">
            <img v-if="logo" :src="logo" class="logo"/>
            <h1 v-if="title" class="title">{{ title }}</h1>
            <div class="action">
              <slot name="action"></slot>
            </div>
          </div>
          <div class="row">
            <div v-if="avatar" class="avatar">
              <a-avatar :src="avatar" />
            </div>
            <div v-if="this.$slots.content" class="headerContent">
              <slot name="content"></slot>
            </div>
            <div v-if="this.$slots.extra" class="extra">
              <slot name="extra"></slot>
            </div>
          </div>
          <div>
            <slot name="pageMenu"></slot>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Breadcrumb from '@/components/tools/Breadcrumb'
export default {
  name: 'PageHeader',
  components: {
    's-breadcrumb': Breadcrumb
  },
  props: {
    title: {
      type: [String,],
      required: false
    },
    logo: {
      type: String,
      default: '',
      required: false
    },
    avatar: {
      type: String,
      default: '',
      required: false
    }
  },
  data () {
    return {}
  }
}
</script>

<style lang="less" scoped>
    @import "./style/index.less";
</style>
