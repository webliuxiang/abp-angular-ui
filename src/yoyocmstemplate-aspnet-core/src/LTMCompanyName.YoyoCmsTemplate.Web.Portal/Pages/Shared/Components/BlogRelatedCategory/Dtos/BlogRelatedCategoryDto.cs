using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages.Shared.Components.BlogRelatedCategory.Dtos
{
    public class BlogRelatedCategoryDto
    {
        public List<PostDetailsDto> postsDto;
        public string BlogShortName { get; set; }

    }
}
