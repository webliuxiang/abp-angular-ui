import { AppComponentBase } from '@shared/component-base/app-component-base';
import { Component, OnInit, Injector, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReuseTabService } from '@delon/abc';
import { HttpClient, HttpRequest, HttpResponse } from '@angular/common/http';
import { UploadFile } from 'ng-zorro-antd/upload';
import { RequestHelper } from '@shared/helpers/RequestHelper';
import { AppConsts } from 'abpPro/AppConsts';
import { filter } from 'rxjs/operators';
import { WechatMediaServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'app-wechat-material-img-text-view',
    templateUrl: './wechat-material-img-text-view.component.html',
    styles: []
})
export class WechatMaterialImgTextViewComponent extends AppComponentBase
    implements OnInit {
    /**
     * 导入excel文件的api相对路径
     */
    excelImportUrl = '/api/services/app/WechatMedia/Test';

    /**
     * 导入的类型
     */
    importType: number;
    /**
     * 文件支持的格式
     */
    accept = '*';
    /**
     * 上传状态
     */
    uploading = false;
    /**
     * 文件集合
     */
    fileList: UploadFile[] = [];

    @Input()
    wechatMediaService: WechatMediaServiceProxy;

    constructor(
        injector: Injector,
        private activatedRoute: ActivatedRoute,
        private reuseTabService: ReuseTabService,
        private http: HttpClient
    ) {
        super(injector);
    }

    ngOnInit() { }
    beforeUpload = (file: UploadFile): boolean => {
        this.fileList.push(file);
        return false;
    }

    save(): void {
        if (this.fileList.length < 1) {
            this.message.warn('请选择文件！');
            return;
        }

        this.uploading = true;

        const input: any = {
            data: 'hello',
            file: this.fileList[0]
        };
        // formdata拼接
        const formData = new FormData();
        formData.append('input', 'input');
        this.fileList.forEach((file: any) => {
            formData.append('files[]', file);
        });

        // 创建HTTP请求头
        const modifiedHeaders = RequestHelper.createHttpHeaders();

        const request = new HttpRequest(
            'POST',
            AppConsts.remoteServiceBaseUrl + this.excelImportUrl,
            formData,
            {
                headers: modifiedHeaders
            }
        );

        // 发送请求
        this.http
            .request(request)
            .pipe(filter(e => e instanceof HttpResponse))
            .subscribe(
                (event: {}) => {
                    this.uploading = false;
                    this.message.success('导入成功');
                },
                err => {
                    const result = err.error;

                    this.uploading = false;
                    this.message.error(`导入失败！${result.error.message}`);
                }
            );
    }
}
