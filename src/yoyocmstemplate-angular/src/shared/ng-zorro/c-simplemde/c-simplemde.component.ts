import { HttpClient, HttpResponse } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, forwardRef, Input, OnChanges, OnInit, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { RequestHelper } from '@shared/helpers/RequestHelper';
import { AppConsts } from 'abpPro/AppConsts';
import { WechatMaterialType } from 'abpPro/AppEnums';
import { SimplemdeComponent } from 'ngx-simplemde';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'c-simplemde',
  templateUrl: './c-simplemde.component.html',
  styleUrls: ['./c-simplemde.component.less'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => CSimplemdeComponent),
    multi: true,
  }],
})

/**Markdown文本编辑器组件，实现了粘贴上传图片 */
export class CSimplemdeComponent implements OnInit, ControlValueAccessor {

  constructor(private rd: Renderer2,
              private ele: ElementRef,
              private http: HttpClient) { }
  @ViewChild('simplemde') simplemde: SimplemdeComponent;


  /**
 * 内部用的值
 */
  value: any;

  /**
 * 日期 Moment
 */
  @Input()
  ngModel: any;

  /**
 * 禁用
 */
  @Input()
  nzDisabled = false;


  /**
 * 时间发生变化的回调
 */
  @Output()
  ngModelChange = new EventEmitter<any>();

  mdOptions = {
    autosave: { enabled: true, uniqueId: 'MyUniqueID' },
  };

  /**
   * 图片上传后台处理地址
   */
  public uploadPictureUrl: string =
    AppConsts.remoteServiceBaseUrl + '/api/services/app/FileManagement/UploadFileToSysFiles';


  writeValue(obj: any): void {
    if (obj) {
      this.value = obj.toDate();
    } else {
      this.value = obj;
    }
  }
  registerOnChange(fn: any): void {
    this.ngModelChange.emit = fn;
  }
  registerOnTouched(fn: any): void {

  }
  setDisabledState?(isDisabled: boolean): void {
    this.nzDisabled = isDisabled;
  }


  onNgModelChangeChange(event: any) {
    if (event) {
      this.ngModelChange.emit(event);
    } else {
      this.ngModelChange.emit(undefined);
    }
  }

  ngOnInit(): void {

    this.addListenPaste();
  }

  /**添加图片粘贴的事件 */
  addListenPaste() {
    this.rd.listen(this.ele.nativeElement, 'paste', (e) => {
      const isImage = (/.jpg$|.jpeg$|.png$|.gif$/i);
      const file = e.clipboardData.files[0];
      if (file && isImage.test(file.type)) {
        const fileReader = new FileReader();  // 文件解读器
        const _ = this;

        fileReader.onloadend = function() {
          _.uploadFileToSysFiles(file); // 将读取后的base64    
        };
        fileReader.onerror = function(err) {
          console.log(err);
        };
        fileReader.readAsDataURL(file); // 读取一个文件返回base64地址
      }
    });
  }

  /**将图片链接插入到markdown中 */
  onInsertUrl(url) {
    debugger;

    const sim = (this.simplemde as any);

    const xy = sim.instance.codemirror.getCursor('start');
    const content = sim.instance.value();

    let lines = [' '];
    if (content) {
      lines = content.split('\n');
    }

    const tempstart = lines[xy.line].substring(0, xy.ch);

    const tempend = lines[xy.line].substring(xy.ch, lines[xy.line].length - 1);
    const newtemp = `${tempstart} ![](${url}) ${tempend}`;
    lines[xy.line] = newtemp;

    let contentValue = '';
    for (let index = 0; index < lines.length; index++) {
      const templine = lines[index];
      contentValue = `${contentValue}\n${templine}`;
    }

    sim.instance.value(contentValue);

  }

  /**上传图片 */
  uploadFileToSysFiles(file: any) {
    const formData = new FormData();
    formData.append('mediaFileType', `${WechatMaterialType.Image}`);
    formData.append('files[]', file);

    // 发送请求
    RequestHelper.createRequest(
      this.http,
      this.uploadPictureUrl,
      'POST',
      formData
    )
      .pipe(filter(e => e instanceof HttpResponse))
      .subscribe(
        (event: any) => {
          // this.message.success(this.l('Successfully'));
          // http://localhost:6298/sysfiles/2020-11-04/hidden/94cf3b67-eb13-67a0-0e3a-39f8a31160a1image.png
          const img = event.body.result[0].path;
          // "2020-11-04/hidden/c5413f4b-6937-fa2f-bc9d-39f8a315abaeimage.png"
          // http://localhost:6298/api/services/app/FileManagement/UploadFileToSysFiles"
          const imgulr = `${AppConsts.remoteServiceBaseUrl}/sysfiles/${img}`;
          this.onInsertUrl(imgulr);
        },
        err => {
          const result = err.error;
          console.log(result);
        }
      );
  }

}
