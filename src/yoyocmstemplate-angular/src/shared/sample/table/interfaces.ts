import { ColumnItemDto } from '@shared/service-proxies/service-proxies';

/** 当action触发 */
export interface ISampleTableAction {
  /** action名称 */
  name: string;
  /** 被点击的数据 */
  record: any;
}
