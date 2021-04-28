using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using Masuit.Tools;

namespace LTMCompanyName.YoyoCmsTemplate.Backgrounds.BackgroundWorkers
{

    /// <summary>
    /// 定时删除审计日志，减少数据库的压力
    /// </summary>
    public class DeleteAuditLogsRegularBasisWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {

        private readonly IRepository<AuditLog, long> _auditLogRepository;
        public DeleteAuditLogsRegularBasisWorker(AbpTimer timer, IRepository<AuditLog, long> auditLogRepository) : base(timer)
        {

            //1000 等于 1秒
            // 24*60*60*5 = 432000  //5天间隔下
            //60*60*24 =86400    1天
             timer.Period = 86400;
            _auditLogRepository = auditLogRepository;
        }
        [UnitOfWork]
        protected override void DoWork()
        {
            var twoMonthAge = Clock.Now.Subtract(TimeSpan.FromDays(60));
            //删除两个月前的日志数据内容。
            var audits = _auditLogRepository.GetAll().Where(a => a.ExecutionTime < twoMonthAge).ToList();


            var dbdd = CurrentUnitOfWork.GetDbContext<YoyoCmsTemplateDbContext>();
            dbdd.AuditLogs.RemoveRange(audits);

            //foreach (var item in audits)
            //{



            //_auditLogRepository.Delete(item);
            //}

            CurrentUnitOfWork.SaveChanges();


        }
    }
}
