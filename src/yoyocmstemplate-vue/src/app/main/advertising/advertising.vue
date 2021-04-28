<template>
  <div>
    <div class="modal-header">
      <div class="modal-title">{{l('小广告~~~')}}</div>
    </div>

    <div>
      <h3>最新视频资料</h3>
      <a-row :gutter="8">
        <a href="https://dwz.cn/kQydQrTe" target="_blank">
          <a-col :span="16">
            <label>【推荐收费课程】52abp框架asp.net core & Angular全栈开发实战视频课程</label>
          </a-col>
        </a>
        <a-col :span="8">
          <a href="https://dwz.cn/kQydQrTe" target="_blank">立即学习</a>
        </a-col>
      </a-row>
      <a-row :gutter="8">
        <a-col :span="8">
          <label>ASP.NET Core框架ABP实战案例</label>
        </a-col>
        <a-col :span="10">
          <a
            href="https://study.163.com/course/introduction/1005208064.htm?share=2&shareId=400000000309007"
            target="_blank"
          >立即学习</a>
        </a-col>
      </a-row>
      <a-row :gutter="8">
        <a-col :span="8">
          <label>Asp.NET Core2.0 项目实战入门视频</label>
        </a-col>
        <a-col :span="8">
          <a
            href="https://study.163.com/course/introduction/1004710017.htm?share=2&shareId=400000000309007"
            target="_blank"
          >立即学习</a>
        </a-col>
      </a-row>
    </div>
    
<br>

<div>
  <h3>文档/博客</h3>
  <a-row :gutter="8">
    <a-col :span="8">
      <label>52ABP中文文档</label>
    </a-col>
    <a-col :span="10">
      <a href="https://www.52abp.com/ReadWiki/ABP%E6%A1%86%E6%9E%B6%E4%B8%AD%E6%96%87%E6%96%87%E6%A1%A3/Documents-3.7.2/1.1ABP%E6%80%BB%E4%BD%93%E4%BB%8B%E7%BB%8D-%E5%85%A5%E9%97%A8%E4%BB%8B%E7%BB%8D.md"
        target="_blank">立即学习</a>
    </a-col>
  </a-row>
  <a-row :gutter="8">
    <a-col :span="8">
      <label>52ABP博客</label>
    </a-col>
    <a-col :span="10">
      <a href="https://www.52abp.com/Blog/Index" target="_blank">立即学习</a>
    </a-col>
  </a-row>

  <a-row :gutter="8">
    <a-col :span="8">
      <label>开始学习《52abp框架asp.net core & Angular全栈开发实战视频课程》阅读须知
      </label>
    </a-col>
    <a-col :span="10">
      <a href="https://www.52abp.com/Blog/BlogDetails/5" target="_blank">立即学习</a>
    </a-col>
  </a-row>
</div>

<!-- 功能按钮 -->
<div class="modal-footer">
  <a-button type="button" @click="close()">
    {{l("Cancel")}}
  </a-button>
</div>
  </div>
</template>

<script>
import { ModalComponentBase } from "@/shared/component-base";
export default {
 mixins: [ModalComponentBase],

};
</script>

<style>
</style>