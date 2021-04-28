using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Pages
{
    public class StudentListBase : AbpComponentBase
    {
        [Inject]
        private IBlogAppService _blogAppService { get; set; }

        [Parameter]
        public List<BlogListDto> Students { get; set; }

        private IConfiguration Config { get; set; }
        [Inject]

        private NavigationManager NavigationManager { get; set; }

        public string Hostname { get; set; }

        

        protected override async Task OnInitializedAsync()
        {
            //if (!AbpSession.UserId.HasValue)
            //{
            //    NavigationManager.NavigateTo("/counter");
            //}
            //else{
            
            //}

            var blogs = await _blogAppService.GetPaged(new GetBlogsInput());

            Hostname = Config["HOSTNAME"];


            Students = (List<BlogListDto>)blogs.Items;
        }

        public string ToolTips { get; set; }

        




       
    }
}
