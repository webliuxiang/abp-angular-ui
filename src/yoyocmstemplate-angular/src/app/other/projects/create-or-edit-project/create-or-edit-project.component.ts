import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { ProjectEditDto, ProjectServiceProxy, CreateOrUpdateProjectInput } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { SFSchema, SFSelectWidgetSchema, SFComponent, SFUploadWidgetSchema } from '@delon/form';
import { type } from 'os';
import { AppConsts } from 'abpPro/AppConsts';


@Component({
  selector: 'create-or-edit-project',
  templateUrl: './create-or-edit-project.component.html'
})
export class CreateOrEditProjectComponent extends ModalComponentBase implements OnInit {

  @ViewChild('validateForm', { static: false }) private sf: SFComponent;

  constructor(
    injector: Injector,
    private _projectService: ProjectServiceProxy
  ) {
    super(injector);
  }


  id: any;
  entity: any = new ProjectEditDto();

  formarts: string[] = [
    'md'
  ];
  documentStoreTypes: string[] = [
    'Github',
    'Gitlab',
    // 'Local'
  ];
  uploadFileUrl: string;

  /**动态表单 */
  schema: SFSchema = {
    properties: {
      documentStoreType: {
        type: 'string',
        title: this.l('DocumentStoreType'),
        enum: this.documentStoreTypes,
        default: null,
        ui: {
          placeholder: this.l('DocumentStoreType'),
          widget: 'select',
          // mode: 'tags',
          size: 'default',
          change: model => {
            this.onDocumentTypeChange(model);
          }
        } as SFSelectWidgetSchema
      }
    }
  };

  ngOnInit(): void {
    this.init();
    this.getFileUploadUrl();
  }

  init(): void {
    this._projectService.getForEdit(this.id).subscribe(res => {
      this.entity = res.project;

      const propertyObj = JSON.parse(this.entity.extraProperties);
      if (this.entity.documentStoreType === 'Github') {
        this.entity.gitHubRootUrl = propertyObj.GitHubRootUrl;
        this.entity.gitHubUserAgent = propertyObj.GitHubUserAgent;
        this.entity.gitHubAccessToken = propertyObj.GitHubAccessToken;
      }

      if (this.entity.documentStoreType === 'Gitlab') {
        this.entity.gitLabBaseUrl = propertyObj.GitLabBaseUrl;
        this.entity.gitLabRootUrl = propertyObj.GitLabRootUrl;
        this.entity.gitLabUserAgent = propertyObj.GitLabUserAgent;
        this.entity.gitLabAccessToken = propertyObj.GitLabAccessToken;
      }

      this.onDocumentTypeChange(this.entity.documentStoreType);
    });
  }


  getFileUploadUrl() {
    this.uploadFileUrl = AppConsts.remoteServiceBaseUrl + '/api/services/app/SysFile/Create';
  }

