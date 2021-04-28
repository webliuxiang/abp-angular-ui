import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
  CreateOrUpdateBlogInput,
  BlogEditDto,
  BlogServiceProxy,
  TagServiceProxy
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { SFSchema, SFSelectWidgetSchema, SFComponent, SFSchemaEnumType } from '@delon/form';

@Component({
  selector: 'create-or-edit-blog',
  templateUrl: './create-or-edit-blog.component.html',
  styleUrls: ['create-or-edit-blog.component.less'],
})
export class CreateOrEditBlogComponent extends ModalComponentBase implements OnInit {

  @ViewChild('validateForm', { static: false }) private sf: SFComponent;
  /**
   * 构造函数，在此处配置依赖注入
   */
  constructor(injector: Injector,
              private _blogService: BlogServiceProxy,
              private _tagService: TagServiceProxy, ) {
    super(injector);
  }
  /**
   * 编辑时DTO的id
   */
  id: any;

  entity: BlogEditDto = new BlogEditDto();

  userName: string;

  /**标签列表 */
  tagList: SFSchemaEnumType[] = [];

  /**用户列表 */
  userList: SFSchemaEnumType[] = [];

  /**动态表单 */
  schema: SFSchema = {
    properties: {
      name: {
        type: 'string',
        title: this.l('BlogName'),
        minLength: 3,
        ui: {
          placeholder: this.l('BlogNameInputDesc'),
        },
      },
      shortName: {
        type: 'string',
        title: this.l('BlogShortName'),
        maxLength: 10,
        ui: {
          placeholder: this.l('BlogShortNameInputDesc'),
        },
      },
      description: {
        type: 'string',
        title: this.l('BlogDescription'),
        ui: {
          placeholder: this.l('BlogDescriptionInputDesc'),
          widget: 'textarea',
          autosize: { minRows: 2, maxRows: 6 },
        },
      },
      blogUserId: {
        type: 'number',
        title: this.l('Users'),
        enum: this.userList,
        default: null,
        ui: {
          widget: 'select',
        } as SFSelectWidgetSchema,
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
    required: ['name', 'blogUserId'],
    ui: {
      errors: {
        required: this.l('ThisFieldIsRequired'),
        minLength: this.l('MinLength', 3),
      },
    },
  };

  ngOnInit(): void {
    this.init();

    this.getUser();

    this.getTagsOfBlog();
  }

  /**或者博客的选择用户 */
  getUser() {
    this._blogService.getUserList(this.userName)
      .pipe(
        finalize(() => {
          this.saving = false;
        }),
      )
      .subscribe((ret) => {
        if (ret.length > 0) {
          const statusProperty = this.sf.getProperty('/blogUserId')!;
          ret.forEach(item => {
            this.userList.push({ label: item.userName, value: item.id });
          });
          statusProperty.schema.enum = this.userList;
          // statusProperty.schema.default = this.abpSession.userId.toString();
          this.sf.refreshSchema();
        }

      });
  }

  /**
   * 初始化方法
   */
  init(): void {
    this._blogService.getForEdit(this.id).subscribe(result => {
      this.entity = result.blog;
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
    const input = new CreateOrUpdateBlogInput();
    input.blog = new BlogEditDto(item);
    input.blog.tagIds = item.tags;
    if (input.blog.blogUserId === 0) {
      input.blog.blogUserId = this.abpSession.userId;
    }

    // console.log(input);
    this.saving = true;

    this._blogService
      .createOrUpdate(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
      });
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
