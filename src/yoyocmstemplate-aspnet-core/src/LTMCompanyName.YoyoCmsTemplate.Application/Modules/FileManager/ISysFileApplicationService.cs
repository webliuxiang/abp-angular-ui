using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.FileManager;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.Dtos;
using Microsoft.AspNetCore.Http;

namespace LTMCompanyName.YoyoCmsTemplate.SystemBaseManage
{
    /// <summary>
    /// 文件应用层服务的接口方法
    ///</summary>
    public interface ISysFileAppService : IApplicationService
    {
        /// <summary>
		/// 获取文件的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SysFileListDto>> GetPaged(GetSysFilesInput input);

        /// <summary>
        /// 通过指定id获取文件ListDto信息
        /// </summary>
        Task<SysFileListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 返回实体文件的EditDto
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task<GetSysFileForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="parentId"> </param>
        /// <returns> </returns>
        Task Create(Guid? parentId);

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task<SysFileListDto> CreateDirectory(SysFileEditDto input);

        /// <summary>
        /// 重命名
        /// </summary>
        /// <returns> </returns>
        Task ReFileName(SysFileEditDto input);

        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task Update(SysFileEditDto input);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除文件
        /// </summary>
        Task BatchDelete(List<Guid> input);

        //// custom codes
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SysFileListDto> CopyFile(SysFileEditDto input);

        Task<List<SysFileListDto>> GetDirectories();

        Task<SysFileListDto> MoveAsync(MoveSysFilesInput input);

         
        //// custom codes end
    }
}
