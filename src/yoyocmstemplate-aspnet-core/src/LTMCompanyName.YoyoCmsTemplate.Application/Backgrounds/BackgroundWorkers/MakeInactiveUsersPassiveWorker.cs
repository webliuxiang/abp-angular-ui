using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Impersonation;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Backgrounds.BackgroundWorkers
{


    /// <summary>
    /// 锁定30天内未登陆的用户。
    /// </summary>
    public class MakeInactiveUsersPassiveWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<User, long> _userRepository;


        private readonly ImpersonationManager _impersonationManager;


        public MakeInactiveUsersPassiveWorker(AbpTimer timer, IRepository<User, long> userRepository, ImpersonationManager impersonationManager)
            : base(timer)
        {
            _userRepository = userRepository;
            Timer.Period = 5000; //5秒（适合测试，但一般情况下会多一些)
            _impersonationManager = impersonationManager;
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var oneMonthAgo = Clock.Now.Subtract(TimeSpan.FromDays(30));

                var inactiveUsers = _userRepository.GetAllList(u =>
                    u.IsActive &&
                    ((u.LastModificationTime < oneMonthAgo && u.LastModificationTime != null) || (u.CreationTime < oneMonthAgo && u.LastModificationTime == null))
                    );

                foreach (var inactiveUser in inactiveUsers)
                {
                    inactiveUser.IsActive = false;
                    Logger.Info(inactiveUser + "由于他（她）在过去30天内没有登录，所以被锁定了。");
                }

                CurrentUnitOfWork.SaveChanges();
            }
        }
    }
}
