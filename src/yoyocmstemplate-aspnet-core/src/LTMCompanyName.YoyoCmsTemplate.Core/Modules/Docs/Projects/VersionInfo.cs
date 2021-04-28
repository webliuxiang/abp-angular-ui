using System;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Projects
{
    [Serializable] //需要序列化，因为该对象存储在缓存中
    public class VersionInfo
    {
        /// <summary>
        /// 对应的是TagName
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 对应的是 ReleaseName
        /// </summary>
        public string Name { get; set; }
    }
}
