using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Shared.Components.BlogCategory
{
    public class BlogRelatedCategory : AbpComponentBase
    {

        private readonly IPostAppService _postAppService;
        private readonly IBlogAppService _blogAppService;



        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();   
        }




    }
}
