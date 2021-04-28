using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Models.Home
{
    public class PostsAndPopularTags
    {
        public IReadOnlyList<PostDetailsDto> PostDetailsDtos { get; set; }

        public string BlogShortName { get; set; }
        public List<TagListDto> TagList { get; set; }
    }
}
