using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseSections.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Portals;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market
{
    public class PortalMarketAppService : YoyoCmsTemplateAppServiceBase, IPortalMarketAppService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Course, long> _courseRepository;
        private readonly CourseManager _courseManager;
        private readonly CourseClassHourManager _classHourManager;

        public PortalMarketAppService(IRepository<Product, Guid> productRepository, IRepository<Course, long> courseRepository, CourseManager courseManager, CourseClassHourManager classHourManager)
        {
            _productRepository = productRepository;
            _courseRepository = courseRepository;
            _courseManager = courseManager;
            _classHourManager = classHourManager;
        }

        /// <summary>
        ///  服务于发布后的产品列表，服务门户
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductListDto>> GetPublishedProductforHomePriceAsync()
        {
            var entities = await _productRepository.GetAll()
                .AsNoTracking().Where(o => o.Published)
                .ToListAsync();

            var dtos = ObjectMapper.Map<List<ProductListDto>>(entities);

            //    var dtos = entities.MapTo<List<ProductListDto>>();

            return dtos;
        }

        public async Task<List<CourseListDto>> GetPublishedCourseListforHomeAsync()
        {
            var entityList = await _courseRepository.GetAll().Where(a => a.CourseState == CourseStateEnum.Publish).ToListAsync();

            List<CourseListDto> dtos = new List<CourseListDto>();
            if (entityList.Count > 0)
            {
                dtos = ObjectMapper.Map<List<CourseListDto>>(entityList);
            }

            return dtos;
        }

        public async Task<CourseDetailsDto> GetCourseDetailsIncludeSections(long courseId)
        {
            var course = await _courseManager.FindById(courseId);

            if (course==null)
            {

                throw new UserFriendlyException($"课程{courseId}的信息不存在，您来到了知识的荒芜地带。");
            }

            var dto = ObjectMapper.Map<CourseDetailsDto>(course);

            var courseSections = await _courseManager.GetSectionsByCourseId(course.Id);

            var sectionDtos = ObjectMapper.Map<List<CourseSectionDetailsDto>>(courseSections.OrderBy(o => o.Index));
           

            foreach (var item in sectionDtos)
            {
                var courseClassHours = await _classHourManager.GetBySectionId(item.Id.Value);

             var courseClassHourDtos =    ObjectMapper.Map<List<CourseClassHourDto>>(courseClassHours.OrderBy(o => o.SortNumber));

                item.CourseClassHours = courseClassHourDtos;
            }


            dto.CourseSections = sectionDtos;


            return dto;
        }
    }
}
