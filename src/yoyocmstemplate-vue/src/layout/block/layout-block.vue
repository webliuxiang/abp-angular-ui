<template>
   <div >
    <div class="content">
      <div class="page-header-index-wide">
        <slot>
            <router-view ref="content" />
        </slot>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "layout-block-keep-alive",
};
</script>

<style lang="less" scoped>
 .content {
    .link {
      margin-top: 16px;
      &:not(:empty) {
        margin-bottom: 16px;
      }
      a {
        margin-right: 32px;
        height: 24px;
        line-height: 24px;
        display: inline-block;
        i {
          font-size: 24px;
          margin-right: 8px;
          vertical-align: middle;
        }
        span {
          height: 24px;
          line-height: 24px;
          display: inline-block;
          vertical-align: middle;
        }
      }
    }
  }
  .page-menu-search {
    text-align: center;
    margin-bottom: 16px;
  }
  .page-menu-tabs {
    margin-top: 48px;
  }
  .extra-img {
    margin-top: -60px;
    text-align: center;
    width: 195px;
    img {
      width: 100%;
    }
  }
  .mobile {
    .extra-img{
      margin-top: 0;
      text-align: center;
      width: 96px;
      img{
        width: 100%;
      }
    }
  }
</style>
