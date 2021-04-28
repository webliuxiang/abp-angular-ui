import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module';
import { YoYoHttpInterceptor } from './YoYoHttpInterceptor';

import * as ApiServiceProxies from '@shared/service-proxies/service-proxies';
import * as ApiYSLogServiceProxies from '@shared/service-proxies/api-service-proxies';

@NgModule({
  providers: [
    ApiServiceProxies.AccountServiceProxy,
    ApiServiceProxies.AuditLogServiceProxy,
    ApiServiceProxies.LanguageServiceProxy,
    ApiServiceProxies.NotificationServiceProxy,
    ApiServiceProxies.OrganizationUnitServiceProxy,
    ApiServiceProxies.PermissionServiceProxy,
    ApiServiceProxies.ProfileServiceProxy,
    ApiServiceProxies.RoleServiceProxy,
    ApiServiceProxies.SessionServiceProxy,
    ApiServiceProxies.TenantServiceProxy,
    // ApiServiceProxies.TokenAuthServiceProxy,
    ApiServiceProxies.HostSettingsServiceProxy,
    ApiServiceProxies.HostCachingServiceProxy,
    ApiServiceProxies.WebSiteLogServiceProxy,
    ApiServiceProxies.TenantSettingsServiceProxy,
    ApiServiceProxies.TenantRegistrationServiceProxy,
    ApiServiceProxies.EditionServiceProxy,
    ApiServiceProxies.TimingServiceProxy,
    ApiServiceProxies.CommonLookupServiceProxy,
    ApiServiceProxies.AddressLinkageServiceProxy,
    // ============= wechat ==================
    ApiServiceProxies.WechatAppConfigServiceProxy,
    ApiServiceProxies.WechatMenuAppSeviceServiceProxy,
    ApiServiceProxies.WechatMediaServiceProxy,
    { provide: HTTP_INTERCEPTORS, useClass: YoYoHttpInterceptor, multi: true },
    ApiServiceProxies.PurchaseServiceProxy,
    // ============= 博客模块 ==================
    ApiServiceProxies.BlogServiceProxy,
    ApiServiceProxies.PostServiceProxy,
    ApiServiceProxies.CommentServiceProxy,
    ApiServiceProxies.TagServiceProxy,
    ApiServiceProxies.SysFileServiceProxy,
    // ============= 网站基础模块 ==================
    ApiServiceProxies.BlogrollTypeServiceProxy,
    ApiServiceProxies.BlogrollServiceProxy,
    ApiServiceProxies.WebSiteNoticeServiceProxy,
    ApiServiceProxies.BannerImgServiceProxy,
    ApiServiceProxies.ChatServiceProxy,
    ApiServiceProxies.FriendshipServiceProxy,

    // ============= market ==================
    ApiServiceProxies.ProductServiceProxy,
    ApiServiceProxies.ProductSecretKeyServiceProxy,
    ApiServiceProxies.OrderServiceProxy,
    // Mooc模块
    ApiServiceProxies.TencentOrderInfoServiceProxy, // 腾讯课堂订单信息
    ApiServiceProxies.NeteaseOrderInfoServiceProxy, // 网易云订单信息
    ApiServiceProxies.AliyunVodCategoryServiceProxy, // 阿里云视频分类
    ApiServiceProxies.AliyunVodUploadServiceProxy, // 阿里云视频上传
    ApiServiceProxies.VideoResourceServiceProxy, // 视频资源信息
    ApiServiceProxies.CourseServiceProxy, // 课程
    ApiServiceProxies.CourseSectionServiceProxy, // 章节
    ApiServiceProxies.CourseClassHourServiceProxy, // 课时
    ApiServiceProxies.CourseCategoryServiceProxy, // 课程分类
    // 数据下载日志
    ApiServiceProxies.DownloadLogServiceProxy,
    // 动态页面配置
    ApiServiceProxies.DynamicPageServiceProxy,
    ApiServiceProxies.ProjectServiceProxy,
    // DropDown
    ApiServiceProxies.DropdownListServiceProxy,
    // ##############################################
    ApiYSLogServiceProxies.FileServiceProxy,

    ApiYSLogServiceProxies.AccountServiceProxy,
    ApiYSLogServiceProxies.LanguageServiceProxy,
    ApiYSLogServiceProxies.AuditLogServiceProxy,
    ApiYSLogServiceProxies.CommonLookupServiceProxy,
    ApiYSLogServiceProxies.GetUserLoginAttemptsServiceProxy,
    ApiYSLogServiceProxies.HostCachingServiceProxy,
    ApiYSLogServiceProxies.SessionServiceProxy,
    ApiYSLogServiceProxies.TimeZoneServiceProxy,
    ApiYSLogServiceProxies.WebSiteLogServiceProxy,

    ApiYSLogServiceProxies.SecuritySettingsServiceProxy,
    ApiYSLogServiceProxies.NetworkSettingsServiceProxy,
    ApiYSLogServiceProxies.SmtpSettingsServiceProxy,
    ApiYSLogServiceProxies.SystemAlertSettingsServiceProxy,

    ApiYSLogServiceProxies.YSLogTenantServiceProxy,
    ApiYSLogServiceProxies.YSLogUserServiceProxy,
    ApiYSLogServiceProxies.YSLogUserManagementServiceProxy,
    ApiYSLogServiceProxies.TokenAuthServiceProxy,
    ApiYSLogServiceProxies.VerificationServiceProxy,
    ApiYSLogServiceProxies.DynamicFormServiceProxy,

    ApiYSLogServiceProxies.YSLogDataSetObjectServiceProxy,
    ApiYSLogServiceProxies.InternalYSLogDataSetObjectServiceProxy,
    ApiYSLogServiceProxies.ExternalYSLogDataSetObjectServiceProxy,

    ApiYSLogServiceProxies.YSLogDataAnalyzeObjectServiceProxy,
    ApiYSLogServiceProxies.YSLogVisualizeObjectServiceProxy,
    ApiYSLogServiceProxies.YSLogSearchObjectServiceProxy,

    ApiYSLogServiceProxies.YSLogDataClientObjectServiceProxy,
    ApiYSLogServiceProxies.YSLogDataCollectorObjectServiceProxy,
    ApiYSLogServiceProxies.YSLogDataFormatterObjectServiceProxy,
    ApiYSLogServiceProxies.YSLogDataEngineObjectServiceProxy,

    ApiYSLogServiceProxies.YSLogDataAlertObjectServiceProxy,

    ApiYSLogServiceProxies.YSLogDataBackupObjectServiceProxy,
    ApiYSLogServiceProxies.YSLogDataRestoreObjectServiceProxy,
    ApiYSLogServiceProxies.YSLogMountPointObjectServiceProxy,
  ],
})
export class ServiceProxyModule {
}
