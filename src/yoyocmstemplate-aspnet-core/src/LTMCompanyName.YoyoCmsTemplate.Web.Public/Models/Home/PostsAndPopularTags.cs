using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Models.Home
{
    public class PostsAndPopularTags
    {
        public IReadOnlyList<PostDetailsDto> posts { get; set; }

        public string BlogShortName { get; set; }
        public List<TagListDto> tags { get; set; }
    }
}
