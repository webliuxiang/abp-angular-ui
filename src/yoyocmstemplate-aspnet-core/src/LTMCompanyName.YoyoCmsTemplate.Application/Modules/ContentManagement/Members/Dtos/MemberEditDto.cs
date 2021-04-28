namespace  LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members.Dtos
{
    public class MemberEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }         


        
		/// <summary>
		/// 地址
		/// </summary>
		public string Address { get; set; }



		/// <summary>
		/// 关于我
		/// </summary>
		public string AboutMe { get; set; }



		/// <summary>
		/// 电话号码
		/// </summary>
		public string PhoneNumber { get; set; }



		/// <summary>
		/// 微信号码
		/// </summary>
		public string Wechat { get; set; }



		/// <summary>
		/// QQ号码
		/// </summary>
		public string QQ { get; set; }



		/// <summary>
		/// GitHub地址
		/// </summary>
		public string GitHub { get; set; }



		/// <summary>
		/// 码云地址
		/// </summary>
		public string Gitee { get; set; }


        /// <summary>
        /// 关联的用户Id
        /// </summary>
        public long? UserId { get; set; }
    }
}
