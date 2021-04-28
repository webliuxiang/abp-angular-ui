<template>
  <ul class="account-language-switch-list">
    <li :key="index" v-for="(language,key,index) of languages">
      <a
        v-if="language.name != currentLanguage.name"
        href="javascript:void();"
        v-bind:title="language.displayName"
        @click="changeLanguage(language.name)"
      >
        <i v-bind:class="language.icon"></i>
      </a>
    </li>
  </ul>
</template>

<script>
import * as _ from "lodash";
import { abpService } from "@/shared/abp";

export default {
  name: "account-languages",
  computed: {
    languages: vm => {
      return _.filter(
        abpService.abp.localization.languages,
        l => !l.isDisabled
      );
    },
    currentLanguage: vm => {
      return abpService.abp.localization.currentLanguage;
    }
  },
  methods: {
    changeLanguage(languageName) {
      abpService.abp.utils.setCookieValue(
        "Abp.Localization.CultureName",
        languageName,
        new Date(new Date().getTime() + 5 * 365 * 86400000), // 5 year
        abp.appPath
      );

      window.location.reload();
    }
  }
};
</script>

<style lang="less" scoped>
@import "./account-languages.less";
</style>