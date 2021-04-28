import { STColumnBadge, STColumnTag } from '@delon/abc';

export class SampleTableConsts {
  private static inited: boolean;

  static ynBadge: STColumnBadge = {};
  static ynTag: STColumnTag = {};

  static init(l: (key) => string) {
    if (SampleTableConsts.inited) {
      return;
    }
    const ynValue: any = {
      true: { text: l('label.yes'), color: 'success' },
      false: { text: l('label.no'), color: 'error' },
      '': { text: '', color: 'default' },
    };

    SampleTableConsts.ynBadge = ynValue;
    SampleTableConsts.ynTag = ynValue;
  }
}
