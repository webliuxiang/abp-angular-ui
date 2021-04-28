<template>
  <div class="example">
    <!-- 加载中... -->
    <a-spin v-if="isLoading"/>
    <div class="modal-header">
      <div class="modal-title">{{l('LoginAttempts')}}</div>
    </div>
    <a-card v-for="item in userLoginAttempts">
      <a-row :gutter="8">
        <a-col :span="3">
          <label>{{l('LoginState')}}</label>
        </a-col>

        <a-col :span="10">
          <a-badge v-if="item.result == 'Success'" :text="l('Success')" status="success" />
          <a-badge v-if="item.result != 'Success'" :text="l('Failed')" status="error" />
        </a-col>
      </a-row>
        <a-row :gutter="8">
          <a-col :span="3">
            <label>{{l('IpAddress')}}:</label>
          </a-col>
          <a-col :span="10">{{item.clientIpAddress}}</a-col>
        </a-row>

        <a-row :gutter="8">
          <a-col :span="3">
            <label>{{l('Client')}}:</label>
          </a-col>
          <a-col :span="10">{{item.clientName}}</a-col>
        </a-row>
        <a-row :gutter="8">
          <a-col :span="3">
            <label>{{l('Browser')}}:</label>
          </a-col>
          <a-col :span="10">{{item.browserInfo}}</a-col>
        </a-row>
        <a-row :gutter="8">
          <a-col :span="3">
            <label>{{l('Time')}}:</label>
          </a-col>
          <a-col :span="10">{{getLoginAttemptTime(item)}}</a-col>
        </a-row>
    </a-card>
  </div>
</template>

<script>
import { ModalComponentBase } from "@/shared/component-base";
import moment from "moment";

import {
  UserLoginServiceProxy,
  ProfileServiceProxy,
  UserLoginAttemptDto
} from "../../../shared/service-proxies";

export default {
  name: "login-attempts-modal",
  mixins: [ModalComponentBase],
  data() {
    return {
      isLoading:true,
      userLoginAttempts: [],
      userLoginService: null
    };
  },
  created() {
    this.userLoginService = new UserLoginServiceProxy(this.$apiUrl, this.$api);
    this.userLoginService.getRecentUserLoginAttempts().then(result => {
      this.isLoading=false;
      this.userLoginAttempts = result.items;
    });
  },
  methods: {
    getLoginAttemptTime(userLoginAttempt) {
      return (
        moment(userLoginAttempt.creationTime).fromNow() +
        " (" +
        moment(userLoginAttempt.creationTime).format("YYYY-MM-DD hh:mm:ss") +
        ")"
      );
    }
  }
};
</script>

<style scoped>

</style>
