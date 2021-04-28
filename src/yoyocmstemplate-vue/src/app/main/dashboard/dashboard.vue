<template>
  <div>
    <page-header :title="'52ABP-PRO工作控制台'">
      <template slot="content">
        <div>
          <p>
            52ABP-PRO:是一个围绕中国特色的.NET 全栈开发社区，目前的重心是围绕.Net
            Core和Angular 两个生态，来搭建高可用的应用开发框架。
          </p>
          <p>
            52abp pro
            是一套为中小型企业准备的快速开发模板，按照平台的开发规范，您可以快速实现功能开发，而不用花费太多心思来处理基础设施的内容。
          </p>
        </div>
        <div class="d-flex pt-md intro-container">
          <a
            class="d-flex intro-item"
            target="_blank"
            href="https://www.52abp.com/Wiki/52abp/latest/Getting-Started-Angular"
          >
            <img
              class="pr-sm"
              src="https://gw.alipayobjects.com/zos/rmsportal/MjEImQtenlyueSmVEfUD.svg"
            />快速开始
          </a>
          <a
            class="d-flex intro-item"
            target="_blank"
            href="https://www.52abp.com/Wiki/52abp/latest/Overview-Angular"
          >
            <img
              class="pr-sm"
              src="https://gw.alipayobjects.com/zos/rmsportal/NbuDUAuBlIApFuDvWiND.svg"
            />架构概述
          </a>
          <a
            class="d-flex intro-item"
            target="_blank"
            href="https://www.52abp.com/Wiki/52abp/latest/Index-Angular"
          >
            <img
              class="pr-sm"
              src="https://gw.alipayobjects.com/zos/rmsportal/ohOEPSYdDTNnyMbGuyLb.svg"
            />产品文档
          </a>
          <a
            class="d-flex intro-item"
            target="_blank"
            href="https://www.52abp.com/Wiki/52abp/latest/52ABP-Power-Tools-Intro"
          >
            <img
              class="pr-sm"
              src="https://gw.alipayobjects.com/zos/rmsportal/ohOEPSYdDTNnyMbGuyLb.svg"
            />配套代码生成器
          </a>
        </div>
      </template>
      <template slot="extra">
        <div class="intro-img" style="text-align: center; ">
          <a href="https://dwz.cn/kQydQrTe">
            <img
              class="img-fluid"
              src="https://edu-image.nosdn.127.net/0300916b-24c4-491b-9665-a0d367ac0b9e.jpg?imageView&quality=100&crop=0_0_1920_1077&thumbnail=450y250"
            />
          </a>
        </div>
      </template>
    </page-header>

    <a-row :gutter="16">
      <a-col :xs="24" :sm="12" :md="6" class="mb-md">
        <a-row type="flex" align="middle" class="bg-primary rounded-md">
          <a-col :span="12" class="p-md text-white">
            <div class="h2 mt0">
              在线Demo：
              <a style="color:whitesmoke" href="http://pro.52abp.com">https://pro.52abp.com</a>
            </div>
            <p class="text-nowrap mb0">通过租户注册按钮来实现全功能的预览</p>
          </a-col>
          <!-- <a-col :span="12">
            <g2-mini-bar height="35" color="#fff" borderWidth="3" :padding="[36, 30, 30, 30]"></g2-mini-bar>
          </a-col>-->
        </a-row>
      </a-col>
      <a-col :xs="24" :sm="12" :md="6" class="mb-md" @click="showAdvertising()">
        <a-row type="flex" align="middle" class="bg-success rounded-md">
          <a-col :span="12" class="p-md text-white">
            <div class="h2 mt0">惊爆!点一下,玩一年</div>
            <p class="text-nowrap mb0">手动狗头</p>
          </a-col>
          <!-- <div nz-col nzSpan="12">
            <g2-mini-bar height="35" color="#fff" borderWidth="3" [padding]="[36, 30, 30, 30]"></g2-mini-bar>
          </div>-->
        </a-row>
      </a-col>
      <a-col :xs="24" :sm="12" :md="6" class="mb-md" @click="showAdvertising()">
        <a-row type="flex" align="middle" class="bg-orange rounded-md">
          <a-col :span="12" class="p-md text-white">
            <div class="h2 mt0">惊爆!点一下,玩一年</div>
            <p class="text-nowrap mb0">飞蝗芜湖</p>
          </a-col>
          <!-- <div nz-col nzSpan="12">
           <g2-mini-bar height="35" color="#fff" borderWidth="3" [padding]="[36, 30, 30, 30]"></g2-mini-bar>
          </div>-->
        </a-row>
      </a-col>
      <a-col :xs="24" :sm="12" :md="6" class="mb-md" @click="showAdvertising()">
        <a-row type="flex" align="middle" class="bg-magenta rounded-md">
          <a-col :span="12" class="p-md text-white">
            <div class="h2 mt0">惊爆!点一下,玩一年</div>
            <p class="text-nowrap mb0">Emmmmm...</p>
          </a-col>
          <!--  <div nz-col nzSpan="12">
            <g2-mini-bar height="35" color="#fff" borderWidth="3" [padding]="[36, 30, 30, 30]"></g2-mini-bar>
          </div>-->
        </a-row>
      </a-col>
    </a-row>

    <a-row :gutter="24">
      <a-col :xs="24" :sm="24" :md="24">
        <a-card
          title="52ABP模板项目中的技术栈"
          :bordered="false"
          :loading="isLoading"
          class="ant-card__body-nopadding mb-lg project-list"
        >
          <a-card-grid v-for="item in notice" :key="item.id" class="project-grid">
            <a-card :bordered="false" class="ant-card__body-nopadding mb0">
              <a-card-meta>
                <template slot="title">
                  <div class="card-title">
                    <a-avatar :src="item.logo" size="small" />
                    <a :href="item.href" target="_blank">{{ item.title }}</a>
                  </div>
                </template>
                <template slot="description">
                  <div class="ant-card-meta-description">{{item.description}}</div>
                </template>
              </a-card-meta>
              <div class="project-item">
                <!-- <a @click="msg.info('show user: ' + item.member)">{{item.member}}</a>
                <span v-if="item.updatedAt" class="datetime" :title="item.updatedAt">
                  {{item.updatedAt | _date: 'fn' }}
                </span>-->
              </div>
            </a-card>
          </a-card-grid>
        </a-card>
      </a-col>
    </a-row>

    <!-- 更多资料  -->
    <a-row :gutter="24">
      <a-col :span="24">
        <a-card
          title="更多资料"
          :bordered="false"
          :bodyStyle="{ 'padding-top.px': 12, 'padding-bottom.px': 12 }"
          class="mb-lg"
        >
          <div class="members">
            <a-row :gutter="48">
              <a-col :span="12" v-for="i in members">
                <a :href="i.link" target="_blank">
                  <a-avatar :src="i.logo" :size="'small'" />
                  <span class="member">{{ i.title }}</span>
                </a>
              </a-col>
            </a-row>
          </div>
        </a-card>
      </a-col>
    </a-row>
  </div>
