import { NameValueDto, PagedResultDtoOfNameValueDto } from '@shared/service-proxies/service-proxies';
import { Observable } from 'rxjs';

export interface ICommonLookupModalOptions {
    title?: string;
    isFilterEnabled?: boolean;
    dataSource: (skipCount: number, maxResultCount: number, filter: string, tenantId?: number) => Observable<PagedResultDtoOfNameValueDto>;
    canSelect?: (item: NameValueDto) => boolean | Observable<boolean>;
    loadOnStartup?: boolean;
    pageSize?: number;
}
