import { GetCurrentLoginInformationsOutput } from '@/shared';
import { IAbp } from '@/shared/abp/interfaces';
import { abpStore_NS, rootStore } from '@/shared/store';

class AbpService {
    public set(val: any) {
        rootStore.commit(`${abpStore_NS}/set`, val);
    }

    get abp(): IAbp {
        const storeAbp = rootStore.getters[`${abpStore_NS}/get`];
        if (storeAbp.localization) {
            return storeAbp;
        }
        return (abp as any);
    }

    public setLoginInfo(val: GetCurrentLoginInformationsOutput) {
        rootStore.commit(`${abpStore_NS}/setLoginInfo`, val);
    }

    get loginInfo(): GetCurrentLoginInformationsOutput {
        return rootStore.getters[`${abpStore_NS}/getLoginInfo`];
    }
}

const abpService = new AbpService();
export default abpService;
