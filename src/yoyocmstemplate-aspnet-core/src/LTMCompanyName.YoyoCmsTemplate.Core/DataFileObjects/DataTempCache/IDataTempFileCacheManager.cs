using System;
using Abp.Dependency;

namespace LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache
{
    public interface IDataTempFileCacheManager : ITransientDependency
    {

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="content"></param>
        void SetFile(string token, byte[] content);




        /// <summary>
        /// 存储另外一种类型
        /// </summary>
        /// <param name="token"></param>
        /// <param name="content"></param>
        void SetFile(string token, DataFileObject content);

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        byte[] GetFile(string token);

        /// <summary>
        /// 获取DataFile文件
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        DataFileObject GetDataFileObject(string token);

    }
}
