using System.Threading.Tasks;
using Abp.Domain.Services;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members.DomainService
{
    public interface IMemberManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitMember();


        /// <summary>
        /// 创建会员信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateMember(Member input);

        /// <summary>
        /// 根据用户id获取会员信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Member> GetMemberByUserId(long? userId);

        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Update(Member input);
    }
}
