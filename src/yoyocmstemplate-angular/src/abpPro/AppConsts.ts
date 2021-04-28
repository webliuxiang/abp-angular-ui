import { state } from '@angular/animations';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService, NzNotificationDataOptions } from 'ng-zorro-antd/notification';
import { NzMessageService } from 'ng-zorro-antd/message';
import { parse } from 'date-fns';

export class AppConsts {
  static remoteServiceBaseUrl: string;
  static portalBaseUrl: string;
  static appBaseUrl: string;
  static uploadApiUrl = '/api/File/Upload';
  static maxProfilPictureMb = 1; // 个人头像上传最大MB
  /** 多租户请求头名称 */
  static tenantIdCookieName = 'Tenant';
  /**
   * 后端本地化和前端angular本地化映射
   */
  static localeMappings: any;
  /**
   * 后端本地化和ng-zorro本地化映射
   */
  static ngZorroLocaleMappings: any;
  /**
   * 后端本地化和ng-alian本地化映射
   */
  static ngAlainLocaleMappings: any;
  /**
   * 后端本地化和moment.js本地化映射
   */
  static momentLocaleMappings: any;

  static readonly userManagement = {
    defaultAdminUserName: 'admin',
  };

  static readonly localization = {
    defaultLocalizationSourceName: 'WebManagement',
  };

  static readonly authorization = {
    encrptedAuthTokenName: 'enc_auth_token',
  };

  /**
   * 数据表格设置
   */
  // tslint:disable-next-line:member-ordering
  static readonly grid = {
    /**
     * 每页显示条目数
     */
    defaultPageSize: 10,
    /**
     * 每页显示条目数下拉框值
     */
    defaultPageSizes: [10, 20, 30, 50, 80, 100],
  };

  /**
   * top bar通知组件中获取通知数量
   */
  static readonly notificationCount = 5;

  /** 配置key */
  static readonly settings = {
    theme: {
      layout: 'App.Theme.Layout',
    },
    /** 启用用户注册验证码 */
    useCaptchaOnUserRegistration: 'App.UseCaptchaOnUserRegistration',
    /** 用户注册验证码类型 0:纯数字 1:纯字母 2:数字+字母  3:纯汉字 */
    captchaOnUserRegistrationType: 'App.CaptchaOnUserRegistrationType',
    /** 用户注册验证码长度 */
    captchaOnUserRegistrationLength: 'App.CaptchaOnUserRegistrationLength',
    /** 启用用户登陆验证码 */
    useCaptchaOnUserLogin: 'App.UseCaptchaOnUserLogin',
    /** 用户登陆验证码类型 0:纯数字 1:纯字母 2:数字+字母  3:纯汉字 */
    captchaOnUserLoginType: 'App.CaptchaOnUserLoginType',
    /** 用户登陆验证码长度 */
    captchaOnUserLoginLength: 'App.CaptchaOnUserLoginLength',
    host: {
      /** 启用租户注册验证码 */
      useCaptchaOnTenantRegistration: 'App.Host.UseCaptchaOnTenantRegistration',
      /** 租户注册验证码类型 0:纯数字 1:纯字母 2:数字+字母  3:纯汉字 */
      captchaOnTenantRegistrationType: 'App.Host.CaptchaOnTenantRegistrationType',
      /** 租户注册验证码验证码长度 */
      captchaOnTenantRegistrationLength: 'App.Host.CaptchaOnTenantRegistrationLength',
    },
    tenant: {},
  };

  /** 配置常量 */
  static readonly settingValues = {
    theme: {
      layout: {
        default: 'Default',
        top_navigation_menu: 'TopNavigationMenu',
      },
    },
  };
}
