import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import {
  CreateOrUpdateCommentInput,
  CommentEditDto,
  CommentServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-comment',
  templateUrl: './create-or-edit-comment.component.html',
  styleUrls: ['create-or-edit-comment.component.less'],
})
export class CreateOrEditCommentComponent extends ModalComponentBase implements OnInit {
  /**
   * 编辑时DTO的id
   */
  id: any;

  postId: any;

  repliedCommentId: any;

  entity: CommentEditDto = new CommentEditDto();

  /**
   * 构造函数，在此处配置依赖注入
   */
  constructor(injector: Injector, private _commentService: CommentServiceProxy) {
    super(injector);
  }

  ngOnInit(): void {
    this.init();
  }

  /**
   * 初始化方法
   */
  init(): void {
    if (this.id) {
      this._commentService.getForEdit(this.id).subscribe(result => {
        this.entity = result.comment;
      });
    }

    if (this.repliedCommentId) {
      this.entity.repliedCommentId = this.repliedCommentId;
    }
  }

  /**
   * 保存方法,提交form表单
   */
  submitForm(): void {
    const input = new CreateOrUpdateCommentInput();
    input.comment = this.entity;
    if (this.postId) {
      input.comment.postId = this.postId;
    }
    if (this.repliedCommentId) {
      input.comment.repliedCommentId = this.repliedCommentId;
    }
    this.saving = true;

    this._commentService
      .createOrUpdate(input)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
      });
  }
}
