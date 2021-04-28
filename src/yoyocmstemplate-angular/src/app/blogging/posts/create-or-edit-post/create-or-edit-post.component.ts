import { Component, OnInit, Injector, ViewChild, Renderer2, ElementRef } from '@angular/core';
import { CreateOrUpdatePostInput, PostEditDto, PostServiceProxy, TagServiceProxy } from '@shared/service-proxies/service-proxies';
import { filter, finalize, window } from 'rxjs/operators';
import { AppComponentBase } from '@shared/component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { ReuseTabService } from '@delon/abc';
import { SFSchema, SFRadioWidgetSchema, SFSchemaEnumType, SFSelectWidgetSchema, SFComponent } from '@delon/form';
import { AppConsts } from 'abpPro/AppConsts';
import { RequestHelper } from '@shared/helpers/RequestHelper';
import { WechatMaterialType } from 'abpPro/AppEnums';
import { HttpClient, HttpResponse } from '@angular/common/http';
@Component({
  selector: 'create-or-edit-post',
  templateUrl: './create-or-edit-post.component.html',
  styleUrls: ['create-or-edit-post.component.less'],
})
export class CreateOrEditPostComponent extends AppComponentBase implements OnInit {
  /**
   * 构造函数，在此处配置依赖注入
   */
  constructor(
    injector: Injector,
    private _postService: PostServiceProxy,
    private _tagService: TagServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _reuseTabService: ReuseTabService,
    private rd: Renderer2,
    private ele: ElementRef,
    private http: HttpClient

  ) {
    super(injector);
  }
  /**
   * 文章的Id
   */
  id: any;
  /** 博客的Id */
  blogId: string;
  /**博客的Dto */
  loading = true;
  /**文章的Dto */
  entity: PostEditDto = new PostEditDto();

  fromCdnUrl = false;

  /**文章类型枚举 */
  postTypeTypeEnum: SFSchemaEnumType[] = [];

  /**文章标签列表 */
  tagList: SFSchemaEnumType[] = [];

  /**动态表单 */
  @ViewChild('validateForm') sf: SFComponent;
  schema: SFSchema = {
    properties: {
      title: {
        type: 'string',
        title: this.l('PostTitle'),
        minLength: 3,
        ui: {
          placeholder: this.l('PostTitleInputDesc'),
        },
      },
      url: {
        type: 'string',
        title: this.l('PostUrl'),
        minLength: 3,
        ui: {
          placeholder: this.l('PostUrlInputDesc'),
        },
      },
      // coverImage: {
      //   type: 'string',

      //   title: this.l('PostCoverImage'),
      //   maxLength: 10,
      //   ui: {
      //     placeholder: this.l('PostCoverImageInputDesc'),
      //   },
      // },

      coverImage: {
        type: 'string',
        title: this.l('PostCoverImage'),
        ui: {
          widget: 'img',
        },
      },
      content: {
        type: 'string',
        title: this.l('PostContent'),
        ui: {
          // placeholder: this.l('PostContentInputDesc'),
          widget: 'md',
          // widget: 'md'  //Markdown
          //    autosize: { minRows: 2, maxRows: 6 },
        },
      },


      postType: {
        type: 'string',
        title: this.l('PostType'),
        enum: this.postTypeTypeEnum,
        ui: {
          widget: 'radio',
          styleType: 'button',
          buttonStyle: 'solid',
        } as SFRadioWidgetSchema,
      },
      tags: {
        type: 'string',
        title: this.l('Tags'),
        //  enum: [],
        //  enum: ['Angular', 'Node', 'HTML5', 'Less', '.net core'],
        enum: this.tagList,
        default: null,
        ui: {
          widget: 'select',
          mode: 'tags',
          size: 'default',
          serverSearch: true,

          // asyncData: () => of(this.getTagsOfBlog()).pipe(delay(1200)) as SFSelectWidgetSchema,
          change: model => {
            this.handleInputConfirm();
          },
        } as SFSelectWidgetSchema,
      },
    },
    required: ['title', 'url', 'content', 'coverImage'],
    ui: {
      errors: {
        required: this.l('ThisFieldIsRequired'),
        minLength: this.l('MinLength', 3),
      },
    },
  };

