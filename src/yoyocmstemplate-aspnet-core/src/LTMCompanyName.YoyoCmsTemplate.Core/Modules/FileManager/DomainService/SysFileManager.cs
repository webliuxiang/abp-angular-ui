using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IO;
using Abp.IO.Extensions;
using Abp.UI;
using L._52ABP.Common.Net.MimeTypes;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.Modules.FileManager;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.DomainService
{
    /// <summary>
    /// 文件领域服务层一个模块的核心业务逻辑
    ///</summary>
    public class SysFileManager : YoyoCmsTemplateDomainServiceBase, ISysFileManager
    {
        private readonly IRepository<SysFile, Guid> _sysFileRepository;
        private readonly IAppFolder _appFolder;

        /// <summary>
        /// SysFile的构造方法
        /// 通过构造函数注册服务到依赖注入容器中
        ///</summary>
        public SysFileManager(IRepository<SysFile, Guid> sysFileRepository, IAppFolder appFoler)
        {
            _sysFileRepository = sysFileRepository;
            _appFolder = appFoler;
        }

        #region 查询判断的业务

        /// <summary>
        /// 返回表达式数的实体信息即IQueryable类型
        /// </summary>
        /// <returns> </returns>
        public IQueryable<SysFile> QuerySysFiles()
        {
            return _sysFileRepository.GetAll();
        }

        /// <summary>
        /// 返回即IQueryable类型的实体，不包含EF Core跟踪标记
        /// </summary>
        /// <returns> </returns>
        public IQueryable<SysFile> QuerySysFilesAsNoTracking()
        {
            return _sysFileRepository.GetAll().AsNoTracking();
        }

        /// <summary>
        /// 根据Id查询实体信息
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public async Task<SysFile> FindByIdAsync(Guid id)
        {
            var entity = await _sysFileRepository.GetAsync(id);
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public async Task<bool> IsExistAsync(Guid id)
        {
            var result = await _sysFileRepository.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        #endregion 查询判断的业务

        public async Task<SysFile> CreateAsync(SysFile entity)
        {
            //获取实体的Code码
            entity.Code = await GetNextChildCodeAsync(entity.ParentId);
            await ValidateSysfileAsync(entity);
            entity.Id = await _sysFileRepository.InsertAndGetIdAsync(entity);

            return entity;
        }

        public async Task UpdateAsync(SysFile entity)
        {
            await _sysFileRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var rootFolder = _appFolder.SysFileRootFolder;
            var children = await FindChildrenAsync(id, true);
            foreach (var entity in children)
            {
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
                await _sysFileRepository.DeleteAsync(entity);
            }
            await _sysFileRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        public async Task BatchDelete(List<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _sysFileRepository.DeleteAsync(a => input.Contains(a.Id));
        }

        //// custom codes

        public virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild == null)
            {
                var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
                return SysFile.AppendCode(parentCode, SysFile.CreateCode(1));
            }

            return SysFile.CalculateNextCode(lastChild.Code);
        }

        /// <summary>
        /// 获取子集信息，可能为null
        /// </summary>
        /// <param name="parentId"> </param>
        /// <returns> </returns>
        public virtual async Task<SysFile> GetLastChildOrNullAsync(Guid? parentId)
        {
            var children = await _sysFileRepository.GetAllListAsync(sf => sf.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        /// <summary>
        /// 获取Code码
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public virtual async Task<string> GetCodeAsync(Guid id)
        {
            return (await _sysFileRepository.GetAsync(id)).Code;
        }

        /// <summary>
        /// 获取子集
        /// </summary>
        /// <param name="parentId"> </param>
        /// <param name="recursive"> </param>
        /// <returns> </returns>
        public async Task<List<SysFile>> FindChildrenAsync(Guid? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await _sysFileRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            }

            if (!parentId.HasValue)
            {
                return await _sysFileRepository.GetAllListAsync();
            }

            var code = await GetCodeAsync(parentId.Value);

            return await _sysFileRepository.GetAllListAsync(
                ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value
            );
        }

        /// <summary>
        /// 验证文件是否符合
        /// </summary>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        public async Task ValidateSysfileAsync(SysFile entity)
        {
            await Task.Yield();

            //var siblings = (await FindChildrenAsync(entity.ParentId))
            //     .Where(ou => ou.Id != entity.Id)
            //     .ToList();

            var result = QuerySysFiles().Any(a => a.Name == entity.Name);
            if (result)
            {
                throw new UserFriendlyException(L("SysfileDuplicateDisplayNameWarning", entity.FileName));
            }

            //对文件进行验证//todo：
        }

        public async Task MoveAsync(Guid id, Guid? parentId)
        {
            var entity = await _sysFileRepository.GetAsync(id);

            if (entity.ParentId == parentId)
            {
                return;
            }

            var children = await FindChildrenAsync(id, true);

            var oldCode = entity.Code;
            //开始移动

            entity.Code = await GetNextChildCodeAsync(parentId);

            entity.ParentId = parentId;
            // await ValidateSysfileAsync(entity);

            foreach (var child in children)
            {
                child.Code = SysFile.AppendCode(entity.Code, SysFile.GetRelativeCode(child.Code, oldCode));
            }
        }

         /// <summary>
        /// 处理上传的文件流
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<SysFile> ProcessUploadedFileAsync(IFormFile file,bool isHidden=false)
        {
            var rootFolder = _appFolder.SysFileRootFolder;
            var dateDirctoryName = DateTime.Now.ToString("yyyy-MM-dd"); //2008-9-4
            var uniqueFileName = SequentialGuidGenerator.Instance.Create().ToString() + Path.GetFileName(file.FileName);

            var dateDirctoryPath = Path.Combine(rootFolder, dateDirctoryName);

            if (isHidden)
            {
                dateDirctoryName = Path.Combine(dateDirctoryName, "hidden");
               dateDirctoryPath = Path.Combine(dateDirctoryPath, "hidden");

            }



            if (!Directory.Exists(dateDirctoryPath))
            {
                Directory.CreateDirectory(dateDirctoryPath);
            }

            var saveRPath = Path.Combine(dateDirctoryName, uniqueFileName);//相对路径保存到数据库中  /
            saveRPath = saveRPath.Replace("\\", "/");
            var itempath = Path.Combine(rootFolder, saveRPath);

            var entity = new SysFile
            {
                ContentType = file.ContentType,
                DateDirctoryName = dateDirctoryName,
                Path = saveRPath,
                Dir = false,
                FileExt = Path.GetExtension(file.FileName),
                Name = uniqueFileName,
                FileName = file.FileName,
                Size = file.Length,
                FormattedSize = GetAutoSizeString(file.Length),
                IsHidden = isHidden
            };

            //因为使用了非托管资源，所以需要手动进行释放
            using (var fileStream = new FileStream(itempath, FileMode.Create))
            {
                //使用IFormFile接口提供的CopyTo()方法将文件复制到文件夹
                await file.CopyToAsync(fileStream);
            }
            byte[] fileBytes;
            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
            }

            if (file.ContentType.IsIn(MimeTypeNames.ImagePng, MimeTypeNames.ImageGif, MimeTypeNames.ImageJpeg, MimeTypeNames.ImagePjpeg, "image/bmp"))
            {
                using (var bmpImage = new Bitmap(new MemoryStream(fileBytes)))
                {   //图片 获取高度 宽度
                    // MimeTypeNames
                    entity.Width = bmpImage.Width;
                    entity.Height = bmpImage.Height;
                    entity.IsImg = true;
                }
            }

            return entity;
        }


        /// <summary>
        /// 项目文档上传的文件流
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<SysFile> ProcessUploadedFileForProjectAsync(IFormFile file)
        {
            var rootFolder = _appFolder.ProjectRootFolder;
            var dateDirctoryName = DateTime.Now.ToString("yyyy-MM-dd");
            var uniqueFileName = SequentialGuidGenerator.Instance.Create().ToString() + Path.GetFileName(file.FileName);
            var dateDirctoryPath = Path.Combine(rootFolder, dateDirctoryName);

            if (!Directory.Exists(dateDirctoryPath))
            {
                Directory.CreateDirectory(dateDirctoryPath);
            }

            var saveRPath = Path.Combine(dateDirctoryName, uniqueFileName);//相对路径保存到数据库中  /
            saveRPath = saveRPath.Replace("\\", "/");
            var itempath = Path.Combine(rootFolder, saveRPath);

            var entity = new SysFile
            {
                ContentType = file.ContentType,
                DateDirctoryName = dateDirctoryName,
                Path = saveRPath,
                Dir = false,
                FileExt = Path.GetExtension(file.FileName),
                Name = uniqueFileName,
                FileName = file.FileName,
                Size = file.Length,
                FormattedSize = GetAutoSizeString(file.Length)
            };

            //因为使用了非托管资源，所以需要手动进行释放
            using (var fileStream = new FileStream(itempath, FileMode.Create))
            {
                //使用IFormFile接口提供的CopyTo()方法将文件复制到文件夹
                await file.CopyToAsync(fileStream);
            }
            byte[] fileBytes;
            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
            }

            if (file.ContentType.IsIn(MimeTypeNames.ImagePng, MimeTypeNames.ImageGif, MimeTypeNames.ImageJpeg, MimeTypeNames.ImagePjpeg, "image/bmp"))
            {
                using (var bmpImage = new Bitmap(new MemoryStream(fileBytes)))
                {   //图片 获取高度 宽度
                    // MimeTypeNames
                    entity.Width = bmpImage.Width;
                    entity.Height = bmpImage.Height;
                    entity.IsImg = true;
                }
            }

            return entity;
        }

        /// <summary>
        /// 获取文件的大小
        /// </summary>
        /// <param name="b"> </param>
        /// <returns> </returns>
        public static string GetAutoSizeString(long b)
         {
             const int GB = 1024 * 1024 * 1024;
             const int MB = 1024 * 1024;
             const int KB = 1024;

             if (b / GB >= 1)
             {
                 return Math.Round(b / (float)GB, 2) + "GB";
             }

             if (b / MB >= 1)
             {
                 return Math.Round(b / (float)MB, 2) + "MB";
             }

             if (b / KB >= 1)
             {
                 return Math.Round(b / (float)KB, 2) + "KB";
             }

             return b + "B";
         }

        //// custom codes end
    }
}
