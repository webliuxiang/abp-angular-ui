import { Menu, MenuIcon } from '@delon/theme';

export class AbpProMenus implements Menu {
  [key: string]: any;
  /** 推荐使用i18n ，没的情况下会显示text中的内容。非多语言的情况使用。 */
  text?: string;
  /** I18n key of menu item, can be choose one of  `text` or `i18n` (Support HTML) */
  i18n?: string;
  /** Whether to display the group name, default: `true` */
  group?: boolean;
  /** Routing for the menu item, can be choose one of `link` or `externalLink` */
  link?: string;
  /** External link for the menu item, can be choose one of `link` or `externalLink` */
  externalLink?: string;
  /** Specifies `externalLink` where to display the linked URL */
  target?: '_blank' | '_self' | '_parent' | '_top';
  /** Icon for the menu item, only valid for the first level menu */
  icon?: string | MenuIcon | null;
  /** Badget for the menu item when `group` is `true` */
  badge?: number;
  /** Whether to display a red dot instead of `badge` value */
  badgeDot?: boolean;
  /** Badge [color](https://ng.ant.design/components/badge/en#nz-badge) */
  badgeStatus?: string;
  /** Whether disable for the menu item */
  disabled?: boolean;
  /** Whether hidden for the menu item */
  hide?: boolean;
  /** Whether hide in breadcrumbs, which are valid when the `page-header` component automatically generates breadcrumbs */
  hideInBreadcrumb?: boolean;
  /** ACL configuration, it's equivalent to `ACLService.can(roleOrAbility: ACLCanType)` parameter value */
  acl?: any;
  /** Whether shortcut menu item */
  shortcut?: boolean;
  /** Wheter shortcut menu root node */
  shortcutRoot?: boolean;
  /** Whether to allow reuse, need to cooperate with the `reuse-tab` component */
  reuse?: boolean;
  /** Whether to expand, when `checkStrictly` is valid in `sidebar-nav` component */
  open?: boolean;
  /** Unique identifier of the menu item, can be used in `getItem`,` setItem` to update a menu */
  key?: string;
  /** Children menu of menu item */
  children?: AbpProMenus[];

  // 1、sort字段
  // 功能：
  // - 默认值为0，处于当前菜单栏最底部。
  // - 起始值以1开头。
  sort = 0;
}
