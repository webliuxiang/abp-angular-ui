using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.Dtos
{
    public class VideoToCourseInput
    {

        [Range(0, long.MaxValue)] public long CourseId { get; set; }

        public List<long> VideoIdList { get; set; }

    }
}
