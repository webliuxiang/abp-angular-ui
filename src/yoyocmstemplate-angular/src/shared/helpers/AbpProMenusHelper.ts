import { AbpProMenus } from 'abpPro/menus/AbpProMenus';
import { Menu } from '@delon/theme';

export class AbpProMenusHelper {
  /**
   * 获取顺序正确Menu
   * @param AbpProMenusItems 菜单数组
   */
  static getMenuItems(AbpProMenusItems: AbpProMenus[]): Menu[] {
    const menuItems = this.recursionItemsSort(AbpProMenusItems) as Menu[];
    return menuItems;
  }

  /**
   * 根据sort字段排序
   * @param AbpProMenusItems  菜单数组
   */
  static recursionItemsSort(AbpProMenusItems: AbpProMenus[]) {
    // 处理初始化值
    for (let index = 0; index < AbpProMenusItems.length; index++) {
      const element = AbpProMenusItems[index];
      if (element.sort == undefined || element.sort == null || element.sort == 0) {
        element.sort = 999999;
      }
    }
    // 排序
    const MenubySore = this.Orderby(AbpProMenusItems, i => i.sort);

    // 递归下一层级
    for (let index = 0; index < MenubySore.length; index++) {
      if ('children' in MenubySore[index] && MenubySore[index].children.length > 0) {
        MenubySore[index].children = this.recursionItemsSort(MenubySore[index].children);
      }
    }
    return MenubySore;
  }

  /**
   * 排序
   * @param arr 数组
   * @param selector 排序字段
   */
  static Orderby<Titem, Tvalue>(arr: Titem[], selector: (i: Titem) => Tvalue): Titem[] {
    if (arr.length <= 1) {
      return arr;
    }
    const pivotIndex = Math.floor(arr.length / 2);
    const pivot = arr.splice(pivotIndex, 1)[0];
    const left = [] as Titem[];
    const right = [] as Titem[];

    for (const i of arr) {
      if (selector(i) < selector(pivot)) {
        left.push(i);
      } else {
        right.push(i);
      }
    }
    return this.Orderby(left, selector).concat([pivot], this.Orderby(right, selector));
  }
}
