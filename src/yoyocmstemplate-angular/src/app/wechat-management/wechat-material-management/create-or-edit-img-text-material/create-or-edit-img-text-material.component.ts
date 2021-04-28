import { AppComponentBase } from '@shared/component-base/app-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReuseTabService } from '@delon/abc';

@Component({
  selector: 'app-create-or-edit-img-text-material',
  templateUrl: './create-or-edit-img-text-material.component.html',
  styles: []
})
export class CreateOrEditImgTextMaterialComponent extends AppComponentBase implements OnInit {

  constructor(
    injector: Injector,
    private activatedRoute: ActivatedRoute,
    private reuseTabService: ReuseTabService,
  ) {
    super(injector);
  }

  ngOnInit() {
  }

}
