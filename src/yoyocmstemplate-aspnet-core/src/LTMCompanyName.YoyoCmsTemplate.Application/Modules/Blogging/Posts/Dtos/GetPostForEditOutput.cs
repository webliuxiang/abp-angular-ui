
using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos
{
    /// <summary>
    /// 读取可编辑文章的Dto
    /// </summary>
    public class GetPostForEditOutput
    {



        public PostEditDto Post { get; set; }

        public List<KeyValuePair<string, string>> PostTypeTypeEnum { get; set; }
        //// custom codes		


        //// custom codes end
    }
}
