import { Component, EventEmitter, Injector, Input, OnChanges, OnInit, Output, SimpleChange, SimpleChanges } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import {
  AliyunVodCategoryServiceProxy,
  AliyunVodUploadServiceProxy,
  RefreshUploadInputDto,
  VodCategoryEditDto,
  VodUploadDto,
} from '@shared/service-proxies/service-proxies';
import { NzMessageService } from 'ng-zorro-antd/message';
import { UploadFile } from 'ng-zorro-antd/upload';

@Component({
  selector: 'upload-video-resouce-panel',
  templateUrl: './upload-video-resouce-panel.component.html',
  styleUrls: ['./upload-video-resouce-panel.component.less'],
})
export class UploadVideoResoucePanelComponent extends AppComponentBase
  implements OnInit, OnChanges {

  private _uploading = false;
  private _dataList = [];


  set saving_self(val: boolean) {
    this.saving = !!val;
    this.savingChange.emit(this.saving);
  }

  /** 保存效果 */
  get saving_self(): boolean {
    return this.saving;
  }

  set uploading(val: boolean) {
    this._uploading = !!val;
    this.uploadingChange.emit(this._uploading);
  }

  /** 上传加载效果 */
  get uploading(): boolean {
    return this._uploading;
  }

  set dataList(val: any[]) {
    this._dataList = val;
    if (Array.isArray(val)) {
      this.dataListLengthChange.emit(this._dataList.length);
    } else {
      this.dataListLengthChange.emit(0);
    }
  }

  /** 数据列表 */
  get dataList(): any[] {
    return this._dataList;
  }

  /** 编辑视频id */
  editVodId: string = null;
  /** 上传dto */
  vodUploadDto: VodUploadDto = new VodUploadDto();
  /** 刷新上传输入dto */
  refreshUploadInputDto: RefreshUploadInputDto = new RefreshUploadInputDto();
  /** 请求参数 */
  requestPara = { title: '' };
  /** 选中的文件集合 */
  selectFileList: UploadFile[] = [];
  /** 上传列表 */
  uploaderList: any = [];
  /** 选择状态 */
  optionState = { start: true, pause: true, resume: true };
  /** 视频分类id-批量修改使用 */
  vodCateId: string = undefined;
  /** 视频分类集合 */
  vodCateList = [];

  /** 多个模式 */
  @Input() multiple = true;

  /** 调用保存函数 */
  @Input() callSave: boolean;

  /** 数据集合长度 */
  @Output() dataListLengthChange = new EventEmitter<number>();

  /** 保存状态修改 */
  @Output() savingChange = new EventEmitter<boolean>();

  /** 上传效果发生改变 */
  @Output() uploadingChange = new EventEmitter<boolean>();

  /** 上传成功回调视频信息 */
  @Output() uploadSuccessVideoInfo = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private vodSer: AliyunVodUploadServiceProxy,
    private aliyunVodeCategorySer: AliyunVodCategoryServiceProxy,
  ) {
    super(injector);
  }


  ngOnInit() {
    this.dataList = [];
    this.getVodCategoryDataList();
  }

  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
    if (changes.callSave && changes.callSave.currentValue) {
      this.handleUpload();
    }
  }

  /** 创建上传对象 */
  createUploader() {
    const self = this;
    const uploader = new AliyunUpload.Vod({
      timeout: 60000,
      //   分片大小默认1M，不能小于100K
      partSize: 1048576,
      // 并行上传分片个数，默认5
      parallel: 5,
      // 网络原因失败时，重新上传次数，默认为3
      retryCount: 3,
      // 网络原因失败时，重新上传间隔时间，默认为2秒
      retryDuration: 2,
      region: 'cn-shanghai',
      userId: '1303984639806000',
      // 添加文件成功
      addFileSuccess: (uploadInfo) => {
        console.log('addFileSuccess: ' + uploadInfo.file.name);
      },
      // 开始上传
      onUploadstarted: (uploadInfo) => {
        // 如果是 UploadAuth 上传方式, 需要调用 uploader.setUploadAuthAndAddress 方法
        // 如果是 UploadAuth 上传方式, 需要根据 uploadInfo.videoId是否有值，调用点播的不同接口获取uploadauth和uploadAddress
        // 如果 uploadInfo.videoId 有值，调用刷新视频上传凭证接口，否则调用创建视频上传凭证接口
        // 注意: 这里是测试 demo 所以直接调用了获取 UploadAuth 的测试接口, 用户在使用时需要判断 uploadInfo.videoId 存在与否从而调用 openApi
        // 如果 uploadInfo.videoId 存在, 调用 刷新视频上传凭证接口(https://help.aliyun.com/document_detail/55408.html)
        // 如果 uploadInfo.videoId 不存在,调用 获取视频上传地址和凭证接口(https://help.aliyun.com/document_detail/55407.html)
        if (!uploadInfo.videoId) {
          // You can use any AJAX library you like
          let videoId = null;

          self.dataList.forEach(item => {
            if (item.id === uploadInfo.file.uid) {
              self.vodUploadDto.title = item.name;
              self.vodUploadDto.fileName = item.name;
              self.vodUploadDto.cateId = item.cateId;
            }
          });

          self.vodSer.createUploadVideoRequest(self.vodUploadDto).subscribe(result => {
            if (result) {
              const uploadAuth = result.uploadAuth;
              const uploadAddress = result.uploadAddress;
              videoId = result.videoId;
              uploader.setUploadAuthAndAddress(uploadInfo, uploadAuth, uploadAddress, videoId);
              self.dataList.forEach(a => {
                if (a.id === uploadInfo.file.uid) {
                  a.state = 'uploading';
                  a.info = '上传中...';
                  a.videoId = videoId;
                }
              });
            } else {
              self.message.error('upload failed.');
            }
          });

          self.dataList.forEach(a => {
            if (a.id === uploadInfo.file.uid) {
              a.state = 'uploading';
              a.info = '上传中...';
              a.videoId = videoId;
            }
          });
        } else {
          // 如果videoId有值，根据videoId刷新上传凭证
          // https://help.aliyun.com/document_detail/55408.html?spm=a2c4g.11186623.6.630.BoYYcY
          self.refreshUploadInputDto.videoId = uploadInfo.videoId;
          self.vodSer.refreshUploadVideoRequest(self.refreshUploadInputDto).subscribe(ret => {
            if (ret) {
              const uploadAuth = ret.uploadAuth;
              const uploadAddress = ret.uploadAddress;
              const videoId = ret.videoId;
              uploader.setUploadAuthAndAddress(uploadInfo, uploadAuth, uploadAddress, videoId);
            } else {
              self.message.error('upload failed.');
            }
          });
        }
      },
      // 文件上传成功
      onUploadSucceed: (uploadInfo) => {
        self.dataList.forEach(o => {
          if (o.id === uploadInfo.file.uid) {
            o.state = 'success';
            o.videoId = uploadInfo.videoId;
          }
        });

        for (const item of self.dataList) {
          self.uploadSuccessVideoInfo.emit(item);
        }
      },
      // 文件上传失败
      onUploadFailed: (uploadInfo, code, message) => {
        self.dataList.forEach(a => {
          if (a.id === uploadInfo.file.uid) {
            a.state = 'error';
            a.info = message;
          }
        });
      },
      // 暂停取消文件上传
      onUploadCanceled: (uploadInfo, code, message) => {
        self.dataList.forEach(a => {
          if (a.id === uploadInfo.file.uid) {
            a.state = 'cancel';
          }
        });
      },
      // 文件上传进度，单位：字节, 可以在这个函数中拿到上传进度并显示在页面上
      onUploadProgress: (uploadInfo, totalSize, progress) => {
        console.log(
          'onUploadProgress:file:' +
          uploadInfo.file.name +
          ', fileSize:' +
          totalSize +
          ', percent:' +
          Math.ceil(progress * 100) +
          '%',
        );
        self.dataList.forEach(a => {
          if (a.id === uploadInfo.file.uid) {
            a.state = 'uploading';
            a.info = `进度：${Math.ceil(progress * 100)}%`;
          }
        });
      },
      // 上传凭证超时
      onUploadTokenExpired: (uploadInfo) => {
        // 上传大文件超时, 如果是上传方式一即根据 UploadAuth 上传时
        // 需要根据 uploadInfo.videoId 调用刷新视频上传凭证接口(https://help.aliyun.com/document_detail/55408.html)重新获取 UploadAuth
        // 然后调用 resumeUploadWithAuth 方法, 这里是测试接口, 所以我直接获取了 UploadAuth
        // $('#status').text('文件上传超时!')
        self.refreshUploadInputDto.videoId = uploadInfo.videoId;
        self.vodSer.refreshUploadVideoRequest(self.refreshUploadInputDto).subscribe(ret => {
          if (ret) {
            const uploadAuth = ret.uploadAuth;
            const uploadAddress = ret.uploadAddress;
            const videoId = ret.videoId;
            uploader.setUploadAuthAndAddress(uploadInfo, uploadAuth, uploadAddress, videoId);
          } else {
            self.message.error('upload failed.');
          }
        });
      },
      // 全部文件上传结束
      onUploadEnd: (uploadInfo) => {
        console.log('上传结束！');
      },
    });
    return uploader;
  }

  /** 上传前的逻辑处理 */
  beforeUpload = (file: UploadFile): boolean => {
    const isVideo = file.type === 'video/mp4' || file.type === 'video/avi' || file.type === 'video/rmvb';
    if (!isVideo) {
      this.message.error('只能上传.mp4、.avi、.rmvb视频类型的文件!');
    }
    //   console.log(file);
    this.selectFileList = this.selectFileList.concat(file);
    this.selectFileList.forEach(a => {
      const sizeValue = `${Math.ceil(a.size / 1024 / 1024)}M`;

      if (this.dataList.find(b => b.id === a.uid) == null) {
        if (this.dataList.find(b => b.name === a.name) === undefined) {
          this.dataList.push({
            id: a.uid,
            name: a.name,
            size: sizeValue,
            state: 'ready',
            info: '',
            videoId: '',
            cateId: this.vodCateId,
          });
        }
      }
    });
    this.dataList = this.dataList.map(a => {
      return a;
    });

    //  console.log(this.dataList);
    this.optionState.start = true;
    const userData = '{"Vod":{}}';
    this.uploaderList.forEach(item => {
      if (item.id === file.uid) {
        item.uploader.stopUpload();
      }
    });

    const uploader = this.createUploader();
    this.uploaderList.push({ id: file.uid, uploader: uploader });
    this.uploaderList.forEach(item => {
      if (item.id === file.uid) {
        item.uploader.addFile(file, null, null, null, userData);
      }
    });

    return false;
  }

  /** 移除上传文件 */
  removeVideo(data: any) {
    this.uploaderList.forEach(item => {
      if (item.id === data.id) {
        item.uploader.stopUpload();
      }
    });
    this.selectFileList.forEach((item, index) => {
      if (item.uid === data.id) {
        this.selectFileList.splice(index, 1);
      }
    });
    this.dataList.forEach((item, index) => {
      if (item.id === data.id) {
        this.dataList.splice(index, 1);
      }
    });
    this.dataList = this.dataList.map(a => {
      return a;
    });
  }

  /** 状态发生改变 */
  handleChange(info: { file: UploadFile }): void {
    const status = info.file.status;
    if (status !== 'uploading') {
      console.log(info.file);
    }
    if (status === 'done') {
      this.message.success(`${info.file.name} file uploaded successfully.`);
    } else if (status === 'error') {
      this.message.error(`${info.file.name} file upload failed.`);
    }
  }

  /** 恢复上传 */
  resumeUpload(data: any) {
    this.uploaderList.forEach(item => {
      if (item.id === data.id && item.uploader !== null) {
        item.uploader.startUpload();
        this.optionState.pause = true;
        this.optionState.resume = false;
      }
    });
    this.dataList.forEach(item => {
      if (item.id === data.id) {
        item.state = 'uploading';
      }
    });
  }

  /** 暂停上传 */
  pauseUpload(data: any) {
    this.uploaderList.forEach(item => {
      if (item.id === data.id && item.uploader !== null) {
        item.uploader.stopUpload();
        this.optionState.pause = false;
        this.optionState.resume = true;
      }
    });
    this.dataList.forEach(item => {
      if (item.id === data.id) {
        item.state = 'cancel';
      }
    });
  }

  /** 开始上传 */
  handleUpload(): void {
    for (const item of this.dataList) {
      if (!item.cateId || item.cateId === '') {
        this.message.warn(this.l('存在未选择分类的视频！'));
        return;
      }
    }
    // 然后调用 startUpload 方法, 开始上传
    for (const item of this.uploaderList) {
      if (this.dataList.find(b => b.id === item.id && b.state === 'ready') == null) {
        continue;
      }
      if (item.uploader !== null) {
        item.uploader.startUpload();
        this.optionState.start = false;
      }
    }
  }

  /** 分类功能 */
  getVodCategoryDataList(): void {
    this.aliyunVodeCategorySer
      .getAllVodCategories()
      .pipe()
      .subscribe(result => {
        this.vodCateList = this.processTree<VodCategoryEditDto, any>(result, 'children', (item, hasChildren) => {
          return {
            ...item,
            key: item.cateId,
            title: item.cateName,
            isLeft: !hasChildren,
          };
        });
      });
  }

  /** 开始编辑 */
  startEdit(id: string): void {
    this.editVodId = id;
  }

  /*** 停止编辑 */
  stopEdit(): void {
    this.editVodId = null;
  }

  /** 转换树 */
  processTree<TIn, TOut>(
    sourceData: TIn[],
    childrenMapKey: string,
    mapFunc: (source: TIn, hasChildren: boolean) => TOut,
  ): TOut[] {
    if (!Array.isArray(sourceData)) {
      return undefined;
    }

    const result: TOut[] = [];

    for (const item of sourceData) {

      const oldChildren = item[childrenMapKey];
      const hasChildren = Array.isArray(oldChildren) && oldChildren.length > 0;

      const newObj = mapFunc(item, hasChildren);

      if (hasChildren) {
        const newChildren = this.processTree(oldChildren, childrenMapKey, mapFunc);
        newObj[childrenMapKey] = newChildren;
      }

      result.push(newObj);
    }

    return result;
  }

  /** 批量视频分类id发生改变 */
  onVodCateIdChange(cateId: string) {
    for (const item of this.dataList) {
      item.cateId = cateId;
    }
  }

}
