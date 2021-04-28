using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.PortalBlog.Dtos
{
    public class CreatePostsByGitlab
    {
        public string RepoName { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }


        public string FullPath => FilePath + FileName;



    }
}
