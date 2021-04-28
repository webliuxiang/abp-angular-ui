using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos
{

    /// <summary>
    /// 移动课程分类的dto
    /// </summary>
    public class MoveCourseCategoryInput
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        public long? NewParentId { get; set; }
    }
}
