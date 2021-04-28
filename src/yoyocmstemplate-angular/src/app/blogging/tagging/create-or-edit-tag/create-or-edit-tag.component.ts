import {
  Component,
  OnInit,
  Injector,
  Input,
  ViewChild,
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
} from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
  CreateOrUpdateTagInput,
  TagEditDto,
  TagServiceProxy,
  PostServiceProxy
} from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { SFSchema, SFSelectWidgetSchema, SFSchemaEnumType, SFComponent } from '@delon/form';
import { BlogServiceProxy } from '../../../../shared/service-proxies/service-proxies';

@Component({
  selector: 'create-or-edit-tag',
  templateUrl: './create-or-edit-tag.component.html',
  styleUrls: ['create-or-edit-tag.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateOrEditTagComponent extends ModalComponentBase implements OnInit {
  /**
   * 构造函数，在此处配置依赖注入
   */
  constructor(
    injector: Injector,
    private _tagService: TagServiceProxy,
    private _cdr: ChangeDetectorRef,
    private _blogService: BlogServiceProxy,
    private _postService: PostServiceProxy,
  ) {
    super(injector);
  }
  /**
   * 编辑时DTO的id
   */
  id: any;
  /** 博客列表 */
  blogsList: SFSchemaEnumType[] = [];

  /**文章列表 */
  postsList: SFSchemaEnumType[] = [];

  entity: TagEditDto = new TagEditDto();
  /**动态表单 */
  @ViewChild('validateForm') sf: SFComponent;
  /**动态表单 */
  schema: SFSchema = {
    properties: {
      name: {
        type: 'string',
        title: this.l('TagName'),
        minLength: 3,
        ui: {
          placeholder: this.l('TagNameInputDesc'),
        },
      },
      blogId: {
        type: 'string',
        title: this.l('Blog'),
        //  enum: [],
        //  enum: ['Angular', 'Node', 'HTML5', 'Less', '.net core'],
        enum: this.blogsList,
        default: [],
        ui: {
          widget: 'select',
          // mode: 'tags',
          size: 'default',

          allowClear: true,
          // serverSearch: true,

          // asyncData: () => of(this.getTagsOfBlog()).pipe(delay(1200)) as SFSelectWidgetSchema,
          change: model => {
            this.handleInputConfirm(model);
          },
        } as SFSelectWidgetSchema,
      },
      postId: {
        type: 'string',
        title: this.l('Post'),
        //  enum: [],
        //  enum: ['Angular', 'Node', 'HTML5', 'Less', '.net core'],
        enum: this.postsList,
        default: [],
        ui: {
          widget: 'select',
          // mode: 'tags',
          size: 'default',

          allowClear: true,
          // serverSearch: true,

          // asyncData: () => of(this.getTagsOfBlog()).pipe(delay(1200)) as SFSelectWidgetSchema,
          change: model => {
            this.handleInputConfirm(model);
          },
        } as SFSelectWidgetSchema,
      },
      description: {
        type: 'string',
        title: this.l('TagDescription'),
        ui: {
          placeholder: this.l('TagDescriptionInputDesc'),
          widget: 'textarea',
          autosize: { minRows: 2, maxRows: 6 },
        },
      },
    },
    required: ['name'],
    ui: {
      errors: {
        required: this.l('ThisFieldIsRequired'),
        minLength: this.l('MinLength', 3),
      },
    },
  };

  ngOnInit(): void {
    this.init();
    this.getBlogList();
    this.getPostList();
  }
  handleInputConfirm(e) {
    //  console.log(e);
    // throw new Error('Method not implemented.');
  }
  /**
   * 初始化方法
   */
  init(): void {
    this._tagService.getForEdit(this.id).subscribe(result => {
      this.entity = result.tag;
    });
  }

  /**获取博客列表 */
  getBlogList(): void {
    this._blogService
      .getBlogs()
      .pipe()
      .subscribe(result => {
        result.map(item => {
          this.blogsList.push({
            label: item.name,
            value: item.id,
          });
        });
        //  this.schema.properties.tags.enum = this.postTagList;
        // this.schema.properties.blogId.default = result.;
        this.sf.refreshSchema();
        // this.sf.validator();

        console.log(this.blogsList);
      });
  }


  getPostList(): void {
    this._postService
      .getPosts()
      .pipe()
      .subscribe(result => {
        result.map(item => {
          this.postsList.push({
            label: item.title,
            value: item.id,
          });
        });

        this.sf.refreshSchema();
      });

  }

  /**
   * 保存方法,提交form表单
   */
  submitForm(item: any): void {
    const input = new CreateOrUpdateTagInput();
    input.tag = new TagEditDto(item);
    console.log(item);

    if (input.tag.blogId.length <= 0) {
      input.tag.blogId = undefined;
    }
    if (input.tag.postId.length <= 0) {
      input.tag.postId = undefined;
    }
    // return;
    this.saving = true;

    this._tagService
      .createOrUpdate(input)
      .pipe(finalize(() => (this.saving = false)))

      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
        this._cdr.detectChanges();
      });
  }
}
