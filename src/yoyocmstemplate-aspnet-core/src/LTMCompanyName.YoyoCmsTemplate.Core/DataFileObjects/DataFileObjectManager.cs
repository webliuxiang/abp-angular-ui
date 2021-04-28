using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.DataFileObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class DataFileObjectManager : IDataFileObjectManager, ITransientDependency
    {
        private readonly IRepository<DataFileObject, Guid> _binaryObjectRepository;

        public DataFileObjectManager(IRepository<DataFileObject, Guid> binaryObjectRepository)
        {
            _binaryObjectRepository = binaryObjectRepository;
        }

        public Task<DataFileObject> GetOrNullAsync(Guid id)
        {
            return _binaryObjectRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Id == id);
        }

        public Task SaveAsync(DataFileObject file)
        {
            return _binaryObjectRepository.InsertAsync(file);
        }

        public Task DeleteAsync(Guid id)
        {
            return _binaryObjectRepository.DeleteAsync(id);
        }

    }
}
