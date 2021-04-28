import { Component, OnInit, Injector } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base';
import { WechatMediaServiceProxy } from '@shared/service-proxies/service-proxies';
import { UploadFile } from 'ng-zorro-antd/upload';
import { HttpClient, HttpRequest, HttpResponse } from '@angular/common/http';
import { RequestHelper } from '@shared/helpers/RequestHelper';

import { filter } from 'rxjs/operators';
import { WechatMaterialType } from 'abpPro/AppEnums';
import { AppConsts } from 'abpPro/AppConsts';

@Component({
    selector: 'app-create-image-material',
    templateUrl: './create-image-material.component.html',
    styles: []
})
export class CreateImageMaterialComponent extends ModalComponentBase
    implements OnInit {
    appId: string;
    wechatMediaService: WechatMediaServiceProxy;

    /**
     * 导入excel文件的api相对路径
     */
    apiUrl = '/api/services/app/WechatMedia/CreateOtherrMaterial';

    /**
     * 导入的类型
     */
    importType: WechatMaterialType = WechatMaterialType.Image;
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

    constructor(injector: Injector, private http: HttpClient) {
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

        // formdata拼接
        const formData = new FormData();
        formData.append('appId', this.appId);
        formData.append('mediaFileType', `${this.importType}`);
        this.fileList.forEach((file: any) => {
            formData.append('files[]', file);
        });

        // 发送请求
        RequestHelper.createRequest(
            this.http,
            AppConsts.remoteServiceBaseUrl + this.apiUrl,
            'POST',
            formData
        )
            .pipe(filter(e => e instanceof HttpResponse))
            .subscribe(
                (event: {}) => {
                    this.uploading = false;
                    this.message.success(this.l('Successfully'));
                    this.success();
                },
                err => {
                    this.uploading = false;
                    const result = err.error;
                    this.message.error(`导入失败！${result.error.message}`);
                }
            );
    }
}