  /**
   * 图片上传后台处理地址
   */
  public uploadPictureUrl: string =
    AppConsts.remoteServiceBaseUrl + '/api/services/app/FileManagement/UploadFileToSysFiles';

  ngOnInit(): void {
    this.init();

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
    const xy = (this.sf.getProperty('/content').widget as any).cd._lView[50][0].instance.codemirror.getCursor('start');
    const content = this.sf.getProperty('/content').value;

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

    const sfContent = this.sf.getProperty('/content') as any;
    sfContent.setValue(contentValue);

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
          this.message.error(`导入失败！${result.error.message}`);
        }
      );
  }


  /**
   * 初始化方法
   */
  init(): void {
    this.id = this._activatedRoute.snapshot.params.id;
    this.blogId = this._activatedRoute.snapshot.params.blogId;
    //  console.log( this.blogId);

    this._reuseTabService.title = this.l('LoadingWithThreeDot');

    this._postService.getForEdit(this.id).subscribe(result => {
      this.entity = result.post;
      if (this.entity.title == null) {
        this._reuseTabService.title = this.l('Create');
      } else {
        this._reuseTabService.title = this.l('Edit') + ' - ' + this.entity.title;
      }

      result.postTypeTypeEnum.map(item => {
        this.postTypeTypeEnum.push({
          label: item.key,
          value: item.value,
        });
      });
      //  this.postTypeTypeEnum = this.ConvertToSFSchemaEnumType(result.postTypeTypeEnum);

      // setTimeout(() => {
      //   this.postTypeTypeEnum = this.ConvertToSFSchemaEnumType(result.postTypeTypeEnum);
      //   this.sf.refreshSchema();
      // }, 1);

      if (result.post.id != null) {
        this.blogId = result.post.blogId;
        //   console.log('blogId=' + this.blogId);
      }

      this.getTagsOfBlog();

      this.loading = false;
    });
  }

  /**获取标签列表 */
  getTagsOfBlog(): void {
    this._tagService
      .getAll()
      .pipe()
      .subscribe(result => {
        result.map(item => {
          this.tagList.push({
            label: item.name,
            value: item.id,
          });
        });
        //  this.schema.properties.tags.enum = this.postTagList;
        this.schema.properties.tags.default = this.entity.tagIds;
        this.sf.refreshSchema();
        // this.sf.validator();

        //    console.log(this.postTagList);
      });
  }
  /**
   * 保存方法,提交form表单
   */
  submitForm(item: any): void {
    console.log(item);

    const input = new CreateOrUpdatePostInput();
    input.post = new PostEditDto(item);
    input.post.blogId = this.blogId;
    // input.post.coverImage = item.img.path;
    input.post.tagIds = item.tags;
    //    console.log(this.entity);
    this.saving = true;
    console.log(typeof input.post.coverImage);

    if (typeof item.coverImage === 'object' && item.coverImage) {
      input.post.coverImage = item.coverImage.path;
    }

    console.log(input.post);

    this._postService
      .createOrUpdate(input)
      .pipe(finalize(() => (this.saving = false)))

      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.close();
      });
  }

  close() {
    // 去触发文章列表刷新的事件
    abp.event.trigger('abp.posts.refresh');

    this._router.navigate(['app/blogging/posts']);
  }

  /**拦截 */
  handleInputConfirm(): void {
    // console.log(e);
    // console.log(this.postTagList);

    // this.postTagList.push({
    //   label: 'dd' + Math.random(),
    //   value: 'dddd' + Math.random(),
    // });
    this.schema.properties.tags.enum = this.tagList;

    //  this.sf.refreshSchema();
  }
}