</template>

<script>

let notice = [
  {
    logo: "https://docs.microsoft.com/zh-cn/dotnet/images/hub/netcore.svg",
    title: ".NET Core",
    href: "https://dotnet.github.io/",
    description:
      ".NET CORE 是微软新一代的跨平台的框架， 是.NET Framework的进化版本,.NET Core的核心点： 创新、开源、跨平台，当前版本为.net core2.x"
  },
  {
    logo: "https://aspnetboilerplate.com/images/logos/abp-logo-long.png",
    title: "ABP",
    href: "https://aspnetboilerplate.com/",
    description:
      "ASP.NET Boilerplate是一个用最佳实践和流行技术开发现代WEB应用程序的新起点，它旨在成为一个通用的WEB应用程序框架和项目模板框架"
  },
  {
    logo: "https://avatars2.githubusercontent.com/u/33684174?s=200&v=4",
    title: "52ABP",
    href: "https://www.52abp.com",
    description:
      "52ABP是一个将目前流行的框架进行了整合，核心以ABP框架和ng-zorro为标准进行研发的应用型框架信息。"
  },
  {
    logo: "https://gw.alipayobjects.com/zos/rmsportal/zOsKZmFRdUtvpqCImOVY.png",
    title: "Angular",
    href: "https://aspnetboilerplate.com/",
    description:
      "Angular 是一个开发平台。它能帮你更轻松的构建 Web 应用。Angular 集声明式模板、依赖注入、端到端工具和一些最佳实践于一身，为你解决开发方面的各种..."
  },
  {
    logo: "https://ng.ant.design/assets/img/logo.svg",
    title: "NG-ZORRO",
    href: "https://ng.ant.design",
    description:
      "这里是 Ant Design 的 Angular 实现，开发和服务于企业级后台产品。也是52ABP框架的前端核心之一"
  },
  {
    logo: "https://ng-alain.com/assets/img/logo-color.svg",
    title: "NG Alain",
    href: "https://ng-alain.com/",
    description:
      "一个基于 Antd的设计，整合了NG-ZORRO的 中后台前端解决方案，提供更多通用性业务模块，让开发者更加专注于业务。"
  }
];

