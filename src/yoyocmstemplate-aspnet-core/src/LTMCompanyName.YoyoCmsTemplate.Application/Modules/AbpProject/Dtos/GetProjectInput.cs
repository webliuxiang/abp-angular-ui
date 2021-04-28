using System;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.AbpProject.Dtos
{
    public class GetProjectInput
    {
        /// <summary>
        ///  项目模板路径信息
        /// </summary>
        public string TemplateProjectPath { get; set; }

        /// <summary>
        ///     输出目录
        /// </summary>
        public string OutPutDir { get; set; }

        /// <summary>
        ///     公司名称
        /// </summary>
        public string ComplanyName { get; set; }

        /// <summary>
        ///     项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目类型 
        /// </summary>
        public ProjectTypeEnum ProjectType { get; set; }
 
       

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }


    }
}
