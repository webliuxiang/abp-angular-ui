using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.Dtos.Portal
{
    /// <summary>
    /// 获取博客的传入参数Dto
    /// </summary>
    public class GetPortalBlogsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {


        public Guid BlogId { get; set; }

        /// <summary>
        /// 博客详情
        /// </summary>
        public string ShortName { get; set; }
        public string TagName { get; set; } 

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    } }
