import {AppConsts} from '@/abpPro/AppConsts';
import abpService from '@/shared/abp/abp.service';
import {II18nService} from '@/shared/common';
import {BehaviorSubject, Observable} from 'rxjs';
import {share} from 'rxjs/operators';

class LocalizationService implements II18nService {

    // TODO: 这里目前没有实现修改语言事件
    private _change$: BehaviorSubject<string> = new BehaviorSubject<string>(null);

    get change(): Observable<string> {
        return this._change$.pipe(share());
    }

    get localizationSourceName(): string {
        return AppConsts.localization.defaultLocalizationSourceName;
    }

    constructor() {

    }

    public l(key: string, ...args: any[]): string {
        args.unshift(key);
        args.unshift(this.localizationSourceName);
        return this.ls.apply(this, args);
    }

    public ls(sourcename: string, key: string, ...args: any[]): string {
        let localizedText = abpService.abp.localization.localize(key, sourcename);

        if (!localizedText) {
            localizedText = key;
        }

        if (!args || !args.length) {
            return localizedText;
        }

        args.unshift(localizedText);
        return abp.utils.formatString.apply(this, args);
    }

    public ld(key: string, defaultValue: string) {
        const value = this.l(key);
        return value && value !== key ? value : defaultValue;
    }


    public fanyi(key: string): string {
        return this.l(key);
    }


}

const localizationService = new LocalizationService();
export default localizationService;
