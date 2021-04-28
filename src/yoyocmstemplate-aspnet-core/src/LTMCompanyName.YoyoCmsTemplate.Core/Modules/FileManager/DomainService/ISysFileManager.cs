using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.FileManager;
using Microsoft.AspNetCore.Http;

namespace LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.DomainService
{
    public interface ISysFileManager : IDomainService
    {
        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns> </returns>
        IQueryable<SysFile> QuerySysFiles();

        /// <summary>
        /// 返回性能更好的IQueryable类型，但不包含EF Core跟踪标记
        /// </summary>
        /// <returns> </returns>

        IQueryable<SysFile> QuerySysFilesAsNoTracking();

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        Task<SysFile> FindByIdAsync(Guid id);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <returns> </returns>
        Task<bool> IsExistAsync(Guid id);

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="entity"> 文件实体 </param>
        /// <returns> </returns>
        Task<SysFile> CreateAsync(SysFile entity);

        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="entity"> 文件实体 </param>
        /// <returns> </returns>
        Task UpdateAsync(SysFile entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"> Id的集合 </param>
        /// <returns> </returns>
        Task BatchDelete(List<Guid> input);

        //// custom codes
        /// <summary>
        /// 验证文件是否符合规则
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task ValidateSysfileAsync(SysFile entity);

        Task<List<SysFile>> FindChildrenAsync(Guid? parentId, bool recursive = false);

        /// <summary>
        /// 移动文件与文件夹
        /// </summary>
        /// <param name="id"> </param>
        /// <param name="ParentId"> </param>
        /// <returns> </returns>
        Task MoveAsync(Guid id, Guid? ParentId);


        Task<SysFile> ProcessUploadedFileAsync(IFormFile file, bool isHidden = false);


        //// custom codes end
    }
}
