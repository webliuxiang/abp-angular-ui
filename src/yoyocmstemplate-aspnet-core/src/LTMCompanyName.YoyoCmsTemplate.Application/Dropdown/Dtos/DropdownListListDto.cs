using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;


namespace LTMCompanyName.YoyoCmsTemplate.Dropdown.Dtos
{
    /// <summary>
    /// 下拉组件的列表DTO
    /// <see cref="DropdownList"/>
    /// </summary>
    public class DropdownListListDto : EntityDto<string> 
    {

        
		/// <summary>
		/// DDType_Id
		/// </summary>
		public string DDType_Id { get; set; }



		/// <summary>
		/// Name_TX
		/// </summary>
		[Required(ErrorMessage="Name_TX不能为空")]
		public string Name_TX { get; set; }



		/// <summary>
		/// IsActive_YN
		/// </summary>
		public bool IsActive_YN { get; set; }



		/// <summary>
		/// Parent
		/// </summary>
		public DropdownType Parent { get; set; }



		/// <summary>
		/// ParentId
		/// </summary>
		public string ParentId { get; set; }



		/// <summary>
		/// ParentIdList
		/// </summary>
		public string ParentIdList { get; set; }



	
    }
}
