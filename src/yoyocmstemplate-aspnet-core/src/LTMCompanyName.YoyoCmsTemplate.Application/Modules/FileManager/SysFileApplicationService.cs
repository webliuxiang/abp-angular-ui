using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IO;
using Abp.IO.Extensions;
using Abp.Linq.Extensions;
using L._52ABP.Common.Net.MimeTypes;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.Modules.FileManager;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.DomainService;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


// ReSharper disable CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 文件应用层服务的接口实现方法
    ///</summary>
    public class SysFileAppService : YoyoCmsTemplateAppServiceBase, ISysFileAppService
    {
        private readonly IRepository<SysFile, Guid> _sysFileRepository;
        private readonly ISysFileManager _sysFileManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppFolder _appFolder;

        /// <summary>
        /// 构造函数
        ///</summary>
        public SysFileAppService(IRepository<SysFile, Guid> sysFileRepository, ISysFileManager sysFileManager,
                                 IHttpContextAccessor httpContextAccessor, IAppFolder appFolder)
        {
            _sysFileRepository = sysFileRepository;
            _sysFileManager = sysFileManager;
            _httpContextAccessor = httpContextAccessor;
            _appFolder = appFolder;

            // httpContextAccessor.HttpContext.Request.
        }

        /// <summary>
        /// 获取文件的分页列表信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(SysFilePermissions.Query)]
        public async Task<PagedResultDto<SysFileListDto>> GetPaged(GetSysFilesInput input)
        {
            var query = _sysFileManager.QuerySysFiles();

            if (input.FilterText.IsNullOrWhiteSpace())
            {
                //如果 FilterText有值 ,说明是搜索功能，进行全表的查询，
                //parentId与过滤查询不能同时存在，这样会影响搜索结果。

                query = query.Where(a => a.ParentId == input.parentId);
            }
            else
            {
                query = query
                            //模糊搜索文件名
                            .Where(a => a.Name.Contains(input.FilterText))
                            //模糊搜索原文件名
                            .Where(a => a.FileName.Contains(input.FilterText))
                            //模糊搜索路径
                            .Where(a => a.Path.Contains(input.FilterText))
                            //模糊搜索文件格式
                            .Where(a => a.FileExt.Contains(input.FilterText))
                            //模糊搜索文件类型
                            .Where(a => a.ContentType.Contains(input.FilterText));
            }

            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var sysFileList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var sysFileListDtos = ObjectMapper.Map<List<SysFileListDto>>(sysFileList);

            return new PagedResultDto<SysFileListDto>(count, sysFileListDtos);
        }

        /// <summary>
        /// 通过指定id获取SysFileListDto信息
        /// </summary>
        [AbpAuthorize(SysFilePermissions.Query)]
        public async Task<SysFileListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _sysFileRepository.GetAsync(input.Id);

            var dto = ObjectMapper.Map<SysFileListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 获取编辑 文件
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(SysFilePermissions.Create, SysFilePermissions.Edit)]
        public async Task<GetSysFileForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetSysFileForEditOutput();
            SysFileEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _sysFileRepository.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<SysFileEditDto>(entity);
            }
            else
            {
                editDto = new SysFileEditDto();
            }

            output.SysFile = editDto;
            return output;
        }

        /// <summary>
        /// 新增文件
        /// </summary>
        public virtual async Task Create(Guid? parentId)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var upfiles = _httpContextAccessor.HttpContext.Request.Form.Files;

            //根据Code节点来保存

            if (upfiles != null && upfiles.Count > 0)
            {
                foreach (var item in upfiles)
                {
                    var entity = await _sysFileManager.ProcessUploadedFileAsync(item);
                    if (parentId.HasValue)
                    {
                        entity.ParentId = parentId;
                    }
                    //调用领域服务
                    await _sysFileManager.CreateAsync(entity);
                }
            }

            //var dto = ObjectMapper.Map<SysFileEditDto>(entity);
        }

        /// <summary>
        /// 编辑文件
        /// </summary>
        [AbpAuthorize(SysFilePermissions.Edit)]
        public virtual async Task Update(SysFileEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _sysFileRepository.GetAsync(input.Id.Value);

            ObjectMapper.Map(input, entity);
            await _sysFileManager.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除文件信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        [AbpAuthorize(SysFilePermissions.Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            var entity = await _sysFileManager.FindByIdAsync(input.Id);

            if (entity != null)
            {
                var rootFolder = _appFolder.SysFileRootFolder;
                var currentPath = Path.Combine(rootFolder, entity.Path);
                if (entity.Dir)
                {
                    if (Directory.Exists(currentPath))
                    {
                        Directory.Delete(currentPath);
                    }
                }
                else
                {
                    FileHelper.DeleteIfExists(currentPath);
                }
                await _sysFileManager.DeleteAsync(entity.Id);
            }
        }

        /// <summary>
        /// 批量删除SysFile的方法
        /// </summary>
        [AbpAuthorize(SysFilePermissions.BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            foreach (var item in input)
            {
                await Delete(new EntityDto<Guid> { Id = item });
            }
        }

        //// custom codes
   
      

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public async Task ReFileName(SysFileEditDto input)
        {
            var entity = await _sysFileRepository.GetAsync(input.Id.Value);
            var rootFolder = _appFolder.SysFileRootFolder;
            entity.Name = input.Name;//新文件名
            await _sysFileManager.ValidateSysfileAsync(entity);

            var OldPath = Path.Combine(rootFolder, entity.Path);//获取旧当前文件的绝对路径
            var saveRPath = Path.Combine(entity.DateDirctoryName, entity.Name);//将重命名的路径保存到数据库中
            var NewPath = Path.Combine(rootFolder, saveRPath);//即将重命名的路径
            if (entity.Dir)
            {
                //文件夹重命名操作
                Directory.Move(OldPath, NewPath);
            }
            else
            {
                File.Move(OldPath, NewPath);//重命名操作
            }

            saveRPath = saveRPath.Replace("\\", "/");

            entity.Path = saveRPath;
            await _sysFileManager.UpdateAsync(entity);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public async Task<SysFileListDto> CopyFile(SysFileEditDto input)
        {
            var entity = await _sysFileRepository.GetAsync(input.Id.Value);
            var rootFolder = _appFolder.SysFileRootFolder;
            var currentFilePath = Path.Combine(rootFolder, entity.Path);//获取旧当前文件的绝对路径
            var uniqueFileName = SequentialGuidGenerator.Instance.Create().ToString();
            var fileExt = Path.GetExtension(currentFilePath);
            var newName = uniqueFileName + fileExt; //copy文件后的新名称
            entity.Name = newName;
            var saveRPath = Path.Combine(entity.DateDirctoryName, entity.Name);//将重命名的路径保存到数据库中
            var NewFilepath = Path.Combine(rootFolder, saveRPath);//即将复制的路径
            saveRPath = saveRPath.Replace("\\", "/");

            entity.Path = saveRPath;
            File.Copy(currentFilePath, NewFilepath);

            var newEntity = entity;
            newEntity.Id = SequentialGuidGenerator.Instance.Create();
            newEntity = await _sysFileManager.CreateAsync(newEntity);

            var dto = ObjectMapper.Map<SysFileListDto>(newEntity);
            return dto;
        }

        /// <summary>
        /// 获取文件夹列表
        /// </summary>
        /// <returns> </returns>
        public async Task<List<SysFileListDto>> GetDirectories()
        {
            var list = await _sysFileRepository.GetAllListAsync(a => a.Dir == true);

            var sysFileListDtos = ObjectMapper.Map<List<SysFileListDto>>(list);

            return sysFileListDtos;
        }

        /// <summary>
        /// 移动文件夹和文件
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public async Task<SysFileListDto> MoveAsync(MoveSysFilesInput input)
        {
            await _sysFileManager.MoveAsync(input.Id, input.NewParentId);

            var entity = await _sysFileRepository.GetAsync(input.Id);

            return ObjectMapper.Map<SysFileListDto>(entity);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public async Task<SysFileListDto> CreateDirectory(SysFileEditDto input)
        {
            input.Dir = true;
            var entity = ObjectMapper.Map<SysFile>(input);
            entity.IsImg = false;
            var rootFolder = _appFolder.SysFileRootFolder;
            var dateDirctoryName = DateTime.Now.ToString("yyyy-MM-dd"); //2008-9-4
            var savePath = Path.Combine(dateDirctoryName, entity.Name);
            var dirPath = Path.Combine(rootFolder, savePath);
            DirectoryHelper.CreateIfNotExists(dirPath);
            savePath = savePath.Replace("\\", "/");
            entity.DateDirctoryName = dateDirctoryName;
            entity.Path = savePath;
            entity.FileExt = ".dir";

            entity = await _sysFileManager.CreateAsync(entity);
            var dto = ObjectMapper.Map<SysFileListDto>(entity);
            return dto;
        }

        //// custom codes end
    }
}
