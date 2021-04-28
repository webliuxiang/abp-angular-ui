using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.DataFileObjects
{
    /// <summary>
    /// 二进制对象
    /// </summary>

    public class DataFileObject : Entity<Guid>, IMayHaveTenant
    {

        public virtual int? TenantId { get; set; }

       


        [Required]
        public virtual byte[] Bytes { get; set; }




        public DataFileObject()
        {
            Id = SequentialGuidGenerator.Instance.Create();
        }

        public DataFileObject(int? tenantId, byte[] bytes)
            : this()
        {
            TenantId = tenantId;
            Bytes = bytes;
        }
    }
}
