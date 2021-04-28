<template>
  <div>
    <page-header title="关于本52ABP框架模板"></page-header>
    <a-row :gutter="16">
      <a-col :span="8">
        <a-card title="52ABP框架总体介绍" :bordered="false">
          <template slot="extra">
            <a href="http://www.52abp.com ">More</a>
          </template>
          <p>
            52ABP是一个整合了前后端的强力框架，
            后端是对.NET Core和ABP框架进行了二次的封装和整合
            前端是在Angular的基础上整合了Ng-Zorro、Ng-alain等组件
            架构层面也是一个基于DDD(领域驱动设计)的经典分层架构 目的是成为一个强大的基础设施符合国人开发习惯的框架现代WEB应用框架。
          </p>
        </a-card>
      </a-col>
      <a-col :span="8">
        <a-card title="什么是52ABP PRO ?" :bordered="false">
          <template slot="extra">
            <a href="http://www.52abp.com ">More</a>
          </template>

          <div>
            52ABP_PRO是一个整合了前后端的强力框架， 后端是对.NET Core和ABP框架进行了二次的封装和整合 前端是在Angular的基础上整合了Ng-Zorro、Ng-alain等组件
            架构层面也是一个基于DDD(领域驱动设计)的经典分层架构 目的是成为一个强大的基础设施符合国人开发习惯的框架现代WEB应用框架。
            <ul>
              <li>
                项目在线预览地址：
                <a
                  href="https://admin.yoyocms.com/"
                  target="_blank"
                >https://admin.yoyocms.com/</a>
              </li>
            </ul>
          </div>
        </a-card>
      </a-col>
      <a-col :span="8">
        <a-card title="什么是Ng Alain    ?" :bordered="false">
          <div>一个基于 Antd 中后台前端解决方案，提供更多通用性业务模块，让开发者更加专注于业务。</div>
        </a-card>
      </a-col>
    </a-row>

    <a-row :gutter="16">
      <a-col :span="8">
        <a-card title="源代码地址" :bordered="false">
          <p>
            此模板是收费模板，您正在访问的是52ABP PRO 版本，此模板的地址是。
            <a
              href="https://gitee.com/aiabpedu/LTMCompanyName.YoyoCmsTemplate"
              target="_blank"
            >https://gitee.com/aiabpedu/LTMCompanyName.YoyoCmsTemplate</a>
          </p>

          <p>
            如果您不熟悉ABP框架，可以看看我录制的公开课视频：
            <a href="http://t.cn/EGp1kTh">ASP.NET Core框架ABP实战案例</a>
          </p>
          <p>
            当然我们也出了收费视频，这个也是我们推荐您食用的课程：
            <a href="https://dwz.cn/kQydQrTe" target="_blank">
              <label>
                【推荐收费课程】52abp框架asp.net
                core & Angular全栈开发实战视频课程
              </label>
            </a>
          </p>
        </a-card>
      </a-col>

      <a-col :span="8">
        <a-card title="视频课程地址" :bordered="false">
          <p>
            当然我们也出了收费视频，这个也是我们推荐您食用的课程：
            <a href="https://dwz.cn/kQydQrTe" target="_blank">
              <label>
                【推荐收费课程】52abp框架asp.net
                core & Angular全栈开发实战视频课程
              </label>
            </a>
          </p>
          <p>
            更多课程内容：
            <a href="http://52abp.ke.qq.com/">http://52abp.ke.qq.com/</a>
            也可以关注微信公众号
            <img
              class="img"
              src=" //www.52abp.com/imgs/common/web_wechat_QR.jpg"
              width="450px"
              height="450px"
            />
          </p>
        </a-card>
      </a-col>
    </a-row>
  </div>
</template>

<script>
export default {
  name: "about",
};
</script>

<style scoped>
</style>
