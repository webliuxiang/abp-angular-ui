import userNotificationState = abp.notifications.userNotificationState;
import IUserNotification = abp.notifications.IUserNotification;
import severity = abp.notifications.severity;
import IFeature = abp.features.IFeature;
import levels = abp.log.levels;
import INameValue = abp.INameValue;
import ITimeZoneInfo = abp.timing.ITimeZoneInfo;
import IClockProvider = abp.timing.IClockProvider;
import ILanguageInfo = abp.localization.ILanguageInfo;
import ILocalizationSource = abp.localization.ILocalizationSource;
import IAbpSession = abp.IAbpSession;
import IMenu = abp.nav.IMenu;

export interface AbpMultiTenancy {
  isEnabled: boolean;

  tenantIdCookieName: string;

  setTenantIdCookie(tenantId?: number): void;

  getTenantIdCookie(): number;
}

export interface AbpLocalization {
  languages: ILanguageInfo[];

  currentLanguage: ILanguageInfo;

  sources: ILocalizationSource[];

  defaultSourceName: string;

  values: { [key: string]: string };

  abpWeb: (key: string) => string;

  localize(key: string, sourceName: string): string;

  getSource(sourceName: string): (key: string) => string;

  isCurrentCulture(name: string): boolean;
}


export interface IAbpAuth {

  /**
   * 所有权限名称
   */
  allPermissions: { [name: string]: boolean };

  /**
   * 当前登录用户拥有的权限
   */
  grantedPermissions: { [name: string]: boolean };

  /**
   * 是否拥有此权限
   * @param permissionName
   */
  isGranted(permissionName: string): boolean;

  /**
   * 是否拥有数组中某个权限
   * @param args
   */
  isAnyGranted(...args: string[]): boolean;

  /**
   * 是否拥有数组中所有权限
   * @param args
   */
  areAllGranted(...args: string[]): boolean;

  /**
   * token 存储到cookie中的 key 值
   */
  tokenCookieName: string;

  /**
   * 设置token
   * @param authToken token 值
   * @param expireDate 过期时间
   */
  setToken(authToken: string, expireDate?: Date): void;


  /**
   * 获取token
   */
  getToken(): string;

  /**
   * 清除token
   */
  clearToken(): void;
}

export interface IAbpFeature {
  allFeatures: { [name: string]: IFeature };

  get(name: string): IFeature;

  getValue(name: string): string;

  isEnabled(name: string): boolean;
}

export interface IAbpSetting {
  values: { [name: string]: string };

  get(name: string): string;

  getBoolean(name: string): boolean;

  getInt(name: string): number;
}

export interface IAbpNav {
  menus: { [name: string]: IMenu };
}

export interface IAbpNotification {
  messageFormatters: any;

  getUserNotificationStateAsString(userNotificationState: userNotificationState): string;

  getUiNotifyFuncBySeverity(severity: severity): (message: string, title?: string, options?: any) => void;

  getFormattedMessageFromUserNotification(userNotification: IUserNotification): string;

  showUiNotifyForUserNotification(userNotification: IUserNotification, options?: any): void;
}


export interface IAbpLog {
  level: levels;

  log(logObject?: any, logLevel?: levels): void;

  debug(logObject?: any): void;

  info(logObject?: any): void;

  warn(logObject?: any): void;

  error(logObject?: any): void;

  fatal(logObject?: any): void;
}

export interface IAbpNotify {
  info(message: string, title?: string, options?: any): void;

  success(message: string, title?: string, options?: any): void;

  warn(message: string, title?: string, options?: any): void;

  error(message: string, title?: string, options?: any): void;
}

export interface IAbpMessage {

  info(message: string, title?: string, isHtml?: boolean): any;

  success(message: string, title?: string, isHtml?: boolean): any;

  warn(message: string, title?: string, isHtml?: boolean): any;

  error(message: string, title?: string, isHtml?: boolean): any;

  confirm(message: string, callback?: (result: boolean) => void): any;

  confirm(message: string, title?: string, callback?: (result: boolean) => void, isHtml?: boolean): any;
}

export interface IAbpUI {
  block(elm?: any): void;

  unblock(elm?: any): void;

  setBusy(elm?: any, optionsOrPromise?: any): void;

  clearBusy(elm?: any): void;
}

export interface IAbpEvent {

  on(eventName: string, callback: (...args: any[]) => void): void;

  off(eventName: string, callback: (...args: any[]) => void): void;

  trigger(eventName: string, ...args: any[]): void;
}

export interface IAbpUtils {
  createNamespace(root: any, ns: string): any;

  replaceAll(str: string, search: string, replacement: any): string;

  formatString(str: string, ...args: any[]): string;

  toPascalCase(str: string): string;

  toCamelCase(str: string): string;

  truncateString(str: string, maxLength: number): string;

  truncateStringWithPostfix(str: string, maxLength: number, postfix?: string): string;

  isFunction(obj: any): boolean;

  buildQueryString(parameterInfos: INameValue[], includeQuestionMark?: boolean): string;

  /**
   * Sets a cookie value for given key.
   * This is a simple implementation created to be used by ABP.
   * Please use a complete cookie library if you need.
   * @param {string} key
   * @param {string} value
   * @param {Date} expireDate (optional). If not specified the cookie will expire at the end of session.
   * @param {string} path (optional)
   */
  setCookieValue(key: string, value: string, expireDate?: Date, path?: string): void;

  /**
   * Gets a cookie with given key.
   * This is a simple implementation created to be used by ABP.
   * Please use a complete cookie library if you need.
   * @param {string} key
   * @returns {string} Cookie value or null
   */
  getCookieValue(key: string): string;

  /**
   * Deletes cookie for given key.
   * This is a simple implementation created to be used by ABP.
   * Please use a complete cookie library if you need.
   * @param {string} key
   * @param {string} path (optional)
   */
  deleteCookie(key: string, path?: string): void;
}


export interface IAbpTiming {
  convertToUserTimezone(date: Date): Date;

  timeZoneInfo: ITimeZoneInfo;
}

export interface IAbpClock {
  provider: IClockProvider;

  now(): Date;

  normalize(date: Date): Date;
}

export interface IAbpAntiForgery {
  tokenCookieName: string;

  tokenHeaderName: string;

  getToken(): string;
}

export interface IAbpSecurity {
  antiForgery: IAbpAntiForgery;
}


export interface IAbp {
  appPath: string;

  pageLoadTime: Date;

  toAbsAppPath(path: string): string;

  multiTenancy: AbpMultiTenancy;

  session: IAbpSession;

  localization: AbpLocalization;

  auth: IAbpAuth;

  features: IAbpFeature;

  setting: IAbpSetting;

  nav: IAbpNav;

  notifications: IAbpNotification;

  log: IAbpLog;

  notify: IAbpNotify;

  message: IAbpMessage;

  ui: IAbpUI;

  event: IAbpEvent;

  utils: IAbpUtils;

  timing: IAbpTiming;

  clock: IAbpClock;

  security: IAbpSecurity;

  custom: any;

}
