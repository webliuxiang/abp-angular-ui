using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp;
using Abp.Domain.Repositories;

using L._52ABP.Common.Extensions;

using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections.DomainService;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.DomainService
{
    /// <summary>
    ///     Course领域层的业务管理
    /// </summary>
    public class CourseManager : AbpDomainService<Course>
    {
        private readonly IRepository<Order, Guid> _orderRepository;

        private readonly CourseSectionManager _courseSectionManager;

        public CourseManager(IRepository<Course, long> entityRepo, IRepository<Order, Guid> orderRepository, CourseSectionManager courseSectionManager) : base(entityRepo)
        {
            _orderRepository = orderRepository;
            _courseSectionManager = courseSectionManager;
        }

        /// <summary>
        /// 课程章节管理器
        /// </summary>
        public virtual CourseSectionManager CourseSections => _courseSectionManager;

        /// <summary>
        ///     检查用户是否购买了课程
        /// </summary>
        /// <param name="courseCode"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserBuyCourse(string courseCode)
        {
            if (!AbpSession.UserId.HasValue)
            {
                return false;
            }

            var entity = await _orderRepository.FirstOrDefaultAsync(a =>
                a.ProductCode == courseCode && a.Status == OrderStatusEnum.ChangeHands &&
                a.UserId == AbpSession.UserId.Value);

            return entity != null;
        }

        /// <summary>
        /// 生成课程编码
        /// 规则: 来源(2位)-支付方式(2位)-时间戳(秒级)-属于用户当日订单数量+1(3位)
        /// </summary>
        /// <param name="sourceType">订单来源</param>
        /// <param name="time">属于用户当日订单数量</param>
        /// <returns></returns>
        public string GetCourseMaxCode(OrderSourceType sourceType, DateTime? time = null)
        {
            var precode = RandomHelper.GetRandom(1, 9) + Convert.ToInt32(sourceType).ToString();

            var date = time ?? DateTime.Now;
            var codeStateWith = precode + date.ToString("yyyyMMdd"); //截取数据格式：年-月-日 20191021

            var code = codeStateWith;

            var list = QueryAsNoTracking.Where(e => e.CourseCode.StartsWith(codeStateWith))
                .ToList();
            var model = list.Select(e => new { Number = e.CourseCode.Substring(code.Length).CastTo(0) })
                .OrderByDescending(e => e.Number).FirstOrDefault(); //返回订单的最后一位

            if (model != null)
            {
                code += (model.Number + 1).ToString().PadLeft(2, '0');
            }
            else
            {
                code += "01";
            }

            return code;
        }

        /// <summary>
        /// 删除课程,并删除对应的章节
        /// </summary>
        /// <param name="id">课程id</param>
        /// <returns></returns>
        public override async Task Delete(long id)
        {
            await base.Delete(id);
            await this.CourseSections.DeleteByCourseId(id);
        }

        /// <summary>
        /// 批量删除课程,并删除对应的章节
        /// </summary>
        /// <param name="idList">课程id集合</param>
        /// <returns></returns>
        public override async Task Delete(List<long> idList)
        {
            await base.Delete(idList);
            await this.CourseSections.DeleteByCourseIds(idList);
        }

        /// <summary>
        /// 根据课程id获取章节
        /// </summary>
        /// <param name="courseId">课程id</param>
        /// <returns>章节集合</returns>
        public async Task<List<CourseSection>> GetSectionsByCourseId(long courseId)
        {
            var entityList = await CourseSections.QueryAsNoTracking
                .Where(o => o.CoursesId == courseId)
                .ToListAsync();

            return entityList;
        }
    }
}
