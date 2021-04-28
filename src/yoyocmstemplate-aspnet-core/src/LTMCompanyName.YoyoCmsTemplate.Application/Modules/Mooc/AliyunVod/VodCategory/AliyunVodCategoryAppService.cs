using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Aliyun.Acs.vod.Model.V20170321;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodCategory.Dtos;
using Yoyo.Abp.Vod;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodCategory
{
    public class AliyunVodCategoryAppService : YoyoCmsTemplateAppServiceBase, IAliyunVodCategoryAppService
    {
        private readonly AliyunVodManager _aliyunVodManager;

        public AliyunVodCategoryAppService(AliyunVodManager aliyunVodManager)
        {
            _aliyunVodManager = aliyunVodManager;
        }

        public async Task<VodCategoryEditDto> CreateVodCategory(VodCategoryEditDto input)
        {
            await Task.Yield();


            var dto = new AddCategoryRequest { ParentId = input.ParentId, CateName = input.CateName };


            var res = _aliyunVodManager.CreateMediaCategory(dto);

            input.CateId = res.Category.CateId;
            input.CateName = res.Category.CateName;
            input.Level = res.Category.Level;
            input.ParentId = res.Category.ParentId;


            return input;
        }

        public async Task UpdateVodCategory(VodCategoryEditDto input)
        {
            await Task.Yield();

            var updto = new UpdateCategoryRequest { CateId = input.CateId, CateName = input.CateName };


            var res = _aliyunVodManager.UpdateMediaCategory(updto);
        }

        public async Task DeleteVodCategory(EntityDto<long> input)
        {
            await Task.Yield();


            _aliyunVodManager.DeleteCategories(new DeleteCategoryRequest()
            {
                CateId = input.Id
            });


        }

        /// <summary>
        /// 获取vod分类的信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<VodCategoryEditDto>> GetAllVodCategories()
        {
            await Task.Yield();

            var request = new GetCategoriesRequest()
            {
                CateId = -1,
                PageNo = 1,
                PageSize = 100,
                SortBy = "CreationTime:Asc"
            };
            var res = _aliyunVodManager.GetMediaCategories(request);

            var dtos = new List<VodCategoryEditDto>();

            if (res.SubCategories.Count <= 0)
            {
                return dtos;
            }

            foreach (var item in res.SubCategories)
            {
                var dto = new VodCategoryEditDto
                {
                    RequestId = res.RequestId,
                    CateId = item.CateId,
                    CateName = item.CateName,
                    Level = item.Level,
                    Type = item.Type,
                    ParentId = item.ParentId
                };

                if (item.SubTotal.HasValue && item.SubTotal > 0)
                {
                    dto.Children = new List<VodCategoryEditDto>();

                    var subres = _aliyunVodManager.GetMediaCategories(new GetCategoriesRequest { CateId = item.CateId });

                    foreach (var subitem in subres.SubCategories)
                    {
                        var subdto = new VodCategoryEditDto
                        {
                            CateId = subitem.CateId,
                            CateName = subitem.CateName,
                            Level = subitem.Level,
                            ParentId = subitem.ParentId,
                            Type = subitem.Type
                        };
                        dto.Children.Add(subdto);


                        if (!subitem.SubTotal.HasValue || !(subitem.SubTotal > 0))
                        {
                            continue;
                        }

                        subdto.Children = new List<VodCategoryEditDto>();
                        var thirdRes = _aliyunVodManager.GetMediaCategories(new GetCategoriesRequest
                        {
                            CateId = subitem.CateId
                        });


                        foreach (var thirdResSubCategory in thirdRes.SubCategories)
                        {
                            var thirddto = new VodCategoryEditDto
                            {
                                CateId = thirdResSubCategory.CateId,
                                CateName = thirdResSubCategory.CateName,
                                Level = thirdResSubCategory.Level,
                                ParentId = thirdResSubCategory.ParentId,
                                Type = thirdResSubCategory.Type
                            };
                            subdto.Children.Add(thirddto);
                        }
                    }
                }

                dtos.Add(dto);
            }


            return dtos;
        }
    }
}
