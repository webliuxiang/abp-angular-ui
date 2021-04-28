using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.Dtos
{
    public class FindVideoResInput : PagedAndFilteredInputDto
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        public long CourseId { get; set; }
    }
}
