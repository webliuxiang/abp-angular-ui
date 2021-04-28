import {
    SettingScopes,
    TenantAvailabilityState,
    UploadMediaFileType,
    UserNotificationState
  } from '@/shared/service-proxies';

export class AppTenantAvailabilityState {
    public static Available: number = TenantAvailabilityState.Available;
    public static InActive: number = TenantAvailabilityState.InActive;
    public static NotFound: number = TenantAvailabilityState.NotFound;
  }

export class AppTimezoneScope {
    public static Application: number = SettingScopes.Application;
    public static Tenant: number = SettingScopes.Tenant;
    public static User: number = SettingScopes.User;
  }

  /**
   * 验证码类型
   */
export class AppCaptchaType {
    /**
     * 宿主租户注册
     */
    public static readonly HostTenantRegister = 1;
    /**
     * 宿主用户登陆
     */
    public static readonly HostUserLogin = 2;
    /**
     * 租户用户注册
     */
    public static readonly TenantUserRegister = 3;
    /**
     * 租户用户登陆
     */
    public static readonly TenantUserLogin = 4;
  }

export class AppEditionExpireAction {
    public static DeactiveTenant = 'DeactiveTenant';
    public static AssignToAnotherEdition = 'AssignToAnotherEdition';
  }

  /**
   * 用户通知状态
   */
export class AppUserNotificationState {
    /**
     * 未读
     */
    public static Unread: number = UserNotificationState.Unread;
    /**
     * 已读
     */
    public static Read: number = UserNotificationState.Read;
  }

  /**
   * 微信素材类型
   */
export class WechatMaterialType {
    /**
     * 图片
     */
    public static Image: number = UploadMediaFileType.Image;
    /**
     * 语音
     */
    public static Voice: number = UploadMediaFileType.Voice;
    /**
     * 视频
     */
    public static Video: number = UploadMediaFileType.Video;
    /**
     * 缩略图
     */
    public static Thumb: number = UploadMediaFileType.Thumb;
    public static News: number = UploadMediaFileType.News;
  }

