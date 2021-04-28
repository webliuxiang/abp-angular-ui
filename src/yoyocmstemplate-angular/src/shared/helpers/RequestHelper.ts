import { HttpHeaders, HttpClient, HttpRequest, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

export class RequestHelper {

    /**
     * 创建请求头
     */
    public static createHttpHeaders(): HttpHeaders {
        let modifiedHeaders = new HttpHeaders();
        modifiedHeaders.append('Pragma', 'no-cache');
        modifiedHeaders.append('Cache-Control', 'no-cache');
        modifiedHeaders.append('Expires', 'Sat, 01 Jan 2000 00:00:00 GMT');
        modifiedHeaders = this.addXRequestedWithHeader(modifiedHeaders);
        modifiedHeaders = this.addAuthorizationHeaders(modifiedHeaders);
        modifiedHeaders = this.addAspNetCoreCultureHeader(modifiedHeaders);
        modifiedHeaders = this.addAcceptLanguageHeader(modifiedHeaders);
        modifiedHeaders = this.addTenantIdHeader(modifiedHeaders);
        return modifiedHeaders;
    }


    /**
     * 创建请求
     * @param httpClient
     * @param url 地址
     * @param methad 请求方式, GET\POST\PUT\DELETE,默认值get
     * @param formDate formDate
     * @param headers 请求头
     */
    public static createRequest(httpClient: HttpClient, url: string, methad?: string, formDate?: FormData, headers?: HttpHeaders): Observable<HttpEvent<{}>> {
        if (!headers) {
            headers = RequestHelper.createHttpHeaders();
        }
        if (!methad) {
            methad = 'GET';
        }

        const request = new HttpRequest(methad, url, formDate, {
            headers: headers
        });
        return httpClient.request(request);
    }

    public static addXRequestedWithHeader(headers: HttpHeaders): HttpHeaders {
        if (headers) {
            headers = headers.set('X-Requested-With', 'XMLHttpRequest');
        }

        return headers;
    }

    public static addAspNetCoreCultureHeader(headers: HttpHeaders): HttpHeaders {
        const cookieLangValue = abp.utils.getCookieValue('Abp.Localization.CultureName');
        if (cookieLangValue && headers && !headers.has('.AspNetCore.Culture')) {
            headers = headers.set('.AspNetCore.Culture', cookieLangValue);
        }

        return headers;
    }

    public static addAcceptLanguageHeader(headers: HttpHeaders): HttpHeaders {
        const cookieLangValue = abp.utils.getCookieValue('Abp.Localization.CultureName');
        if (cookieLangValue && headers && !headers.has('Accept-Language')) {
            headers = headers.set('Accept-Language', cookieLangValue);
        }

        return headers;
    }

    public static addTenantIdHeader(headers: HttpHeaders): HttpHeaders {
        const cookieTenantIdValue = abp.utils.getCookieValue('Abp.TenantId');
        if (cookieTenantIdValue && headers && !headers.has('Abp.TenantId')) {
            headers = headers.set('Abp.TenantId', cookieTenantIdValue);
        }

        return headers;
    }

    public static addAuthorizationHeaders(headers: HttpHeaders): HttpHeaders {
        let authorizationHeaders = headers ? headers.getAll('Authorization') : null;
        if (!authorizationHeaders) {
            authorizationHeaders = [];
        }

        if (!this.itemExists(authorizationHeaders, (item: string) => item.indexOf('Bearer ') === 0)) {
            const token = abp.auth.getToken();
            if (headers && token) {
                headers = headers.set('Authorization', 'Bearer ' + token);
            }
        }

        return headers;
    }

    private static itemExists<T>(items: T[], predicate: (item: T) => boolean): boolean {
        for (let i = 0; i < items.length; i++) {
            if (predicate(items[i])) {
                return true;
            }
        }

        return false;
    }

}