  onDocumentTypeChange(type) {
    if (type === 'Github') {
      this.schema = {
        properties: {
          documentStoreType: {
            type: 'string',
            title: this.l('DocumentStoreType'),
            enum: this.documentStoreTypes,
            default: 'Github',
            ui: {
              placeholder: this.l('DocumentStoreType'),
              widget: 'select',
              size: 'default',
              change: model => {
                this.onDocumentTypeChange(model);
              }
            } as SFSelectWidgetSchema
          },


          imgUrl: {
            type: 'string',
            title: this.l('CoverImg'),
            ui: {
              placeholder: this.l('CoverImg'),
            },
          },
          name: {
            type: 'string',
            title: this.l('DisplayName'),
            ui: {
              placeholder: this.l('DisplayName'),
            }
          },
          shortName: {
            type: 'string',
            title: this.l('ShortName'),
            ui: {
              placeholder: this.l('ShortName')
            },
          },
          format: {
            type: 'string',
            title: this.l('Format'),
            enum: this.formarts,
            default: 'md',
            ui: {
              placeholder: this.l('Format'),
              widget: 'select',
              size: 'default',
            } as SFSelectWidgetSchema
          },
          defaultDocumentName: {
            type: 'string',
            title: this.l('DefaultDocumentName'),
            ui: {
              placeholder: this.l('DefaultDocumentName'),
            }
          },

          navigationDocumentName: {
            type: 'string',
            title: this.l('NavigationDocumentName'),
            ui: {
              placeholder: this.l('NavigationDocumentName'),
            }
          },

          gitHubRootUrl: {
            type: 'string',
            title: this.l('GithubUrl'),
            default: this.entity.gitHubRootUrl ? this.entity.gitHubRootUrl : undefined,
            ui: {
              placeholder: this.l('GithubUrl')
            },
          },
          gitHubUserAgent: {
            type: 'string',
            title: this.l('GithubUserAgent'),
            default: this.entity.gitHubUserAgent ? this.entity.gitHubUserAgent : 'Mozilla/5.0',
            ui: {
              placeholder: this.l('GithubUserAgent')
            },
          },
          gitHubAccessToken: {
            type: 'string',
            title: this.l('GitHubAccessToken'),
            default: this.entity.gitHubAccessToken ? this.entity.gitHubAccessToken : undefined,
            ui: {
              placeholder: this.l('GitHubAccessToken')
            },
          },

          mainWebsiteUrl: {
            type: 'string',
            title: this.l('MainWebsiteUrl'),
            default: '/Wiki',
            ui: {
              placeholder: this.l('MainWebsiteUrl'),
            }
          },

          latestVersionBranchName: {
            type: 'string',
            title: this.l('LatestVersionBranchName'),
            ui: {
              placeholder: this.l('LatestVersionBranchName'),
            }
          },

          minimumVersion: {
            type: 'string',
            title: this.l('MinimumVersion'),
            ui: {
              placeholder: this.l('MinimumVersion'),
            }
          },

          enabled: {
            type: 'boolean',
            title: this.l('IsEnable')
          },

          sort: {
            type: 'number',
            title: this.l('Sort'),
            ui: {
              placeholder: this.l('Sort'),
            }
          }
        },

        required: ['name', 'shortName', 'gitHubRootUrl', 'gitHubAccessToken'],
        ui: {
          errors: {
            required: this.l('ThisFieldIsRequired'),
            minLength: this.l('MinLength', 3),
          },
        },
      };
    }

    if (type === 'Gitlab') {
      this.schema = {
        properties: {
          documentStoreType: {
            type: 'string',
            title: this.l('DocumentStoreType'),
            enum: this.documentStoreTypes,
            default: 'Gitlab',
            ui: {
              placeholder: this.l('DocumentStoreType'),
              widget: 'select',
              size: 'default',
              change: model => {
                this.onDocumentTypeChange(model);
              }
            } as SFSelectWidgetSchema
          },


          imgUrl: {
            type: 'string',
            title: this.l('CoverImg'),
            ui: {
              placeholder: this.l('CoverImg'),
            },
          },
          name: {
            type: 'string',
            title: this.l('DisplayName'),
            ui: {
              placeholder: this.l('DisplayName'),
            }
          },
          shortName: {
            type: 'string',
            title: this.l('ShortName'),
            ui: {
              placeholder: this.l('ShortName')
            },
          },
          format: {
            type: 'string',
            title: this.l('Format'),
            enum: this.formarts,
            default: 'md',
            ui: {
              placeholder: this.l('Format'),
              widget: 'select',
              size: 'default',
            } as SFSelectWidgetSchema
          },
          defaultDocumentName: {
            type: 'string',
            title: this.l('DefaultDocumentName'),
            ui: {
              placeholder: this.l('DefaultDocumentName'),
            }
          },

          navigationDocumentName: {
            type: 'string',
            title: this.l('NavigationDocumentName'),
            ui: {
              placeholder: this.l('NavigationDocumentName'),
            }
          },

          gitLabBaseUrl: {
            type: 'string',
            title: this.l('GitLabBaseUrl'),
            default: this.entity.gitLabBaseUrl ? this.entity.gitLabBaseUrl : undefined,
            ui: {
              placeholder: this.l('GitLabBaseUrl')
            },
          },
          gitLabRootUrl: {
            type: 'string',
            title: this.l('GitLabRootUrl'),
            default: this.entity.gitLabRootUrl ? this.entity.gitLabRootUrl : undefined,
            ui: {
              placeholder: this.l('GitLabRootUrl')
            },
          },
          gitLabUserAgent: {
            type: 'string',
            title: this.l('GitLabUserAgent'),
            default: this.entity.gitLabUserAgent ? this.entity.gitLabUserAgent : 'Mozilla/5.0',
            ui: {
              placeholder: this.l('GitLabUserAgent')
            },
          },
          gitLabAccessToken: {
            type: 'string',
            title: this.l('GitLabAccessToken'),
            efault: this.entity.gitLabAccessToken ? this.entity.gitLabAccessToken : undefined,
            ui: {
              placeholder: this.l('GitLabAccessToken')
            },
          },

          mainWebsiteUrl: {
            type: 'string',
            title: this.l('MainWebsiteUrl'),
            default: '/Wiki',
            ui: {
              placeholder: this.l('MainWebsiteUrl'),
            }
          },

          latestVersionBranchName: {
            type: 'string',
            title: this.l('LatestVersionBranchName'),
            ui: {
              placeholder: this.l('LatestVersionBranchName'),
            }
          },

          minimumVersion: {
            type: 'string',
            title: this.l('MinimumVersion'),
            ui: {
              placeholder: this.l('MinimumVersion'),
            }
          },

          enabled: {
            type: 'boolean',
            title: this.l('IsEnable')
          },

          sort: {
            type: 'number',
            title: this.l('Sort'),
            ui: {
              placeholder: this.l('Sort'),
            }
          }
        },

        required: ['name', 'shortName', 'gitLabBaseUrl', 'gitLabRootUrl', 'gitLabAccessToken'],
        ui: {
          errors: {
            required: this.l('ThisFieldIsRequired'),
            minLength: this.l('MinLength', 3),
          },
        },
      };
    }

    // if (type === 'Local') {
    //   this.schema = {
    //     properties: {
    //       documentStoreType: {
    //         type: 'string',
    //         title: this.l('DocumentStoreType'),
    //         enum: this.documentStoreTypes,
    //         default: 'Local',
    //         ui: {
    //           placeholder: this.l('DocumentStoreType'),
    //           widget: 'select',
    //           size: 'default',
    //           change: model => {
    //             this.onDocumentTypeChange(model)
    //           }
    //         } as SFSelectWidgetSchema
    //       },


    //       imgUrl: {
    //         type: 'string',
    //         title: '上传头像',
    //         ui: {
    //           widget: 'upload',
    //           action: this.uploadFileUrl,
    //           resReName: 'resource_id',
    //           urlReName: 'url',
    //         } as SFUploadWidgetSchema,
    //       },
    //       name: {
    //         type: 'string',
    //         title: this.l('DisplayName'),
    //         ui: {
    //           placeholder: this.l('DisplayName'),
    //         }
    //       },
    //       shortName: {
    //         type: 'string',
    //         title: this.l('ShortName'),
    //         ui: {
    //           placeholder: this.l('ShortName')
    //         },
    //       },
    //       format: {
    //         type: 'string',
    //         title: this.l('Format'),
    //         enum: this.formarts,
    //         default: 'md',
    //         ui: {
    //           placeholder: this.l('Format'),
    //           widget: 'select',
    //           size: 'default',
    //         } as SFSelectWidgetSchema
    //       },
    //       defaultDocumentName: {
    //         type: 'string',
    //         title: this.l('DefaultDocumentName'),
    //         ui: {
    //           placeholder: this.l('DefaultDocumentName'),
    //         }
    //       },

    //       navigationDocumentName: {
    //         type: 'string',
    //         title: this.l('NavigationDocumentName'),
    //         ui: {
    //           placeholder: this.l('NavigationDocumentName'),
    //         }
    //       },

    //       extraProperties: {
    //         type: 'string',
    //         title: this.l('ExtraProperties'),
    //         ui: {
    //           placeholder: this.l('ExtraProperties '),
    //           widget: 'textarea',
    //           autosize: { minRows: 2, maxRows: 15 },
    //         },
    //       },

    //       mainWebsiteUrl: {
    //         type: 'string',
    //         title: this.l('MainWebsiteUrl'),
    //         ui: {
    //           placeholder: this.l('MainWebsiteUrl'),
    //         }
    //       },

    //       latestVersionBranchName: {
    //         type: 'string',
    //         title: this.l('latestVersionBranchName'),
    //         ui: {
    //           placeholder: this.l('latestVersionBranchName'),
    //         }
    //       },

    //       minimumVersion: {
    //         type: 'string',
    //         title: this.l('minimumVersion'),
    //         ui: {
    //           placeholder: this.l('minimumVersion'),
    //         }
    //       },

    //       enabled: {
    //         type: 'boolean',
    //         title: this.l('是否启用')
    //       },

    //       sort: {
    //         type: 'number',
    //         title: this.l('Sort'),
    //         ui: {
    //           placeholder: this.l('Sort'),
    //         }
    //       }
    //     },

    //     required: ['name'],
    //     ui: {
    //       errors: {
    //         required: this.l('ThisFieldIsRequired'),
    //         minLength: this.l('MinLength', 3),
    //       },
    //     },
    //   };
    // }
  }


  /**
   * 保存方法,提交form表单
   */
  submitForm(item: any): void {
    const input = new CreateOrUpdateProjectInput();
    input.project = new ProjectEditDto(item);
    if (input.project.documentStoreType === 'Github') {
      const extraProperties = {
        GitHubRootUrl: item.gitHubRootUrl,
        GitHubUserAgent: item.gitHubUserAgent,
        GitHubAccessToken: item.gitHubAccessToken
      };
      input.project.extraProperties = JSON.stringify(extraProperties);
    }
    if (input.project.documentStoreType === 'Gitlab') {
      const extraProperties = {
        GitLabBaseUrl: item.gitLabBaseUrl,
        GitLabRootUrl: item.gitLabRootUrl,
        GitLabUserAgent: item.gitLabUserAgent,
        GitLabAccessToken: item.gitLabAccessToken
      };
      input.project.extraProperties = JSON.stringify(extraProperties);
    }

    this.saving = true;
    this._projectService.createOrUpdate(input).pipe(finalize(() => (this.saving = false)))
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.success(true);
      });
  }

  /**拦截 */
  handleInputConfirm(): void {
    // this.schema.properties.tags.enum = this.tagList;
  }
}