let members = [
  {
    id: "members-1",
    title: "52ABP社区",
    logo: "https://avatars2.githubusercontent.com/u/33684174?s=200&v=4",
    link:
      // tslint:disable-next-line:max-line-length
      "https://www.52abp.com"
  },
  {
    id: "members-2",
    title: "视频课程：网易云云课堂",
    logo: "http://pp.myapp.com/ma_icon/0/icon_11357537_1529483301/256",
    link:
      "http://study.163.com/provider/400000000309007/course.htm?share=2&shareId=400000000309007"
  },
  {
    id: "members-1",
    title: "微信公众号-角落的白板报",
    logo:
      "http://wx.qlogo.cn/mmhead/Q3auHgzwzM6PQV7JWIpJ2seavD5UuzCVWPyZs0SVqFkdYRyc3HQUkg/0",
    link:
      // tslint:disable-next-line:max-line-length
      "https://mp.weixin.qq.com/profile?src=3&timestamp=1532171698&ver=1&signature=hRm1TI4zh80GpKxR5LYIc9SyUcyUPiM1EE8qlUdm4dbNzP06DOfA0HKfgajY2Dyj2xku0anPcrOwE8f7mjlwxg=="
  },
  {
    id: "members-3",
    title: "视频课程：腾讯课堂",
    logo:
      "http://is4.mzstatic.com/image/thumb/Purple111/v4/06/e9/d3/06e9d3e2-4e07-f556-a765-7c8749f09c12/source/1200x630bb.jpg",
    link: "https://ke.qq.com/course/287301?tuin=2522cdf3"
  },

  {
    id: "members-5",
    title: "github源代码",
    logo: "https://major.io/wp-content/uploads/2014/08/github.png",
    link: "https://github.com/52ABP/LTMCompanyNameFree.YoyoCmsTemplate"
  },
  {
    id: "members-5",
    title: "微软MVP",
    logo: "https://mvp.microsoft.com/Content/Images/mvp-banner.png",
    link: "https://mvp.microsoft.com/zh-CN/PublicProfile/5002741"
  },
  {
    id: "members-4",
    title: "博客园-博文地址",
    logo: "",
    link: "https://www.cnblogs.com/wer-ltm/"
  },

  {
    id: "members-5",
    title: "博文地址:知乎专栏",
    logo:
      "http://wx.qlogo.cn/mmhead/Q3auHgzwzM6PQV7JWIpJ2seavD5UuzCVWPyZs0SVqFkdYRyc3HQUkg/0",
    link: "https://zhuanlan.zhihu.com/52abp"
  }
  // 、、https://github.com/52ABP/LTMCompanyNameFree.YoyoCmsTemplate
];

import { AppComponentBase } from "@/shared/component-base";
import { ModalHelper } from "@/shared/helpers";
import { AdvertisingModal } from "../advertising";

export default {
  mixins: [AppComponentBase],
  name: "dashboard",
  data() {
    return {
      isLoading: false,
      notice,
      members
    };
  },
  methods: {
    showAdvertising() {
      ModalHelper.create(AdvertisingModal, null, {
        width: "900px"
      }).subscribe(() => {});
    }
  }
};
</script>

<style lang="less" scoped>
@import "./dashboard.less";
</style>
