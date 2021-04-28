using System.Collections.Generic;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Web.Portal.Startup;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Com
{
    public class StudentListBase : AbpComponentBase
    {
        [Inject]
        private IBlogAppService BlogAppService { get; set; }

        [Parameter]
        public List<BlogListDto> Students { get; set; }
        [Inject]

        private IConfiguration Config { get; set; }
        [Inject]

        private NavigationManager NavigationManager { get; set; }

        public string Hostname { get; set; }



        protected override async Task OnInitializedAsync()
        {
            await Task.Yield();
            if (!AbpSession.UserId.HasValue)
            {
                //       NavigationManager.NavigateTo("/counter");
            }
            else
            {

            }

            // var blogs = await _blogAppService.GetPaged(new GetBlogsInput());

            Hostname = Config["HOSTNAME"];


            //   Students = (List<BlogListDto>)blogs.Items;
        }

        public string ToolTips { get; set; }







    }
}
