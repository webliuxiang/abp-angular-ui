using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members.DomainService
{
    /// <summary>
    /// Member领域层的业务管理
    ///</summary>
    public class MemberManager : YoyoCmsTemplateDomainServiceBase, IMemberManager
    {

        private readonly IRepository<Member, long> _repository;

        /// <summary>
        /// Member的构造方法
        ///</summary>
        public MemberManager(
            IRepository<Member, long> repository
        )
        {
            _repository = repository;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitMember()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码


        public async Task CreateMember(Member input)
        {
            // 这个userId做了唯一约束，所以重复会报错
            await _repository.InsertAsync(input);
        }

        public async Task<Member> GetMemberByUserId(long? userId)
        {
            var entity = await _repository.GetAll()
                .Where(o => o.UserId == userId.Value)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task Update(Member input)
        {
            var entity = await _repository.GetAsync(input.Id);

            // 关联用户Id不能被修改
            input.UserId = entity.UserId;

            await _repository.UpdateAsync(input);
        }
    }
}
